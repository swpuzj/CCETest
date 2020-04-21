using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CCE.Models
{
    public class SysUser
    {
        public string UserName { get;  set; }
        public string PassWord { get;  set; }
        public string AuthenticationType { get;  set; }
        public string UserId { get;  set; }
    }
}
