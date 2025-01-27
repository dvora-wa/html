using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace html
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement(string id = null, string name = null)
        {
            Id = id;
            Name = name;
            Attributes = new List<string>();
            Classes = new List<string>();
            InnerHtml = string.Empty;
            Parent = null;
            Children = new List<HtmlElement>();
        }

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                yield return current;

                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement current = this;
            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }

        public List<HtmlElement> FindElements(Selector selector)
        {
            var result = new HashSet<HtmlElement>();
            FindElementsRecursive(this, selector, result);
            return result.ToList();
        }

        private void FindElementsRecursive(HtmlElement currentElement, Selector currentSelector, HashSet<HtmlElement> result)
        {
            var descendants = currentElement.Descendants();

            foreach (var descendant in descendants)
            {
                bool matches = true;

                if (!string.IsNullOrEmpty(currentSelector.TagName) && descendant.Name != currentSelector.TagName)
                {
                    matches = false;
                }

                if (!string.IsNullOrEmpty(currentSelector.Id) && descendant.Id != currentSelector.Id)
                {
                    matches = false;
                }

                if (currentSelector.Classes.Count > 0 && !currentSelector.Classes.All(c => descendant.Classes.Contains(c)))
                {
                    matches = false;
                }

                if (matches)
                {
                    result.Add(descendant);
                }
            }

            if (currentSelector.Child != null)
            {
                FindElementsRecursive(currentElement, currentSelector.Child, result);
            }
        }

        public HtmlElement AddChild(HtmlElement child)
        {
            child.Parent = this;
            Children.Add(child);
            return child;
        }

        public override string ToString()
        {
            return $"<Element Name='{Name}' Id='{Id}' Classes='{string.Join(", ", Classes)}' />";
        }

    }
}
