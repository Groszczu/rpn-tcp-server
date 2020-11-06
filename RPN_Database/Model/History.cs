using System;

namespace RPN_Database.Model
{
    public class History
    {
        public int? Id { get; set; }
        public string Expression { get; set; }
        public string Result { get; set; }
        public DateTime Executed { get; set; } = DateTime.Now;

        public override string ToString() => $"History{{ Id: {Id}, Expression: {Expression}, Result: {Result}, Executed: {Executed} }}";

    }
}
