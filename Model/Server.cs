using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Server
    {
        public int Id { get; set; }
        public string ServerName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string IISVersion { get; set; }
        public string SqlVersion { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string OSName { get; set; }
        public string ServerType { get; set; }
        public DateTime AddTime { get; set; }
    }
}
