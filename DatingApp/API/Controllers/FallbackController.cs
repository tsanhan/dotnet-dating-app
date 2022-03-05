
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //1. first of all the derive from Controller (not ApiController)
    // * the difference is that Controller is for MVC, ApiController is for Web API
    // * this will give us view support (can return html)
    // * now, our angular app is the "view" for our app

    // * so two things:
    //  1. this controller will serve an html file (we'll it what file to serve)
    //  2. we'll tell out api to use this controller when there is no other controller that can handle the route
    //    * our angular app will know what to do with this route 
    public class FallbackController : Controller
    {
        public ActionResult Index(){
            // * this is the file that we want to serve
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/HTML");
            // go to Startup.cs to navigate to this controller on fallback
        }
    }
}