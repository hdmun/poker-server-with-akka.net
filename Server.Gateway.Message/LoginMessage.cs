namespace Server.Gateway.Message
{
    public class LoginMessage
    {
        public string Id { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseMessage
    {
        public bool Authenticated { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
