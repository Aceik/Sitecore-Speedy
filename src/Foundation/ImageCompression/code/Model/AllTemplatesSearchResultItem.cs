using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Converters;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;

namespace Sitecore.Foundation.ImageCompression.Model
{
    public class AllTemplatesSearchResultItem : SearchResultItem
    {
        [IndexField("_templates")]
        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        public IEnumerable<ID> ItemBaseTemplates { get; set; }
    }
}



