using System;
using System.Configuration;
using System.Threading;
using BotTrader.Service;

namespace BotTrader.Controller
{
    class Controller
    {
        public void Processar()
        {
            Service.Service service = new Service.Service();

            while (true)
            {
                Comunicacao.EscreverNaTela("iniciando aplicação");

                service.ProcessarInformacoesBitCoinTrade();

                Comunicacao.EscreverNaTela(string.Format("aplicação em pausa por {0} segundos", Convert.ToInt16(ConfigurationManager.AppSettings.Get("SegundosPausaPrograma"))));

                Thread.Sleep(Convert.ToInt32(
                    TimeSpan.FromSeconds(
                        Convert.ToInt16(ConfigurationManager.AppSettings.Get("SegundosPausaPrograma")
                        )).TotalMilliseconds));
            }
        }
    }
}
