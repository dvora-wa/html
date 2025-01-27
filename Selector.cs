using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace html
{   
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public static Selector Parse(string query)
        {
            var levels = query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Selector root = new Selector();
            Selector currentSelector = root;

            foreach (var level in levels)
            {
                var parts = level.Split(new[] { '#', '.' }, StringSplitOptions.RemoveEmptyEntries);
                var tagName = parts.FirstOrDefault();
                var id = parts.FirstOrDefault(p => level.StartsWith("#" + p));
                var classes = parts.Where(p => level.StartsWith("." + p)).ToList();

                if (!string.IsNullOrEmpty(tagName) && HtmlHelper.HTMLhelper.HtmlTags.Contains(tagName))
                {
                    currentSelector.TagName = tagName;
                }
                if (!string.IsNullOrEmpty(id))
                {
                    currentSelector.Id = id;
                }
                currentSelector.Classes.AddRange(classes);

                var newSelector = new Selector
                {
                    Parent = currentSelector,
                    TagName = currentSelector.TagName,
                    Id = currentSelector.Id,
                    Classes = new List<string>(currentSelector.Classes)
                };

                currentSelector.Child = newSelector;
                currentSelector = newSelector;
            }

            return root;
        }
    }
}
