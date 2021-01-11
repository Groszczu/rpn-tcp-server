using System;

namespace Client.Model
{
    public class Application
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime Created { get; set; }

        public static Application FromString(string input)
        {
            var elems = input.Split(',');

            var application = new Application
            {
                Id = int.Parse(elems[0]),
                Username = elems[1],
                Created = DateTime.Parse(elems[2])
            };

            return application;
        }

        public string AsAcceptMessage() => $"application accept {Id}";
        public string AsDeclineMessage() => $"application decline {Id}";

        public override string ToString() => $"Application id: {Id}, username: {Username}, issued on {Created}";
    }
}