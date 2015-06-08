?# Owin.FastMvc

Owin.FastMvc��һ����������MVC��ܣ�����owin(asp.net nextҲ�ǻ���owin)

##      	Ŀ¼


*	[��������](#��������)
*	[˵��](#��������)

## ��������

### ����web.config
<appSettings>
    <add key="applicationClass" value="WebTest.Startup, WebTest" />
</appSettings>

### ���������ļ�
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

##	��д����������

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

## ˵��

1��Owin.FastMVC���FastTemplateʹ��Ч������

2���������������������Controller

3��PreRequest��actionִ��ǰִ�У�PreRequest����false��action����ִ��

4������UrlΪ�������ռ�Controllers��߲���+����������������ռ�Ϊ��demo.Controllers.manage��action������Ϊ��index��������URLΪ��/manage/index

5��Url������������QueryString ? ��ʽ���ݲ�����Ҳ������!�ָ����/List!1��!�ָ����������ֵ��Ϊsting���ͣ�������ο�ʾ����


FastTemplate��Ŀ��ַ: [FastTemplate](https://github.com/lmm1990/FastTemplate)
