using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models.User
{
    public class EditModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Phone { get; set; }
        public string Position { get; set; }
    }
}
