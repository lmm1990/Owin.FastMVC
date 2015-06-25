using System;
using System.Configuration;

namespace Owin.FastMVC.ClassHandler
{
    public static class AppConfig
    {
        public static readonly string rootPath = AppDomain.CurrentDomain.BaseDirectory;

        public static readonly string applicationClass = ConfigurationManager.AppSettings["applicationClass"];
    }
}