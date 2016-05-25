using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eomat.Helpers
{
    class NodeBase
    {
        public long NodeID { get; set; }
        public long? ParentNodeID { get; set; }
        public string Name { get; set; }
        
    }
}
