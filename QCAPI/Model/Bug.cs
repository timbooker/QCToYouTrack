using System.Collections.Generic;

namespace QCAPI.Model
{
    public class Bug
    {
        public string Status { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}