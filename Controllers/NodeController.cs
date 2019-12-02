using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TestTask.Data.Models;
using TestTask.ViewModels;

namespace TestTask.Controllers
{
    public class NodeController : Controller
    {
        private INodesHelper NodeRepository;
        private List<NodeViewModel> NodesChildList;
        private List<NodeViewModel> NodesForDelete = new List<NodeViewModel>();
        private List<NodeViewModel> ViewNodes = new List<NodeViewModel>();

        public NodeController(INodesHelper nodeRep)
        {
            NodeRepository = nodeRep;
        }

        public IActionResult Index()
        {
            return View();
        }

        // main page
        [HttpGet]
        public ViewResult Tree()
        {
            ViewBag.Title = "Tree of nodes";
            TreeViewModel treeViewModel = new TreeViewModel();
            foreach (var node in NodeRepository.Nodes)
            {
                ViewNodes.Add(new NodeViewModel
                {
                    Id = node.Id,
                    Title = node.Title,
                    parentId = node.parentId,
                    IsExpanded = node.IsExpanded,
                    Children = GetChilden(node.Id, NodeRepository.Nodes),
                    IsParent = GetChilden(node.Id, NodeRepository.Nodes).Count() != 0
                });
            }

            foreach (var viewNode in ViewNodes)
            {
                viewNode.ViewChildren = ViewNodes.Where(x => x.parentId == viewNode.Id);
            }

            ViewNodes = SetOffset(ViewNodes);

            ViewNodes
                .Where(x => x.parentId == null)
                .ToList()
                .ForEach(n =>
                {
                    NodesChildList = new List<NodeViewModel>();
                    Recursion(n);
                    n.WholeChildrenList = NodesChildList;
                });

            treeViewModel.AllNodes = ViewNodes.Where(x => x.parentId == null);
            return View(treeViewModel);
        }


        private IEnumerable<Node> GetChilden(long parentId, IEnumerable<Node> list)
        {
            return list.Where(n => n.parentId == parentId);

        }

        private List<NodeViewModel> SetOffset(List<NodeViewModel> list)
        {
            list.Where(n => n.parentId == null)
                .ToList()
                .ForEach(n => n.Offset = 0);

            while (list.Where(n => n.Offset == null).Any())
            {
                foreach (var node in list)
                {
                    if (node.Offset != null)
                    {
                        node.ViewChildren
                            .ToList()
                            .ForEach(n => n.Offset = node.Offset + 1);
                    }
                }
            }

            return list;
        }

        private NodeViewModel Recursion(NodeViewModel node)
        {
            if (node.IsParent && node.IsExpanded)
            {
                foreach (var child in node.ViewChildren)
                {
                    NodesChildList.Add(child);
                    Recursion(child);
                }
                return node;
            }
            return node;
        }

        private NodeViewModel RecursionFindChildrenForDelete(NodeViewModel node)
        {
            if (node.IsParent)
            {
                foreach (var child in node.ViewChildren)
                {
                    NodesForDelete.Add(child);
                    RecursionFindChildrenForDelete(child);
                }
                return node;
            }
            return node;
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = NodeRepository.Nodes.Where(n => n.Id == id).FirstOrDefault();

            if (model == null)
            {
                return RedirectToAction("tree");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, Node input)
        {
            var node = NodeRepository.Nodes.Where(n => n.Id == id).FirstOrDefault();

            if (node != null && ModelState.IsValid)
            {
                node.Title = input.Title;
                NodeRepository.EditNode(node);
                return RedirectToAction("tree");
            }
            return View(node);
        }

        // open the node
        [HttpPost]
        public IActionResult SwitchExpand(int id)
        {
            var node = NodeRepository.Nodes.Where(n => n.Id == id).FirstOrDefault();

            node.IsExpanded = !node.IsExpanded;
            NodeRepository.EditNode(node);
            return RedirectToAction("tree");
        }

        public IActionResult Add()
        {
            return View();
        }

        // cascade delete
        [HttpDelete]
        public IActionResult Delete(long id)
        {
            foreach (var node in NodeRepository.Nodes)
            {
                ViewNodes.Add(new NodeViewModel
                {
                    Id = node.Id,
                    Title = node.Title,
                    parentId = node.parentId,
                    IsExpanded = node.IsExpanded,
                    Children = GetChilden(node.Id, NodeRepository.Nodes),
                    IsParent = GetChilden(node.Id, NodeRepository.Nodes).Count() != 0
                });
            }

            foreach (var viewNode in ViewNodes)
            {
                viewNode.ViewChildren = ViewNodes.Where(x => x.parentId == viewNode.Id);
            }

            ViewNodes
                .Where(x => x.parentId == null)
                .ToList()
                .ForEach(n =>
                {
                    NodesChildList = new List<NodeViewModel>();
                    Recursion(n);
                    n.WholeChildrenList = NodesChildList;
                });

            var tmp = ViewNodes.Where(n => n.Id == id).FirstOrDefault();
            RecursionFindChildrenForDelete(tmp);

            foreach (var nodesForDelete in NodesForDelete)
            {
                foreach (var n in ViewNodes)
                    if (n.Id == nodesForDelete.Id)
                        NodeRepository.DeleteNode(n.Id);
            }
            NodeRepository.DeleteNode(id);

            return RedirectToAction("tree");
        }

        [HttpPost]
        public IActionResult Add(Node input)
        {
            var node = new Node();

            if (ModelState.IsValid)
            {
                node.Title = input.Title;
                NodeRepository.AddNode(node);
                return RedirectToAction("tree");
            }
            return View(node);
        }

        // change node position
        [HttpPost]
        public IActionResult ChangePosition(int element, int? into)
        {
            if(into == null)
            {
                NodeRepository.Nodes.FirstOrDefault(n => n.Id == element).parentId = into;
                NodeRepository.EditNode(NodeRepository.Nodes.FirstOrDefault(n => n.Id == element));
            }
            else if (IsChildToParent(element, into))
            {
                NodeRepository.Nodes.FirstOrDefault(n => n.Id == element).parentId = into;
                NodeRepository.EditNode(NodeRepository.Nodes.FirstOrDefault(n => n.Id == element));
            }
            return Ok();
        }

        // we need to make sure that the node is not a parent for the node we try to put into
        private bool IsChildToParent(int element, int? into)
        {
            foreach (var node in NodeRepository.Nodes)
            {
                ViewNodes.Add(new NodeViewModel
                {
                    Id = node.Id,
                    Title = node.Title,
                    parentId = node.parentId,
                    IsExpanded = node.IsExpanded,
                    Children = GetChilden(node.Id, NodeRepository.Nodes),
                    IsParent = GetChilden(node.Id, NodeRepository.Nodes).Count() != 0
                });
            }

            foreach (var viewNode in ViewNodes)
            {
                viewNode.ViewChildren = ViewNodes.Where(x => x.parentId == viewNode.Id);
            }

            ViewNodes = SetOffset(ViewNodes);

            var tmp = ViewNodes.FirstOrDefault(n => n.Id == element);

            NodesChildList = new List<NodeViewModel>();
            Recursion(tmp);
            tmp.WholeChildrenList = NodesChildList;


            if (tmp.WholeChildrenList.Count == 0)
                return true;
            foreach(var child in tmp.WholeChildrenList)
            {
                if (child.Id == into)
                    return false;
            }
            return true;
        }
    }
}