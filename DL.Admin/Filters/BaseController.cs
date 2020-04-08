using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;

namespace DL.Admin.Filters
{ 
    [Authorize]
    [Produces("application/json")]
    public abstract class BaseController : Controller
    {
       
    }
}
