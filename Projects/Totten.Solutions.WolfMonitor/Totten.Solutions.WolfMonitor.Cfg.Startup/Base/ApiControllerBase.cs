using AutoMapper.QueryableExtensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Exceptions;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Base
{
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        protected Guid UserId => Guid.Parse(GetClaimValue("UserId"));
        protected Guid CompanyId => Guid.Parse(GetClaimValue("CompanyId"));
        protected string Role => GetClaimValue("Role");
        #region Handlers    
        protected IActionResult HandleCommand<TFailure, TSuccess>(Result<TFailure, TSuccess> result) where TFailure : Exception
                            => result.IsFailure ? HandleFailure(result.Failure) : Ok(result.Success);

        protected async Task<IActionResult> HandleQueryable<TQueryOptions, TResult>(
                Result<Exception, IQueryable<TQueryOptions>> result,
                ODataQueryOptions<TQueryOptions> queryOptions)
        {
            if (result.IsFailure)
                return HandleFailure(result.Failure);

            return Ok(await HandlePageResult<TQueryOptions, TResult>(result.Success, queryOptions));
        }

        protected async Task<PageResult<TResult>> HandlePageResult<TQueryOptions, TResult>
            (IQueryable<TQueryOptions> query, ODataQueryOptions<TQueryOptions> queryOptions)
        {
            var queryResults = queryOptions.ApplyTo(query);

            List<TResult> list;

            try
            {
                list = await queryResults.ProjectToListAsync<TResult>();
            }
            catch
            {
                list = queryResults.ProjectTo<TResult>().ToList();
            }

            var pageResult = new PageResult<TResult>(list,
                                                    Request.HttpContext.ODataFeature().NextLink,
                                                    Request.HttpContext.ODataFeature().TotalCount);
            return pageResult;
        }

        protected IActionResult HandleFailure<T>(T exceptionToHandle) where T : Exception
        {
            if (exceptionToHandle is ValidationException)
                return StatusCode(HttpStatusCode.BadRequest.GetHashCode(), (exceptionToHandle as ValidationException).Errors);

            var exceptionPayload = ExceptionPayload.New(exceptionToHandle);
            return exceptionToHandle is BusinessException ?
                StatusCode(exceptionPayload.ErrorCode.GetHashCode(), exceptionPayload) :
                StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), exceptionPayload);
        }

        protected IActionResult HandleValidationFailure<T>(IList<T> validationFailure) where T : ValidationFailure
            => StatusCode(HttpStatusCode.BadRequest.GetHashCode(), validationFailure);

        protected IActionResult HandleQueryableInMemory<TQueryOptions, TResult>(
                Result<Exception, IQueryable<TQueryOptions>> result,
                ODataQueryOptions<TQueryOptions> queryOptions,
                bool enableCaseInsensitive = false)
        {
            if (result.IsFailure)
                return HandleFailure(result.Failure);

            if (enableCaseInsensitive)
                queryOptions = queryOptions.EnableCaseInsensitive();

            var queryResults = queryOptions.ApplyTo(result.Success);
            var list = queryResults.ProjectTo<TResult>().ToList();

            return Ok(new PageResult<TResult>(list, Request.HttpContext.ODataFeature().NextLink, Request.HttpContext.ODataFeature().TotalCount));
        }

        #endregion

        #region Utils
        protected string GetCustomHeaderValue(string key)
        {
            StringValues headerValues;

            if (Request.Headers.TryGetValue(key, out headerValues))
            {
                return headerValues.FirstOrDefault();
            }

            return null;
        }

        private string GetClaimValue(string type)
                => ((ClaimsIdentity)HttpContext.User.Identity).FindFirst(type)?.Value;

        protected IActionResult HandleQuery<TSource, TResult>(Result<Exception, TSource> result)
            => result.IsSuccess ? Ok(AutoMapper.Mapper.Map<TSource, TResult>(result.Success)) : HandleFailure(result.Failure);

        #endregion
    }
}
