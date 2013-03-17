using System.Collections.Generic;
using System.IO;
using System.Linq;
using Markdown.Reader.Models;

namespace Markdown.Reader.Logic
{
    /// <summary>
    /// Encapsulates the directory work with the repository folder
    /// </summary>
    public class ContentRepository
    {
        #region private fields

        private readonly string repositoryPath;
        private readonly string baseUrl;
        private List<string> ignoreFolders;
        private const string IgnoreFileName = "mdr.ignore";

        #endregion

        #region contructor

        public ContentRepository(string repositoryPath, string baseUrl)
        {
            this.repositoryPath = repositoryPath;
            this.baseUrl = baseUrl;

            this.LoadIgnoreFolders();
        }

        #endregion

        #region public methods


        public ContentItem CreateContentTree()
        { 
            var di = new DirectoryInfo(this.repositoryPath);

            var root = new ContentItem
                {
                    Id = this.repositoryPath.GetHashCode(), 
                    Name = di.Name, 
                    Path = this.repositoryPath , 
                    Level = 1,
                    Url = baseUrl
                };

            return CreateSubTree(root);
        }

        public ContentItem Find(int itemId)
        {
            var contentTree = this.CreateContentTree();

            return FindRecursive(contentTree, itemId);
        }

        #endregion

        #region private methods

        private void LoadIgnoreFolders()
        {
            ignoreFolders = new List<string>();
            string ignoreFile = Path.Combine(this.repositoryPath, IgnoreFileName);
            if (File.Exists(ignoreFile))
            {
                ignoreFolders.AddRange(File.ReadLines(ignoreFile));
            }
        }

        private ContentItem FindRecursive(ContentItem current, int itemId)
        {
            ContentItem matchedItem = null;

            foreach (var childItem in current.ChildItems)
            {
                if (childItem.Id == itemId)
                {
                    matchedItem = childItem;
                }
                else
                {
                    matchedItem = FindRecursive(childItem, itemId);
                }

                if (matchedItem != null)
                {
                    break;
                }
            }
            return matchedItem;
        }

        private ContentItem CreateSubTree(ContentItem current)
        {
            var di = new DirectoryInfo(current.Path);

            foreach (var subFolder in di.EnumerateDirectories())
            {
                if (IsDirectoryToProcess(subFolder))
                {
                    var child = new ContentItem
                    {
                        Id = subFolder.FullName.GetHashCode(),
                        Name = subFolder.Name,
                        Path = subFolder.FullName,
                        Level = current.Level + 1,
                        Url = UriCombine(current.Url, subFolder.Name)
                    };
                    current.ChildItems.Add(child);
                    CreateSubTree(child);
                }
            }
            return current;
        }

        private bool IsDirectoryToProcess(DirectoryInfo directoryInfo)
        {
            return ignoreFolders.All(x => x != directoryInfo.Name);
        }

        public static string UriCombine(string uri1, string uri2)
        {
            uri1 = uri1.TrimEnd('/');
            uri2 = uri2.TrimStart('/');
            return string.Format("{0}/{1}", uri1, uri2);
        }

        #endregion
       
    }
}