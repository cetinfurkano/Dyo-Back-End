using Dyo.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Entity.DTOs
{
    public class TeacherForUpdateDto
    {
        public string Id { get; set; }
        public string Email { get; set; } 
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string School { get; set; }
        public string PhoneNumber { get; set; }
        public Address Address { get; set; }
        public string Branch { get; set; }
       
    }
}
