using System;
using System.Collections.Generic;
using System.Linq;
using Skoruba.Models;

namespace Skoruba.Models
{
	// collection of extionsion for varios model type..
	static public class Extensions
	{
        public static List<SelectItem> ToSelectList(this Type enumType)
        {			
            var values = Enum.GetValues(enumType).OfType<int>().ToList();
            return values.Select(x => new SelectItem(x.ToString(),   Enum.GetName(enumType, x))).ToList();
        }

        public static string GetEnumName<T>(this int val)
        {
            return Enum.GetName( typeof(T),val);
        }		

        static public IPagedList<TResult> Select<TSource,TResult>(this IPagedList<TSource> pagedList, Func<TSource, TResult> selector)        
        {
            var list = pagedList.Select(selector);
            return new PagedList<TResult>(list,pagedList.PageSize,pagedList.TotalCount);
        }

        static public IPagedList<T> ToPagedList<T>(this IEnumerable<T> list,int pageSize, int total)        
        {
            return new PagedList<T>(list,pageSize,total);
        }		
	}
}