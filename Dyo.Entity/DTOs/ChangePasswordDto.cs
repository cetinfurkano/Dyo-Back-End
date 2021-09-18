using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Entity.DTOs
{
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
