using DotnetSpider.Core.Selector;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extension.Model.Attribute;
using System;

namespace DotNet.Spider
{
    //添加数据库及索引信息
    //[EntityTable("数据库名", "表名", EntityTable.Monday, Indexs = new[] { "Category" }, Uniques = new[] { "Category,Sku", "Sku" })]
    [EntityTable("OWNDB", "jd_sku")]
    //每个商品都在class为gl-i-wrap j-sku-item的DIV下面，因此添加EntitySelector到数据对象Product的类名上面
    [EntitySelector(Expression = "//li[@class='gl-item']/div[contains(@class,'j-sku-item')]")]
    [TargetUrlsSelector(XPaths = new[] { "//span[@class=\"p-num\"]" }, Patterns = new[] { @"&page=[0-9]+&" })]
    public class Product : SpiderEntity
    {
        //假设你需要采集SKU信息，观察HTML结构，计算出相对的XPath, 为什么是相对XPath？
        //因为EntitySelector已经把HTML截成片段了，内部的Html元素查询都是相对于
        //EntitySelector查询出来的元素。最后再加上数据库中列的信息
        [PropertyDefine(Expression = "./@data-sku", Length = 100)]
        public string Sku { get; set; }

        //爬虫内部，链接是通过Request对象来存储信息的，构造Request对象时可以添加额外的属性值，
        //这时候允许数据对象从Request的额外属性值中查询数据
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
