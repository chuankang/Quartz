using DotnetSpider.Core.Downloader;
using DotnetSpider.Extension;
using DotnetSpider.Extension.Pipeline;
using System.Collections.Generic;

namespace DotNet.Spider
{
    //配置爬虫（继承EntitySpider）
    public class JdSkuSampleSpider : EntitySpider
    {
        public JdSkuSampleSpider() : base("JdSkuSample")
        {

        }

        protected override void MyInit(params string[] arguments)
        {
            Identity = Identity ?? "JD SKU SAMPLE";
            //当爬虫连续30秒无法从调度中心取得需要采集的链接时结束.
            EmptySleepTime = 3000;
            ThreadNum = 1;
            // dowload html by http client
            Downloader = new HttpClientDownloader();

            // storage data to mysql, default is mysql entity pipeline, so you can comment this line. Don't miss sslmode.
            //AddPipeline(new MySqlEntityPipeline("Database='mysql';Data Source=localhost;User ID=root;Password=;Port=3306;SslMode=None;"));
            AddPipeline(new SqlServerEntityPipeline("Server=.;Database=OWNDB;uid=sa;pwd=123456;MultipleActiveResultSets=true"));

            //AddStartUrl第二个参数Dictionary<string, object>就是用于Enviroment查询的数据
            AddStartUrl("http://list.jd.com/list.html?cat=9987,653,655&page=2&JL=6_0_0&ms=5#J_main",
                new Dictionary<string, object>
                {
                    { "name", "手机" },
                    { "cat3", "655" }
                });
            AddEntityType<Product>();
        }
    }
}
