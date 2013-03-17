using System.IO;
using System.Text.RegularExpressions;
using Markdown.Reader.Models;

namespace Markdown.Reader.Logic
{
    public class MarkdownConverter
    {
        private readonly ContentItem item;
        private readonly MarkdownSharp.Markdown engine;
        private static Regex fileReferenceRegex = new Regex("\"files/", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public MarkdownConverter(ContentItem item)
        {
            this.item = item;

            this.engine = new MarkdownSharp.Markdown();

        }

        public string Render()
        {
            string fileName = Path.Combine(item.Path, "readme.md");

            if (!File.Exists(fileName))
                return string.Empty;

            string markdownContent = File.ReadAllText(fileName);

            var htmlContent = engine.Transform(markdownContent);


            htmlContent = fileReferenceRegex.Replace(htmlContent, "\"" + item.Url + "/files/");

            return htmlContent;
        }
    }
}