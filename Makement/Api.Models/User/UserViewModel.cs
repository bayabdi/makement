using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models.User
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string Position { get; set; }
    }
}
