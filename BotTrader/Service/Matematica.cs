using System;
using System.Collections.Generic;
using System.Linq;
using BotTrader.DAO;
using BotTrader.Model.Trades;
using static BotTrader.Model.Matematica;

namespace BotTrader.Service
{
    internal class Matematica
    {
        TradesDAO tradesDAO;

        internal Matematica()
        {
            tradesDAO = new TradesDAO();
        }

        /// <summary>
        /// Análises para identificar se existe insights ou alertas para comprar
        /// </summary>
        internal void AnalisarCompra()
        {
            AnalisarCrescimentoValorCompraEQuantidadeCompras();
            AnalisarSeHouveQuedaContinuaEVoltouASubir();
            AnalisarValorCompraAbaixoValorUltimaVenda();

            AnalisarQuantidadeCompraAcimaMedia();
        }

        /// <summary>
        /// Análise para identificar se o valor de compra está em uma crescente contínua e tem muita gente comprando
        /// </summary>
        private ResultadoAnalisaCrescimentoValorCompraEQuantidadeCompras AnalisarCrescimentoValorCompraEQuantidadeCompras()
        {
            #region Análise variação

            DadosConsultaTradeBD dadosConsultaTradeBD = new DadosConsultaTradeBD()
            {
                Tipo = "buy",
                DataInicial = DateTime.Now.AddHours(-3).ToString("yyyyMMdd HH:mm:ss"),
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

            decimal variacaoMedia = listaVariacao.Average();

            #endregion

            #region Análise qtd de compras

            //Verifica se a quantidade de compras na última hora está muito acima da hora anterior
            List<Trade> listaTradeQtdCompra = listaTradeVariacao;

            var listaTradeQtdCompraAgrupada = listaTradeQtdCompra
            .GroupBy(u => u.date.ToString("HH"))
            .Select(grp => grp.ToList())
            .ToList();

            decimal crescimentoQtdCompra;

            int indiceInicial = listaTradeQtdCompraAgrupada.Count == 4 ? 2 : 1;

            crescimentoQtdCompra = (Convert.ToDecimal(listaTradeQtdCompraAgrupada[indiceInicial].Count) / Convert.ToDecimal(listaTradeQtdCompraAgrupada[indiceInicial-1].Count)) - 1;

            #endregion

            return new ResultadoAnalisaCrescimentoValorCompraEQuantidadeCompras()
            {
                CrescimentoQtdCompra = crescimentoQtdCompra,
                VariacaoMediaCrescimento = variacaoMedia
            };
        }

        /// <summary>
        /// Analise para identificar se houve um período de queda do valor de compra e o mesmo voltou a subir
        /// </summary>
        private void AnalisarSeHouveQuedaContinuaEVoltouASubir()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Análise para identificar se o valor de compra está X % abaixo do valor da última venda
        /// </summary>
        private void AnalisarValorCompraAbaixoValorUltimaVenda()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Análise para identificar se a quantidade de compra está X % acima da média de um determinado período
        /// </summary>
        private void AnalisarQuantidadeCompraAcimaMedia()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Análises para identificar se existe insights ou alertas para vender
        /// </summary>
        internal void AnalisarVenda()
        {
            AnalisarValorVendaAcimaValorUltimaCompra();
            AnalisarQuantidadeVendaAcimaMedia();

        }

        private void AnalisarValorVendaAcimaValorUltimaCompra()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Análise para identificar se a quantidade de venda está X % acima da média de um determinado período
        /// </summary>
        private void AnalisarQuantidadeVendaAcimaMedia()
        {
            throw new NotImplementedException();
        }

        private decimal CalcularVariacao(decimal a, decimal b)
        {
            decimal difference = a - b;
            decimal variation = difference / a;
            return variation * 100;
        }

    }
}
