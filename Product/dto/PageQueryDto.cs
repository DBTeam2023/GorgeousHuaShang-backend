﻿using Product.exception;
using System.Text.Json.Serialization;

namespace Product.dto
{
    public class PageQueryDto
    {
        public int PageSize { get; set; } = 0;
        public int PageIndex { get; set; } = 0;
        public Dictionary<string, string>? Filter { get; set; }

        public void check()
        {
            if (PageSize <= 0)
                throw new PageException("page size should be positive");
            if (PageIndex <= 0)
                throw new PageException("page index should be larger than 0");
        }
       

        public string? getStrValue(string key)
        {
            if (Filter == null)
                return null;

            if (Filter.ContainsKey(key))
                return Filter[key];
            else
                return null;
        }
        public decimal? getDoubleValue(string key)
        {
            var val = getStrValue(key);
            if (val == null)
                return null;
            else
            {
                decimal ans;
                if (decimal.TryParse(val, out ans))
                    return ans;
                else
                    return null;
            }
                

        }



    }
}
