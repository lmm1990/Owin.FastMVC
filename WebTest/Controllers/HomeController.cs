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

        public Task About(IOwinContext context, IOwinRequest request, IOwinResponse response)
        {
            return response.WriteAsync("about");
        }

        #region /manage/index
        public async Task Index(IOwinContext context,IOwinRequest request,IOwinResponse response)
        {
            #region POST
            if(request.Method.Equals("POST"))
            {
                IFormCollection formData = await request.ReadFormAsync();
                response.ContentType = "text/plan;charset=utf-8";
                await response.WriteAsync(string.Format("userName:{0},score:{1}", formData["userName"], formData["score"]));
                return;
            }
            #endregion
            await response.WriteAsync(TemplateEngine.Compile("/Template/Index.html", new Dictionary<string, dynamic> { 
                {"Title","首页"},
                {"cssList",new List<string>{"/static/css/login.css"}}
            }));
            return;
        }
        #endregion

        #region /List!{p}
        public Task List(IOwinContext context,IOwinRequest request,IOwinResponse response,string p)
        {
            return response.WriteAsync("currentPage:"+p);
        }
        #endregion
    }
}