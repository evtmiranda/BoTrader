using System;
using BotTrader.DAO;
using BotTrader.Model.Orders;
using BotTrader.Model.Ticker;
using BotTrader.Model.Trades;

namespace BotTrader.Service
{
    internal class Service
    {
        Orders listaOrder;
        Trades listaTrade;
        Ticker ticker;
        DadosConsultaTrade dadosConsultaTrade;

        OrdersDAO ordersDAO;
        TradesDAO tradesDAO;
        TickerDAO tickerDAO;

        Matematica matematica;

        internal Service()
        {
            ordersDAO = new OrdersDAO();
            tradesDAO = new TradesDAO();
            tickerDAO = new TickerDAO();

            matematica = new Matematica();
        }

        /// <summary>
        /// Consulta as informações de trade e insere no banco de dados
        /// </summary>
        internal void ProcessarInformacoesBitCoinTrade()
        {
            Comunicacao.EscreverNaTela("iniciando a consulta de informações de trade");
            ConsultarInformacoesBitCoinTrade();

            Comunicacao.EscreverNaTela("inserindo as informações de trade no banco de dados");
            InserirNoBancoInformacoesBitCoinTrade();
        }

        internal void GerarInsightEAlerta()
        {


            matematica.AnalisarCompra();
            matematica.AnalisarVenda();
        }

        private void ConsultarInformacoesBitCoinTrade()
        {
            RequisicaoRest reqRest = new RequisicaoRest();

            Comunicacao.EscreverNaTela("consultando as ordens");
            listaOrder = reqRest.GetOrders();

            dadosConsultaTrade = new DadosConsultaTrade
            {
                DataInicial = tradesDAO.ConsultarUltimaDataProcessamento(),
                DataFinal = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss-03:00"),
                NumeroPagina = 1,
                TamanhoPagina = int.MaxValue
            };

            Comunicacao.EscreverNaTela("consultando os trades");
            listaTrade = reqRest.GetTrades(dadosConsultaTrade);

            Comunicacao.EscreverNaTela("consultando o ticker");
            ticker = reqRest.GetTicker();
        }

        private void InserirNoBancoInformacoesBitCoinTrade()
        {
            if(listaOrder != null)
            {
                Comunicacao.EscreverNaTela("inserindo as ordens");
                ordersDAO.Inserir(listaOrder);
            }
            else
            {
                Comunicacao.EscreverNaTela("a lista de ordens está vazia, portanto os dados não serão inseridos no banco de dados");
            }

            if (listaTrade != null)
            {
                Comunicacao.EscreverNaTela("inserindo os trades");
                tradesDAO.Inserir(listaTrade);
            }
            else
            {
                Comunicacao.EscreverNaTela("a lista de trades está vazia, portanto os dados não serão inseridos no banco de dados");
            }

            if (ticker != null)
            {
                Comunicacao.EscreverNaTela("inserindo o ticker");
                tickerDAO.Inserir(ticker);
            }
            else
            {
                Comunicacao.EscreverNaTela("o ticker está vazio, portanto os dados não serão inseridos no banco de dados");
            }
        }


    }
}
