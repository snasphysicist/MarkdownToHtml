
using Microsoft.AspNetCore.Mvc;

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
                markdown.Markdown
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
