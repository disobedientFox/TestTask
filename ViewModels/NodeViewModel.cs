using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTask.Data.Models;

namespace TestTask.ViewModels
{
    public class NodeViewModel : Node
    {
        public bool IsParent { get; set; }

        public IEnumerable<Node> Children { get; set; }
        public IEnumerable<NodeViewModel> ViewChildren { get; set; }

        public List<NodeViewModel> WholeChildrenList { get; set; }

        public int? Offset { get; set; }

    }
}
