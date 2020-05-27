
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public JsonResult Post(
            [FromBody] MarkdownModel markdown
        ) {
            return new JsonResult(
                markdown
            );
        }

        public class MarkdownModel
        {
            public string Markdown
            { get; set; }
        }
    }
}
