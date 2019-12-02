using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTask.ViewModels;

namespace TestTask.ViewModels
{
    public class TreeViewModel
    {
        public IEnumerable<NodeViewModel> AllNodes { get; set; }
    }
}
