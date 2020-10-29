using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace Skoruba.Models
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
}
