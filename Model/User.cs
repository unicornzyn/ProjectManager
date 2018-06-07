using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RealName { get; set; }
        public int RoleType { get; set; }
        public int Status { get; set; }
        public DateTime LeaveTime { get; set; }
        public int BugzillaId { get; set; }
    }
}
