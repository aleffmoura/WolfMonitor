using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.OData;
using Microsoft.OData.UriParser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions
{
    public static class QueryOptionsExtensions
    {
        public static ODataQueryOptions<T> EnableCaseInsensitive<T>(this ODataQueryOptions<T> queryOptions)
        {
            if (queryOptions.Filter != null)
            {
                var query = queryOptions.RewriteQuery();
                queryOptions.Request.QueryString = new QueryString(query);

                return new ODataQueryOptions<T>(queryOptions.Context, queryOptions.Request);
            }

            return queryOptions;
        }

        #region Private Methods

        private static string RewriteQuery(this ODataQueryOptions queryOptions)
        {
            var odataUri = queryOptions.BuildODataUri();
            var uri = odataUri.BuildUri(ODataUrlKeyDelimiter.Slash);

            return uri.Query;
        }

        /// <summary>
        /// Recria a URI OData para contemplar as alterações aplicadas no filtro
        /// </summary>
        private static ODataUri BuildODataUri(this ODataQueryOptions queryOptions)
        {
            var uri = new ODataUri()
            {
                ServiceRoot = new Uri(queryOptions.Request.GetUri().AbsoluteUri.Replace(queryOptions.Request.GetUri().PathAndQuery, string.Empty)),
                Path = new Microsoft.OData.UriParser.ODataPath(Enumerable.Empty<ODataPathSegment>()),
                Filter = queryOptions.Filter?.FilterClause.RewriteFilter(),
                OrderBy = queryOptions.OrderBy?.OrderByClause,
                SelectAndExpand = queryOptions.SelectExpand?.SelectExpandClause,
                Skip = queryOptions.Skip?.Value,
                Top = queryOptions.Top?.Value,
                QueryCount = queryOptions.Count.Value
            };

            return uri;
        }

        /// <summary>
        /// Sobrescreve o filtro aplicando Case Insensitive na Function "contains" 
        /// </summary>
        private static FilterClause RewriteFilter(this FilterClause filterClause)
        {
            if (filterClause != null)
            {
                var filterExpr = filterClause.Expression;

                if (filterExpr is BinaryOperatorNode || filterExpr is UnaryOperatorNode)
                {
                    filterExpr = filterExpr.RewriteAsCaseInsensitive();
                    filterClause = new FilterClause(filterExpr, filterClause.RangeVariable);
                }
                else if (filterExpr is SingleValueFunctionCallNode funcExpr)
                {
                    filterExpr = ParseNode(filterExpr);
                    filterClause = new FilterClause(filterExpr, filterClause.RangeVariable);
                }
            }

            return filterClause;
        }

        /// <summary>
        /// Percorre os nós da árvore buscando as funções "contains" para aplicar o Case Insensitive
        /// </summary>
        private static SingleValueNode RewriteAsCaseInsensitive(this SingleValueNode node)
        {
            if (node is BinaryOperatorNode bon)
            {
                var left = bon.Left;
                var right = bon.Right;

                if (left is SingleValueFunctionCallNode)
                {
                    left = left.ParseNode();
                }
                else if (left is BinaryOperatorNode)
                {
                    left = left.RewriteAsCaseInsensitive();
                }

                if (right is SingleValueFunctionCallNode)
                {
                    right = right.ParseNode();
                }
                else if (right is BinaryOperatorNode)
                {
                    right = right.RewriteAsCaseInsensitive();
                }

                //Reconfigura o nó com as alterações da árvore
                node = new BinaryOperatorNode(bon.OperatorKind, left, right);
            }

            return node;
        }

        /// <summary>
        /// Recria o nó da Function "contains" aplicando a Function "toupper" para ser Case Insensitive
        /// </summary>
        private static SingleValueNode ParseNode(this SingleValueNode node)
        {
            if (node is SingleValueFunctionCallNode singleValue)
            {
                var left = singleValue.Parameters.First();
                var right = singleValue.Parameters.Last();

                if (left is SingleValuePropertyAccessNode && right is ConstantNode)
                {
                    node = new SingleValueFunctionCallNode("contains",
                        new List<QueryNode>
                        {
                            new SingleValueFunctionCallNode("toupper", new List<QueryNode> { left }, ((SingleValuePropertyAccessNode) left).TypeReference),
                            new SingleValueFunctionCallNode("toupper", new List<QueryNode> { right }, ((ConstantNode) right).TypeReference)
                        },
                        node.TypeReference);
                }
            }

            return node;
        }
        private static Uri GetUri(this HttpRequest request)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.IsHttps ? request.Host.Port.GetValueOrDefault(443) : request.Host.Port.GetValueOrDefault(80),
                Path = request.Path.ToString(),
                Query = request.QueryString.ToString()
            };

            return uriBuilder.Uri;
        }
        #endregion
    }
}
