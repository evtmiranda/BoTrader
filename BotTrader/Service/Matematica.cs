using System;
using System.Collections.Generic;
using System.Linq;
using BotTrader.DAO;
using BotTrader.Model.Ticker;
using BotTrader.Model.Trades;
using static BotTrader.Model.Matematica;

namespace BotTrader.Service
{
    public class Matematica
    {
        TradesDAO tradesDAO;
        TickerDAO tickerDAO;

        public Matematica()
        {
            tradesDAO = new TradesDAO();
            tickerDAO = new TickerDAO();
        }

        #region análise compra

        /// <summary>
        /// Análises para identificar se existe insights ou alertas para comprar
        /// </summary>
        public DadosAnaliseCompra AnalisarCompra()
        {
            decimal percentualMedioCrescimentoValorCompraUltimas3Horas = CalcularPercentualMedioCrescimentoValorCompra();
            decimal percentualCrescimentoValorCompraEmRelacaoValorUltimaVenda = CompararValorCompraXValorUltimaVenda();
            decimal percentualCrescimentoQtdComprasUltimaHora = CalcularCrescimentoQuantidadeCompraUltimaHora();
            decimal percentualCrescimentoQtdTradesUltimaHora = CalcularCrescimentoQtdTradesUltimaHora();
            decimal valorUltimaCompra = tradesDAO.ConsultarUltimoValorCompra().vlr_compra;
            decimal valorUltimaVenda = tradesDAO.ConsultarUltimoValorVenda().vlr_venda;

            return new DadosAnaliseCompra()
            {
                PercentualMedioCrescimentoValorCompraUltimas3Horas = percentualMedioCrescimentoValorCompraUltimas3Horas,
                PercentualCrescimentoQtdComprasUltimaHora = percentualCrescimentoQtdComprasUltimaHora,
                PercentualCrescimentoValorCompraEmRelacaoValorUltimaVenda = percentualCrescimentoValorCompraEmRelacaoValorUltimaVenda,
                PercentualCrescimentoQtdTradesUltimaHora = percentualCrescimentoQtdTradesUltimaHora,
                ValorUltimaCompra = valorUltimaCompra,
                ValorUltimaVenda = valorUltimaVenda
            };
        }

        /// <summary>
        /// Calcula o percentual médio de crescimento do valor de compra nas últimas 3 horas
        /// </summary>
        private decimal CalcularPercentualMedioCrescimentoValorCompra()
        {
            DadosConsultaTradeBD dadosConsultaTradeBD = new DadosConsultaTradeBD()
            {
                Tipo = "buy",
                DataInicial = DateTime.Now.AddHours(-3).ToString("yyyyMMdd HH:00:00"),
                DataFinal = DateTime.Now.ToString("yyyyMMdd HH:mm:ss"),
                NomeCampoOrdenacao = "cod_bitcoin_trade_trade",
                TipoOrdenacao = "ASC"
            };

            //Consulta os trades de compra das últimas 3 horas ordenados pelo mais recente
            List<Trade> listaTradeVariacao = tradesDAO.Consultar(dadosConsultaTradeBD);

            List<decimal> listaVariacao = new List<decimal>();
            decimal valorTradeAnterior = 0;
            decimal variacaoComparacaoUltimoTrade = 0;

            foreach (var trade in listaTradeVariacao)
            {
                if (valorTradeAnterior == 0)
                {
                    valorTradeAnterior = trade.unit_price;
                }
                    
                variacaoComparacaoUltimoTrade = (trade.unit_price / valorTradeAnterior) - 1;

                valorTradeAnterior = trade.unit_price;

                listaVariacao.Add(variacaoComparacaoUltimoTrade);
            }

            decimal percentualMedioCrescimentoValorCompra = (listaVariacao.Sum() * 100 / listaVariacao.Count) * 100;

            return percentualMedioCrescimentoValorCompra;
        }

