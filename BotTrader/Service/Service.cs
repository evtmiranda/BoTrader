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

        private void ConsultarInformacoesBitCoinTrade()
        {
            RequisicaoRest reqRest = new RequisicaoRest();

            Comunicacao.EscreverNaTela("consultando as ordens");
            listaOrder = reqRest.GetOrders();

            Comunicacao.EscreverNaTela("consultando os trades");
            listaTrade = reqRest.GetTrades();

            Comunicacao.EscreverNaTela("consultando o ticker");
            ticker = reqRest.GetTicker();
        }

        private void InserirNoBancoInformacoesBitCoinTrade()
        {
            if(listaOrder != null)
            {
                Comunicacao.EscreverNaTela("inserindo as ordens");
                new OrdersDAO().Inserir(listaOrder);
            }
            else
            {
                Comunicacao.EscreverNaTela("a lista de ordens está vazia, portanto os dados não serão inseridos no banco de dados");
            }

            if (listaTrade != null)
            {
                Comunicacao.EscreverNaTela("inserindo os trades");
                new TradesDAO().Inserir(listaTrade);
            }
            else
            {
                Comunicacao.EscreverNaTela("a lista de trades está vazia, portanto os dados não serão inseridos no banco de dados");
            }

            if (ticker != null)
            {
                Comunicacao.EscreverNaTela("inserindo o ticker");
                new TickerDAO().Inserir(ticker);
            }
            else
            {
                Comunicacao.EscreverNaTela("o ticker está vazio, portanto os dados não serão inseridos no banco de dados");
            }
        }

        /// <summary>
        /// Executa diversas operações para identificar se é um bom momento para comprar ou vender. Se for um bom bomento,
        /// enviará mensagem pelos canais configurados
        /// </summary>
        internal void Pensar()
        {

        }

    }
}
