using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    //1. add this, this is how we use the filter
    [ServiceFilter(typeof(LogUserActivity))] 
    //2. test this, login with a member, and see the LastActive change using section 12: 'Get User by username'.
    //3. go back to LogUserActivity to point 11
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {

    }
}