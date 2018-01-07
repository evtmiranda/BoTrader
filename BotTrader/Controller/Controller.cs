using System;
using System.Configuration;
using System.Threading;
using BotTrader.Service;

namespace BotTrader.Controller
{
    internal class Controller
    {
        Service.Service service;

        internal Controller()
        {
            service = new Service.Service();
        }

        internal void ProcessamentoDados()
        {
            Comunicacao.EscreverNaTela("iniciando aplicação no modo ProcessamentoDados");

            while (true)
            {
                Comunicacao.EscreverNaTela("iniciando o processamento das informações");
                service.ProcessarInformacoesBitCoinTrade();

                Comunicacao.EscreverNaTela(string.Format("aplicação em pausa por {0} segundos", Convert.ToInt16(ConfigurationManager.AppSettings.Get("SegundosPausaPrograma"))));

                Thread.Sleep(Convert.ToInt32(
                    TimeSpan.FromSeconds(
                        Convert.ToInt16(ConfigurationManager.AppSettings.Get("SegundosPausaPrograma")
                        )).TotalMilliseconds));
            }
        }

        public void GeracaoInsightEAlerta()
        {
            Comunicacao.EscreverNaTela("iniciando aplicação no modo GeracaoInsightEAlerta");

            while (true)
            {
                Comunicacao.EscreverNaTela("iniciando a busca por insights e alertas");
                service.GerarInsightEAlerta();

                Comunicacao.EscreverNaTela(string.Format("aplicação em pausa por {0} segundos", Convert.ToInt16(ConfigurationManager.AppSettings.Get("SegundosPausaPrograma"))));

                Thread.Sleep(Convert.ToInt32(
                    TimeSpan.FromSeconds(
                        Convert.ToInt16(ConfigurationManager.AppSettings.Get("SegundosPausaPrograma")
                        )).TotalMilliseconds));
            }
        }

        public void ProcessamentoFull()
        {
            Comunicacao.EscreverNaTela("iniciando aplicação no modo ProcessamentoFull");

            while (true)
            {
                Comunicacao.EscreverNaTela("iniciando o processamento das informações");
                service.ProcessarInformacoesBitCoinTrade();

                Comunicacao.EscreverNaTela("iniciando a busca por insights e alertas");
                service.GerarInsightEAlerta();

                Comunicacao.EscreverNaTela(string.Format("aplicação em pausa por {0} segundos", Convert.ToInt16(ConfigurationManager.AppSettings.Get("SegundosPausaPrograma"))));

                Thread.Sleep(Convert.ToInt32(
                    TimeSpan.FromSeconds(
                        Convert.ToInt16(ConfigurationManager.AppSettings.Get("SegundosPausaPrograma")
                        )).TotalMilliseconds));
            }
        }

    }
}
