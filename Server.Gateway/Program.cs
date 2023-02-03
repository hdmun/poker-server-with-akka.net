using Topshelf;

namespace Server.Gateway
{
    class Program
    {
        static int Main(string[] args)
        {
            return (int)HostFactory.Run(x =>
            {
                x.SetServiceName("Poker Gateway Server");
                x.SetDisplayName("Poker Gateway Server Service");
                x.SetDescription("Poker Gateway Server Service by Akka.net");

                x.RunAsLocalSystem();
                x.StartAutomatically();
                x.Service(() => new GatewayService());
                x.EnableServiceRecovery(r => r.RestartService(1));
            });
        }
    }
}
