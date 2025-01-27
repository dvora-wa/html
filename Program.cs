using html;
using System.Text.RegularExpressions;

static async Task<string> Load(string url)
{
    using (HttpClient client = new HttpClient())
    {
        try
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error fetching URL: {e.Message}");
            return string.Empty;
        }
    }
}

static IEnumerable<string> HtmlListString(string html)
{
    var htmlClean = new Regex("\\s").Replace(html, "");
    var htmlLines = new Regex("<(.*?)>").Split(htmlClean).Where(s => s.Length > 0);
    return htmlLines;
}

static HtmlElement BuildTreeHtmlElement(IEnumerable<string> htmlArr)
{
    HtmlElement root = new HtmlElement();
    HtmlElement currentElement = root;
    Stack<HtmlElement> stack = new Stack<HtmlElement>();

    foreach (var line in htmlArr)
    {
        var firstWord = line.Split(' ')[0];

        if (HtmlHelper.HTMLhelper.HtmlTags.Contains(firstWord) || HtmlHelper.HTMLhelper.HtmlVoidTags.Contains(firstWord))
        {
            var newElement = new HtmlElement
            {
                Name = firstWord,
                Parent = currentElement
            };

            currentElement.Children.Add(newElement);

            var remainingString = line.Substring(firstWord.Length).Trim();
            var attributes = new Regex("([^\\s]+)=\"(.*?)\"").Matches(remainingString);

            foreach (Match attribute in attributes)
            {
                if (attribute.Groups[1].Success)
                {
                    newElement.Attributes.Add(attribute.Groups[1].Value);
                    if (attribute.Groups[1].Value == "class")
                    {
                        newElement.Classes.AddRange(attribute.Groups[2].Value.Split(' '));
                    }
                    if (attribute.Groups[1].Value == "id")
                    {
                        newElement.Id = attribute.Groups[2].Value;
                    }
                }
            }

            bool isSelfClosing = remainingString.EndsWith("/") || HtmlHelper.HTMLhelper.HtmlVoidTags.Contains(newElement.Name);
            if (!isSelfClosing)
            {
                stack.Push(currentElement);
                currentElement = newElement;
            }
        }
        else if (firstWord.StartsWith("/"))
        {
            if (stack.Count > 0)
            {
                currentElement = stack.Pop();
            }
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                currentElement.InnerHtml += line;
            }
        }
    }
    return root;
}

static void PrintHtmlElement(HtmlElement element, int indent = 0)
{
    Console.WriteLine(new string(' ', indent) + $"<{element.Name} id=\"{element.Id}\">");

    if (element.Attributes.Count > 0)
    {
        Console.WriteLine(new string(' ', indent) + " Attributes: " + string.Join(", ", element.Attributes));
    }

    if (!string.IsNullOrEmpty(element.InnerHtml))
    {
        Console.WriteLine(new string(' ', indent + 2) + $"InnerHtml: {element.InnerHtml}");
    }

    foreach (var child in element.Children)
    {
        PrintHtmlElement(child, indent + 2);
    }

    Console.WriteLine(new string(' ', indent) + $"</{element.Name}>");
}


//string URL = "https://learn.malkabruk.co.il/practicode/projects/pract-2/";
//var html = await Load(URL);
//IEnumerable<string> h = HtmlListString(html);
//foreach (string hItem in h)
//{
//    Console.WriteLine("-"+hItem);
//}
//HtmlElement root = BuildTreeHtmlElement(h);
////PrintHtmlElement(root);

//var selectorTree = Selector.Parse("div#mydiv.class-name");
//Console.WriteLine(selectorTree);
//Console.ReadLine();









//static HtmlElement BuildTreeHtmlElement(string html)
//{
//    var htmlLines = HtmlListString(html);
//    //Console.WriteLine(HtmlHelper.HTMLhelper.HtmlVoidTags);
//    //Console.WriteLine(HtmlHelper.HTMLhelper.HtmlTags);
//    HtmlElement root = new HtmlElement();
//    HtmlElement currentElement = root;
//    Stack<HtmlElement> stack = new Stack<HtmlElement>();

//    foreach (var line in htmlLines)
//    {
//        var firstWord = line.Split(' ')[0];

//        if (HtmlHelper.HTMLhelper.HtmlTags.Contains(firstWord))
//        {
//            break; // סיום ה-html
//        }
//        else if (firstWord.StartsWith("/"))
//        {
//            // תגית סוגרת
//            currentElement = stack.Pop(); // חזור לרמה הקודמת
//        }
//        else
//        {
//            // תגית פתוחה
//            var newElement = new HtmlElement
//            {
//                Name = firstWord,
//                Parent = currentElement
//            };

//            // הוספת לתוך הילדים של האלמנט הנוכחי
//            currentElement.Children.Add(newElement);

//            // פרוק המחרוזת להמשך המחרוזת והמאפיינים
//            var remainingString = line.Substring(firstWord.Length).Trim();
//            var attributes = new Regex("([^\\s]+)=\"([^\"]*)\"").Matches(remainingString);

//            foreach (Match attribute in attributes)
//            {
//                newElement.Attributes.Add(attribute.Groups[1].Value);
//                if (attribute.Groups[1].Value == "class")
//                {
//                    newElement.Attributes.AddRange(attribute.Groups[2].Value.Split(' '));
//                }
//                if (attribute.Groups[1].Value == "id")
//                {
//                    newElement.Id = attribute.Groups[2].Value;
//                }
//            }

//            // בדוק אם התגית סוגרת את עצמה
//            bool isSelfClosing = remainingString.EndsWith("/") || HtmlHelper.HTMLhelper.HtmlVoidTags.Contains(newElement.Name);
//            if (!isSelfClosing)
//            {
//                stack.Push(currentElement); // שמור את האלמנט הנוכחי
//                currentElement = newElement; // עדכן את האלמנט הנוכחי
//            }
//        }
//    }
//    return root;
//}
