using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.DataProvider
{
    /// <summary>
    /// Home 的摘要说明
    /// </summary>
    public class Home : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            using (var edm = new BcuEntities())
            {
                context.Response.Write(JsonSerializer.Serialize(edm.newsSet));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}