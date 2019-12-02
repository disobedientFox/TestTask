using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTask.Data.Models;

namespace TestTask
{
    public interface INodesHelper
    {
        IEnumerable<Node> Nodes { get; }
        Node GetNode(long idNode);
        void EditNode(Node node);
        void AddNode(Node node);
        void DeleteNode(long id);
    }
}
