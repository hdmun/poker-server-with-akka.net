namespace Domain.Network
{
    public class Packet
    {
        public object Message { get; set; }

        public static Packet Create(object message)
        {
            return new Packet
            {
                Message = message
            };
        }
    }
}
