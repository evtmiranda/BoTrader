using System;
using System.Linq;
using BotTrader.DAO;
using BotTrader.Model.Orders;
using BotTrader.Model.Ticker;
using BotTrader.Model.Trades;

namespace BotTrader.Service
{
    public class Service
    {
        Orders listaOrder;
        Trades listaTrade;
        Ticker ticker;
        DadosConsultaTrade dadosConsultaTrade;

        OrdersDAO ordersDAO;
        TradesDAO tradesDAO;
        TickerDAO tickerDAO;

        Matematica matematica;

        public Service()
        {
            ordersDAO = new OrdersDAO();
            tradesDAO = new TradesDAO();
            tickerDAO = new TickerDAO();

            matematica = new Matematica();
        }

        /// <summary>
        /// Consulta as informações de trade e insere no banco de dados
        /// </summary>
        public void ProcessarInformacoesBitCoinTrade()
        {
            Comunicacao.EscreverNaTela("iniciando a consulta de informações de trade");
            ConsultarInformacoesBitCoinTrade();

            Comunicacao.EscreverNaTela("inserindo as informações de trade no banco de dados");
            InserirNoBancoInformacoesBitCoinTrade();
        }

        public void GerarInsightEAlerta()
        {
            DadosAnaliseCompra dadosAnaliseCompra = matematica.AnalisarCompra();
            DadosAnaliseVenda dadosAnaliseVenda = matematica.AnalisarVenda();

            AnalisarResultadoEEnviarMensagem(dadosAnaliseCompra, dadosAnaliseVenda);
        }

        private void AnalisarResultadoEEnviarMensagem(DadosAnaliseCompra dadosAnaliseCompra, DadosAnaliseVenda dadosAnaliseVenda)
        {
            DadosConsultaTickerBD dadosConsultaTickerBD = new DadosConsultaTickerBD()
            {
                QtdRegistros = 1,
                NomeCampoOrdenacao = "date",
                TipoOrdenacao = "DESC"
            };

            Model.Ticker.Data ticker = tickerDAO.Consultar(dadosConsultaTickerBD).First();

            string tipoUltimaNegociacao = tradesDAO.ConsultarTipoUltimaNegociacao();

            if(tipoUltimaNegociacao == "sell")
            {
                //Verifica se o valor para compra está menor que o valor da última venda
                if (dadosAnaliseCompra.PercentualCrescimentoValorCompraEmRelacaoValorUltimaVenda <= Convert.ToDecimal(-0.009))
                {
                    Comunicacao.EnviarMensagem(
                        string.Format("Boa notícia: o valor para compra R${0} está {1}% menor que o valor da última venda R${2}.",
                        ticker.sell,
                        dadosAnaliseVenda.PercentualGanhoVenda.ToString("#.###"),
                        dadosAnaliseVenda.ValorUltimaVenda));
                }

                if(dadosAnaliseCompra.PercentualMedioCrescimentoValorCompraUltimas3Horas >= Convert.ToDecimal(0.01))
                {
                    Comunicacao.EnviarMensagem(
                        string.Format("Boa notícia: o valor para compra R${0} está em uma crescente contínua. Cresceu {1}% nas últimas 3 horas. Valor da última compra R${2}. Valor da última venda R${3}.",
                        ticker.sell,
                        dadosAnaliseVenda.PercentualGanhoVenda.ToString("#.###"),
                        dadosAnaliseVenda.ValorUltimaCompra,
                        dadosAnaliseVenda.ValorUltimaVenda));
                }
            }
            else if(tipoUltimaNegociacao == "buy")
            {
                if (dadosAnaliseVenda.PercentualGanhoVenda >= Convert.ToDecimal(0.009))
                {
                    Comunicacao.EnviarMensagem(
                    string.Format("Boa notícia: o valor para venda R${0} está {1}% maior que o valor da última compra R${2}.",
                    ticker.buy,
                    dadosAnaliseVenda.PercentualGanhoVenda.ToString("#.###"),
                    dadosAnaliseVenda.ValorUltimaCompra));
                }
            }
        }

        private void ConsultarInformacoesBitCoinTrade()
        {
            RequisicaoRest reqRest = new RequisicaoRest();

            Comunicacao.EscreverNaTela("consultando as ordens");
            listaOrder = reqRest.GetOrders();

            dadosConsultaTrade = new DadosConsultaTrade
            {
                DataInicial = tradesDAO.ConsultarUltimaDataProcessamento(),
                DataFinal = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss-00:00"),
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
            if (listaOrder != null)
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
