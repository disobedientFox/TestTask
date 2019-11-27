using Microsoft.AspNetCore.Mvc;
using TestTask.ViewModels;

namespace TestTask.Controllers
{
    public class NodeController : Controller
    {
        private INodesHelper AllNodes;

        public NodeController(INodesHelper _allNodes)
        {
            AllNodes = _allNodes;
        }

        public ViewResult List()
        {
            ViewBag.Title = "List of nodes";
            NodeListViewModel nodeListViewModel = new NodeListViewModel();
            nodeListViewModel.AllNodes = AllNodes.Nodes;
            return View(nodeListViewModel);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}