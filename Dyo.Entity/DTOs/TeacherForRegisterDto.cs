using Dyo.Core.Entities;
using Dyo.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Entity.DTOs
{
    public class TeacherForRegisterDto
    {
        public string Email { get; set; } = "defaultTeacher@email.com";
        public string Password { get; set; } = "Default.123";
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string School { get; set; }
        public string PhoneNumber { get; set; }
        public Address Address { get; set; } = new Address();
        public string Branch { get; set; } = "Default Branch";
        public string DistributorId { get; set; }
    }
}
