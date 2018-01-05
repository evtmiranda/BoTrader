using System;
using System.Threading;

namespace BotTrader.Controller
{
    class Controller
    {
        public void Processar()
        {
            while (true)
            {
                Service.Service service = new Service.Service();

                service.Pensar();

                Thread.Sleep(Convert.ToInt32(TimeSpan.FromSeconds(10).TotalMilliseconds));
            }
        }
    }
}
