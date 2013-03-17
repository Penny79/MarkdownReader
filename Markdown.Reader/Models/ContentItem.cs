using System.Collections.Generic;

namespace Markdown.Reader.Models
{
    public class ContentItem
    {
        public ContentItem()
        {
            this.ChildItems = new List<ContentItem>();
        }

        public int Id { get; set; }        
        
        public string Name { get; set; }

        public string Path { get; set; }

        public string Url { get; set; } 

        public int Level { get; set; }


        public IList<ContentItem> ChildItems { get; private set; }
        
        public bool IsLeaf
        {
            get
            {
                return this.ChildItems.Count == 0;
            }
        }
    }
}