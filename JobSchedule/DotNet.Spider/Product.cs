using DotnetSpider.Core.Selector;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extension.Model.Attribute;
using System;

namespace DotNet.Spider
{
    //数据库名和表名
    [EntityTable("OWNDB", "jd_sku")]
    [EntitySelector(Expression = "//li[@class='gl-item']/div[contains(@class,'j-sku-item')]")]
    [TargetUrlsSelector(XPaths = new[] { "//span[@class=\"p-num\"]" }, Patterns = new[] { @"&page=[0-9]+&" })]
    public class Product : SpiderEntity
    {
        [PropertyDefine(Expression = "./@data-sku", Length = 100)]
        public string Sku { get; set; }

        [PropertyDefine(Expression = "name", Type = SelectorType.Enviroment, Length = 100)]
        public string Category { get; set; }

        [PropertyDefine(Expression = "cat3", Type = SelectorType.Enviroment)]
        public int CategoryId { get; set; }

        [PropertyDefine(Expression = "./div[1]/a/@href")]
        public string Url { get; set; }

        [PropertyDefine(Expression = "./div[5]/strong/a")]
        public long CommentsCount { get; set; }

        [PropertyDefine(Expression = ".//div[@class='p-shop']/@data-shop_name", Length = 100)]
        public string ShopName { get; set; }

        [PropertyDefine(Expression = ".//div[@class='p-name']/a/em", Length = 100)]
        public string Name { get; set; }

        [PropertyDefine(Expression = "./@venderid", Length = 100)]
        public string VenderId { get; set; }

        [PropertyDefine(Expression = "./@jdzy_shop_id", Length = 100)]
        public string JdzyShopId { get; set; }

        [PropertyDefine(Expression = "Monday", Type = SelectorType.Enviroment)]
        public DateTime RunId { get; set; }
    }
}
