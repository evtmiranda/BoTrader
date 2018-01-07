using System;
using System.Collections.Generic;
using BotTrader.DAO;
using BotTrader.Model.Trades;

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
        private void AnalisarCrescimentoValorCompraEQuantidadeCompras()
        {
            DadosConsultaTradeBD dadosConsultaTradeBD = new DadosConsultaTradeBD()
            {
                Tipo = "buy",
                DataInicial = DateTime.Now.AddHours(-3).ToString("yyyyMMdd HH:mm:ss"),
                DataFinal = DateTime.Now.ToString("yyyyMMdd HH:mm:ss")
            };

            List<Trade> listaTrade = tradesDAO.Consultar(dadosConsultaTradeBD);

            //TODO: Implementar a lógica de análise de crescimento e quantidade compras
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
