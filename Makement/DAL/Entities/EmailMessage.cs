using Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class EmailMessage
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public EmailMessageTypeEnum Type { get; set; }
    }
}
