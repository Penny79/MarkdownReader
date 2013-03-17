using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Markdown.Reader.Logic;
using Markdown.Reader.Models;

namespace Markdown.Reader.Controllers
{
    public class HomeController : Controller
    {
        private readonly ContentRepository repository;

        public HomeController()
        {
            string path = ConfigurationManager.AppSettings["MarkdownRepository"];
            string baseUrl = ConfigurationManager.AppSettings["MarkdownRepositoryBaseUrl"];
            repository = new ContentRepository(path, baseUrl);

           string test = "<img src=\"files/Screenshot-Enabling-Exception-Reporting-in-Magento-Error-Folder-Local-Xml.png\" alt=\"Screenshot of the local.xml and local.xml.sample file in Magentos errors folder\">";
            Regex rgx = new Regex("\"files/");
            string result = rgx.Replace(test, "\"http://www.google.de/files/");
        }

        public ActionResult Index()
        {
            var contentTree = repository.CreateContentTree();
            var viewModel = new ReaderViewModel() {ContentTree = contentTree, Document = string.Empty};
            return View(viewModel);


        }

        public ViewResult ShowDocument(int id)
        {
            var contentTree = repository.CreateContentTree();
            var selectedItem = repository.Find(id);

            var viewModel = new ReaderViewModel { ContentTree = contentTree, Document = string.Empty };

            if (selectedItem != null)
            {
                var renderer = new MarkdownConverter(selectedItem);
                viewModel.Document = renderer.Render();
            }
            return View("Index", viewModel);

        }

    }
}
