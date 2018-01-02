using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Attentions
    {
        public int Id { get; set; }
        public string Remark { get; set; }        
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime AddTime { get; set; }
    }
}
