using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CCETest.Model
{
    public class SysUser
    {
        public string UserName { get; internal set; }
        public string PassWord { get; internal set; }
        public string AuthenticationType { get; internal set; }
        public string UserId { get; internal set; }
    }
}
