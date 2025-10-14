using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using simplified_picpay.Views.ViewModels;

namespace simplified_picpay.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Home()
        {
            var homeViewModel = new HomeViewModel
            {
                Status = "Ok",
                Time = DateTime.UtcNow
            };

            return Ok(new ResultViewModel<HomeViewModel>(homeViewModel));
        }
    }
}