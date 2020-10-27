using System;
using System.Linq;
using System.Collections.Generic;


namespace Skoruba.EntityFramework.Extensions.Common
{
    public class PagedList<T> 
    {
        public PagedList()
        {
            Data = new List<T>();
        }

        public PagedList(IEnumerable<T> list)
        {
            Data = list.ToList();
        }
        public PagedList(IEnumerable<T> list, int pageSize, int totalCount)
        {
            Data = list.ToList();
            PageSize = pageSize;
            TotalCount = totalCount;
        }
        public List<T> Data { get; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }
    }


    static public class Extensions {
        static public PagedList<TResult> Select<TSource,TResult>(this PagedList<TSource> pagedList, Func<TSource, TResult> selector)
        
        {
            var list = pagedList.Data.Select(selector);

            return new PagedList<TResult>(list,pagedList.PageSize,pagedList.TotalCount);
        }
    }
}
