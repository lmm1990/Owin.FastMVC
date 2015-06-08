using System;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using Microsoft.Owin;
using Owin;
using FastMVC.ClassHandler;

[assembly: OwinStartup(typeof(Startup))]
public class Startup
{
    private static readonly object lockObj = new object();
    private static bool isFirstIn = false;

    public void Configuration(IAppBuilder app)
    {
        if (!isFirstIn)
        {
            lock (lockObj)
            {
                if (!isFirstIn)
                {
                    Type appType = Type.GetType(AppConfig.applicationClass, false);

                    if (appType == null)
                    {
                        throw new Exception(string.Format("Application class \"{0}\" could not be loaded", AppConfig.applicationClass));
                    }
                    MethodInfo initMethod = appType.GetMethod("Init", new Type[] { typeof(IAppBuilder) });

                    if (initMethod == null || !initMethod.IsStatic)
                    {
                        throw new Exception(string.Format("No \"public static void Init()\" method defined for class {0}", appType.Name));
                    }
                    initMethod.Invoke(null, new object[] { app });

                    MethodInfo preRequestMethod;
                    string path;
                    foreach (Type type in Assembly.GetAssembly(appType).GetTypes())
                    {
                        if (!type.IsClass || !type.IsPublic || type.Name.IndexOf("Controller") == -1)
                        {
                            continue;
                        }
                        preRequestMethod = type.GetMethod("PreRequest");
                        if (preRequestMethod != null && preRequestMethod.ReturnType != typeof(Task<bool>))
                        {
                            preRequestMethod = null;
                        }
                        foreach (MethodInfo item in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                        {
                            if (item.Name.Equals("GetType") || 
                                item.Name.Equals("GetHashCode") || 
                                item.Name.Equals("ToString") || 
                                item.Name.Equals("Equals") ||
                                item.Name.Equals("PreRequest"))
                            {
                                continue;
                            }
                            path = type.Namespace.Substring(type.Namespace.IndexOf("Controllers") + 11).Replace("Controller", "").Replace('.', '/');
                            DataHandler.routeList[string.Format("{0}/{1}", path, item.Name).ToLower()] = new Route
                            {
                                Method = item,
                                PreRequestMethod = preRequestMethod,
                                Instance = Activator.CreateInstance(type)
                            };
                        }
                    }
                    preRequestMethod = null;
                    path = null;
                    isFirstIn = true;
                }
            }
        }

        //beginRequest
        app.Run(HandleRequest);
    }

    static Task HandleRequest(IOwinContext context)
    {
        IOwinRequest request = context.Request;
        string url = request.Uri.AbsolutePath.ToLower();
        IOwinResponse response = context.Response;
        string[] urlAndParamList = HttpUtility.UrlDecode(url).Split('!');
        url = urlAndParamList[0];
        if (url.Equals("/"))
        {
            url = "/index";
        }
        if (!DataHandler.routeList.ContainsKey(url))
        {
            url = null;
            urlAndParamList = null;
            return response.WriteAsync("page not found!");
        }
        //preRequest
        if (DataHandler.routeList[url].PreRequestMethod != null)
        {
            bool state = (DataHandler.routeList[url].PreRequestMethod.Invoke(DataHandler.routeList[url].Instance,
            new object[] { context, request, response }) as Task<bool>).Result;
            if(!state)
            {
                return Task.Factory.StartNew(()=>{});
            }
        }
        int paramLength = urlAndParamList.Length;
        object[] methodParamList = new object[paramLength + 2];
        methodParamList[0] = context;
        methodParamList[1] = request;
        methodParamList[2] = response;
        for (int i = 1; i < paramLength; i++)
        {
            methodParamList[2 + i] = urlAndParamList[i];
        }
        urlAndParamList = null;
        return DataHandler.routeList[url].Method.Invoke(DataHandler.routeList[url].Instance, methodParamList) as Task;
    }
}