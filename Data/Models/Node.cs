using System.Collections.Generic;

namespace TestTask
{
    public class Node
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public Node parentNode { get; set; }
        public List<Node> childrenNodes { get; set; }
    }
}
