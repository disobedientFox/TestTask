using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask
{
    public interface INodesHelper
    {
        IEnumerable<Node> Nodes { get; }
        IEnumerable<Node> getChildrens(long idNode);
        Node getParent(long idNode);
    }
}
