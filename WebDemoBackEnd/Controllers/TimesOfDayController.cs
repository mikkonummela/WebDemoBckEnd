using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebDemoBackEnd.Models;

namespace WebDemoBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesOfDayController : ControllerBase
    {
        //Returns the list of all food categories
        [HttpGet]
        [Route("")]
        public List<TimesOfDay> GetTimesOfDay()
        {
            return (new RpkContext()).TimesOfDay.ToList();
        }
    }
}