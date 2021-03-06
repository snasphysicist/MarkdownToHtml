﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using MarkdownToHtml;

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
            MarkdownParser parser = new MarkdownParser(
                markdown.Lines()
            );
            HtmlModel html = new HtmlModel(
                parser.ToHtml()
            );
            return new JsonResult(
                html
            );
        }

        public class MarkdownModel
        {
            public string Markdown
            { get; set; }

            public string[] Lines()
            {
                return Markdown.Split("\n");
            }
        }

        public class HtmlModel
        {
            public string Html
            { get; set; }

            public HtmlModel(
                string html
            ) {
                Html = html;
            }
        }
    }
}
