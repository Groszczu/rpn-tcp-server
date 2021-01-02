using System;

namespace RPN_Database.Model
{
    public class AdminApplication
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public bool? IsRejected { get; set; } = null;
        public DateTime Created { get; set; } = DateTime.Now;

        public override string ToString() =>
            $"Application{{Id: {Id}, UserId: {UserId}, IsRejected: {IsRejected}, Created: {Created}}}";
    }
}