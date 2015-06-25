using System;
using System.Collections.Generic;

namespace Owin.FastMVC.ClassHandler
{
    public static class DataHandler
    {
        /// <summary>
        ///     <para>路由列表</para>
        /// </summary>
        internal static Dictionary<string, Route> routeList = new Dictionary<string, Route>();

        /// <summary>
        ///     <para>获得路由列表</para>
        /// </summary>
        public static Dictionary<string, Route> RouteList
        {
            get { return routeList; }
        }
    }
}