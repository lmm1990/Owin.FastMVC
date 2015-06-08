?# Owin.FastMvc

Owin.FastMvc是一个轻量级的MVC框架，基于owin(asp.net next也是基于owin)

##      	目录


*	[快速上手](#快速上手)
*	[说明](#快速上手)

## 快速上手

### 配置web.config
<appSettings>
    <add key="applicationClass" value="WebTest.Startup, WebTest" />
</appSettings>

### 添加入口类文件
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

##	编写控制器代码

    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.Owin;
    using FastTemplate;

    namespace WebTest.Controllers
    {
        public class HomeController
        {
            public Task<bool> PreRequest(IOwinContext context,IOwinRequest request,IOwinResponse response)
            {
                return Task.FromResult<bool>(true);
            }
	
    	   #region /about
            public Task About(IOwinContext context, IOwinRequest request, IOwinResponse response)
            {
                return response.WriteAsync("about");
            }
            #endregion
        }
    }

## 说明

1、Owin.FastMVC配合FastTemplate使用效果更佳

2、控制器类名必须包含：Controller

3、PreRequest在action执行前执行，PreRequest返回false，action将不执行

4、访问Url为：命名空间Controllers后边部分+广告名，例：命名空间为：demo.Controllers.manage，action方法名为：index，则请求URL为：/manage/index

5、Url参数：可以用QueryString ? 形式传递参数，也可以用!分割，例：/List!1（!分割参数，参数值均为sting类型，详情请参考示例）


FastTemplate项目地址: [FastTemplate](https://github.com/lmm1990/FastTemplate)
