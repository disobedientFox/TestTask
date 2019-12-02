using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TestTask.Data.Models;

namespace TestTask
{
    public class NodeRepository : INodesHelper
    {
        private static AppDBContext _appDBContext;

        public NodeRepository(AppDBContext context)
        {
            _appDBContext = context;
        }

        public IEnumerable<Node> Nodes => _appDBContext.Nodes;

        public Node GetNode(long idNode)
        {
            return _appDBContext.Nodes.FirstOrDefault(n => n.Id == idNode);
        }

        public void AddNode(Node node)
        {
            node.parentId = null;
            _appDBContext.Nodes.Add(node);
            _appDBContext.SaveChanges();
        }

        public void EditNode(Node node)
        {
            _appDBContext.Update(node);
            _appDBContext.SaveChanges();
        }

        public void DeleteNode(long id)
        {
            var node =_appDBContext.Nodes.Where(n => n.Id == id).FirstOrDefault();
            _appDBContext.Remove(node);
            _appDBContext.SaveChanges();
        }

    }
}
