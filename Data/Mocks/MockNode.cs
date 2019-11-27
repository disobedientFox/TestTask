using System.Collections.Generic;

namespace TestTask
{
    public class MockNode : INodesHelper
    {
        public IEnumerable<Node> Nodes
        {
            get {
                return new List<Node>
                {
                    new Node { Title = "First"},
                    new Node { Title = "Second"},
                    new Node { Title = "Third"}
                };
            }
        }

        public IEnumerable<Node> getChildrens(long idNode)
        {
            return new List<Node>
            {
                new Node { Title = "First"}
            };
        }

        public Node getParent(long idNode)
        {
            throw new System.NotImplementedException();
        }
    }
}
