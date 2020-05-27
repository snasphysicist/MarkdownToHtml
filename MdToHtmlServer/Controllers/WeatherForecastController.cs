
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MdToHtmlServer.Controllers
{
    [ApiController]
    [Route("mdtohtml/convert")]
    public class ConvertController : ControllerBase
    {
        [HttpPost]
        public IEnumerable<WeatherForecast> Post()
        {

        }
    }
}
