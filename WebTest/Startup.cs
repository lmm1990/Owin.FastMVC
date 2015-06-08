using System;
using System.Collections.Generic;
using Owin;
using Beginor.Owin.StaticFile;
using FastTemplate;
using FastMVC.ClassHandler;

namespace WebTest
{
    public class Startup
    {
        public static void Init(IAppBuilder app)
        {
            TemplateEngine.IsDebug = true;
            TemplateEngine.Init(new HashSet<string> { }, "/Template");
            app.UseStaticFile(new StaticFileMiddlewareOptions
            {
                RootDirectory = AppConfig.rootPath,
                EnableETag = true,
                MimeTypeProvider = new MimeTypeProvider(new Dictionary<string, string> { 
                {".css","text/css"},
                    {".js","application/x-javascript"},
                    {".xml","text/xml"} ,
                    {".gif" , "image/gif"},
                    {".png" , "image/png"},
                    {".html","text/html"}
                })
            });
        }
    }
}