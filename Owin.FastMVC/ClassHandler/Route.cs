using System;
using System.Reflection;

namespace Owin.FastMVC.ClassHandler
{
    public class Route
    {
        /// <summary>
        ///     <para>请求前执行的方法</para>
        /// </summary>
        public MethodInfo PreRequestMethod { get; set; }

        /// <summary>
        ///     <para>请求执行的方法</para>
        /// </summary>
        public MethodInfo Method { get; set; }

        /// <summary>
        ///     <para>Controller实例</para>
        /// </summary>
        public Object Instance { get; set; }
    }
}