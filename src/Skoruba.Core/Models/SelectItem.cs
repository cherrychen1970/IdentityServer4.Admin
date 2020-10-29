using System;
using System.Linq;
using System.Collections.Generic;

namespace Skoruba.Models
{
	static public class SelectList {
		public static List<SelectItem> From<T>()
		{
			Type enumType = typeof(T);
            var values = Enum.GetValues(enumType).OfType<int>().ToList();
            return values.Select(x => new SelectItem(x.ToString(),   Enum.GetName(enumType, x))).ToList();			
		}
	}
	public class SelectItem
	{
		public SelectItem(string id, string text)
		{
			Id = id;
			Text = text;
		}

		public string Id { get; set; }

		public string Text { get; set; }
	}
}