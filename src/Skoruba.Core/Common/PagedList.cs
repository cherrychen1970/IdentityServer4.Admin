using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace Skoruba.Core.Dtos.Common
{
    public interface IPagedList<T> : IList<T>
    {
        int PageSize { get; set; }
        int TotalCount { get; set; }        
    }
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        public PagedList()
        {}

        public PagedList(IEnumerable<T> list)
        {
            AddRange(list);
        }
        public PagedList(IEnumerable<T> list, int pageSize, int totalCount)
        {
            AddRange(list);            
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }
    }

    static public class Extensions {
        static public IPagedList<TResult> Select<TSource,TResult>(this IPagedList<TSource> pagedList, Func<TSource, TResult> selector)        
        {
            var list = pagedList.Select(selector);
            return new PagedList<TResult>(list,pagedList.PageSize,pagedList.TotalCount);
        }
    }
}
