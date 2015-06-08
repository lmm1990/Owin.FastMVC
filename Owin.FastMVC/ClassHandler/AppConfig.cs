using System;
using System.Configuration;

namespace FastMVC.ClassHandler
{
    public static class AppConfig
    {
        public static readonly string rootPath = AppDomain.CurrentDomain.BaseDirectory;

        public static readonly string applicationClass = ConfigurationManager.AppSettings["applicationClass"];
    }
}