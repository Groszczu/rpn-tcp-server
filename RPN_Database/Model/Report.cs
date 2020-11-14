using System;
using System.Collections.Generic;
using System.Text;

namespace RPN_Database.Model
{
    public class Report
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public User User { get; set; }

        public override string ToString() => $"Report{{Id: {Id}, Message: {Message}, CreatedAt: {CreatedAt}, UserId: {UserId}}}";
    }
}
