using AutoMapper.QueryableExtensions;
using DelegateDecompiler.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions
{
    public static class QueryableExtensions
    {
        public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable)
        {
            return queryable.ProjectTo<TDestination>().DecompileAsync().ToListAsync();
        }
    }
}
