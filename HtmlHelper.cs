using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace html
{
    internal class HtmlHelper
    {
        private static HtmlHelper _htmlHelper = new HtmlHelper("JSON Files/HtmlTags.json", "JSON Files/HtmlVoidTags.json");
        public static HtmlHelper HTMLhelper => _htmlHelper;
        public string[] HtmlTags { get; private set; }
        public string[] HtmlVoidTags { get; private set; }

        private HtmlHelper(string HtmlTagsPath, string HtmlVoidTagsPath)
        {
            LoadTags(HtmlTagsPath, HtmlVoidTagsPath);
        }

        private void LoadTags(string allTagsFilePath, string selfClosingTagsFilePath)
        {
            HtmlTags = JsonSerializer.Deserialize<string[]>(File.ReadAllText(allTagsFilePath));
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(File.ReadAllText(selfClosingTagsFilePath));
        }
    }
}