        /// <summary>
        /// Analise para identificar se houve um período de queda do valor de compra e o mesmo voltou a subir
        /// </summary>
        private void AnalisarSeHouveQuedaContinuaEVoltouASubir()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Compara o valor de compra atual com o valor da última venda e devolve a diferença percentual do valor de compra.
        /// Quando a diferença percentual é negativa, significa que o valor para compra está abaixo do valor da última venda
        /// </summary>
        private decimal CompararValorCompraXValorUltimaVenda()
        {
            DadosConsultaTickerBD dadosConsultaTickerBD = new DadosConsultaTickerBD()
            {
                QtdRegistros = 1,
                NomeCampoOrdenacao = "date",
                TipoOrdenacao = "DESC"
            };

            Model.Ticker.Data ticker = tickerDAO.Consultar(dadosConsultaTickerBD).First();

            DadosUltimoValorVenda ultimoValorVenda = tradesDAO.ConsultarUltimoValorVenda();

            if (ultimoValorVenda == null)
                return Convert.ToDecimal(0);

            decimal valorCompraAtual = ticker.sell;
            decimal valorUltimaVenda = ultimoValorVenda.vlr_venda;

            decimal diferencaValorCompra = valorCompraAtual - valorUltimaVenda;
            decimal diferencaValorCompraPercentual = diferencaValorCompra / valorUltimaVenda;

            return diferencaValorCompraPercentual;
        }

        /// <summary>
        /// Calcula o percentual de crescimento da quantidade de compras da última hora
        /// </summary>
        private decimal CalcularCrescimentoQuantidadeCompraUltimaHora()
        {
            int qtdHorasAnalise = -2;
            int qtdHorasAnalisePositivo = qtdHorasAnalise * -1;

            DadosConsultaTradeBD dadosConsultaTradeBD = new DadosConsultaTradeBD()
            {
                Tipo = "buy",
                DataInicial = DateTime.Now.AddHours(qtdHorasAnalise).ToString("yyyyMMdd HH:00:00"),
                DataFinal = DateTime.Now.ToString("yyyyMMdd HH:mm:ss"),
                NomeCampoOrdenacao = "cod_bitcoin_trade_trade",
                TipoOrdenacao = "ASC"
            };

            List<Trade> listaTrade = tradesDAO.Consultar(dadosConsultaTradeBD);

            List<Trade> listaTradeQtdCompra = listaTrade;

            var listaTradeQtdCompraAgrupada = listaTradeQtdCompra
            .GroupBy(u => u.date.ToString("HH"))
            .Select(grp => grp.ToList())
            .ToList();

            int indiceInicial = listaTradeQtdCompraAgrupada.Count == (qtdHorasAnalisePositivo + 1) ? qtdHorasAnalisePositivo -1 : qtdHorasAnalisePositivo;

            decimal crescimentoQuantidadeCompraUltimaHora = (Convert.ToDecimal(listaTradeQtdCompraAgrupada[indiceInicial].Count) / Convert.ToDecimal(listaTradeQtdCompraAgrupada[indiceInicial - 1].Count)) - 1;

            return crescimentoQuantidadeCompraUltimaHora;
        }

        private decimal CalcularCrescimentoQtdTradesUltimaHora()
        {
            int qtdHorasAnalise = -2;
            int qtdHorasAnalisePositivo = qtdHorasAnalise * -1;

            DadosConsultaTradeBD dadosConsultaTradeBD = new DadosConsultaTradeBD()
            {
                DataInicial = DateTime.Now.AddHours(qtdHorasAnalise).ToString("yyyyMMdd HH:00:00"),
                DataFinal = DateTime.Now.ToString("yyyyMMdd HH:mm:ss"),
                NomeCampoOrdenacao = "cod_bitcoin_trade_trade",
                TipoOrdenacao = "ASC"
            };

            List<Trade> listaTrade = tradesDAO.Consultar(dadosConsultaTradeBD);

            List<Trade> listaTradeQtdCompra = listaTrade;

            var listaTradeQtdCompraAgrupada = listaTradeQtdCompra
            .GroupBy(u => u.date.ToString("HH"))
            .Select(grp => grp.ToList())
            .ToList();

            int indiceInicial = listaTradeQtdCompraAgrupada.Count == (qtdHorasAnalisePositivo + 1) ? qtdHorasAnalisePositivo - 1 : qtdHorasAnalisePositivo;

            decimal crescimentoQuantidadeCompraUltimaHora = (Convert.ToDecimal(listaTradeQtdCompraAgrupada[indiceInicial].Count) / Convert.ToDecimal(listaTradeQtdCompraAgrupada[indiceInicial - 1].Count)) - 1;

            return crescimentoQuantidadeCompraUltimaHora;
        }

        #endregion

        #region análise venda

        /// <summary>
        /// Análises para identificar se existe insights ou alertas para vender
        /// </summary>
        public DadosAnaliseVenda AnalisarVenda()
        {
            decimal percentualGanhoVenda = CompararValorVendaXValorUltimaCompra();
            decimal percentualCrescimentoQtdVendas = CalcularCrescimentoQuantidadeVendaUltimaHora();
            decimal percentualCrescimentoQtdTradesUltimaHora = CalcularCrescimentoQtdTradesUltimaHora();
            decimal valorUltimaCompra = tradesDAO.ConsultarUltimoValorCompra().vlr_compra;
            decimal valorUltimaVenda = tradesDAO.ConsultarUltimoValorVenda().vlr_venda;

            return new DadosAnaliseVenda()
            {
                PercentualCrescimentoQtdVendasUltimaHora = percentualCrescimentoQtdVendas,
                PercentualGanhoVenda = percentualGanhoVenda,
                PercentualCrescimentoQtdTradesUltimaHora = percentualCrescimentoQtdTradesUltimaHora,
                ValorUltimaCompra = valorUltimaCompra,
                ValorUltimaVenda = valorUltimaVenda
            };
        }

        /// <summary>
        /// Compara o valor de venda atual com o valor da última compra e devolve a diferença percentual do valor de venda.
        /// Quando a diferença percentual é positiva, significa que o valor para venda está acima do valor da última compra
        /// </summary>
        /// <returns></returns>
        private decimal CompararValorVendaXValorUltimaCompra()
        {
            DadosConsultaTickerBD dadosConsultaTickerBD = new DadosConsultaTickerBD()
            {
                QtdRegistros = 1,
                NomeCampoOrdenacao = "date",
                TipoOrdenacao = "DESC"
            };

            Model.Ticker.Data ticker = tickerDAO.Consultar(dadosConsultaTickerBD).First();

            DadosUltimoValorCompra ultimoValorCompra = tradesDAO.ConsultarUltimoValorCompra();

            if(ultimoValorCompra == null)
                return Convert.ToDecimal(0);

            decimal valorVendaAtual = ticker.buy;
            decimal valorUltimaCompra = ultimoValorCompra.vlr_compra;

            decimal diferencaValorVenda = valorVendaAtual - valorUltimaCompra;
            decimal diferencaValorVendaPercentual = diferencaValorVenda / valorUltimaCompra;

            return diferencaValorVendaPercentual;
        }

        /// <summary>
        /// Calcula o percentual de crescimento da quantidade de vendas da última hora
        /// </summary>
        private decimal CalcularCrescimentoQuantidadeVendaUltimaHora()
        {
            int qtdHorasAnalise = -2;
            int qtdHorasAnalisePositivo = qtdHorasAnalise * -1;

            DadosConsultaTradeBD dadosConsultaTradeBD = new DadosConsultaTradeBD()
            {
                Tipo = "sell",
                DataInicial = DateTime.Now.AddHours(qtdHorasAnalise).ToString("yyyyMMdd HH:00:00"),
                DataFinal = DateTime.Now.ToString("yyyyMMdd HH:mm:ss"),
                NomeCampoOrdenacao = "cod_bitcoin_trade_trade",
                TipoOrdenacao = "ASC"
            };

            List<Trade> listaTrade = tradesDAO.Consultar(dadosConsultaTradeBD);

            List<Trade> listaTradeQtdVenda = listaTrade;

            var listaTradeQtdVendaAgrupada = listaTradeQtdVenda
            .GroupBy(u => u.date.ToString("HH"))
            .Select(grp => grp.ToList())
            .ToList();

            int indiceInicial = listaTradeQtdVendaAgrupada.Count == (qtdHorasAnalisePositivo + 1) ? qtdHorasAnalisePositivo - 1 : qtdHorasAnalisePositivo;

            decimal crescimentoQuantidadeVendaUltimaHora = (Convert.ToDecimal(listaTradeQtdVendaAgrupada[indiceInicial].Count) / Convert.ToDecimal(listaTradeQtdVendaAgrupada[indiceInicial - 1].Count)) - 1;

            return crescimentoQuantidadeVendaUltimaHora;
        }

        #endregion

        private decimal CalcularVariacao(decimal a, decimal b)
        {
            decimal difference = a - b;
            decimal variation = difference / a;
            return variation * 100;
        }

    }
}
