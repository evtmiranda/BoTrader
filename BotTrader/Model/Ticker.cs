using System;

namespace BotTrader.Model.Ticker
{
    public class Data
    {
        public double high { get; set; }
        public double low { get; set; }
        public double volume { get; set; }
        public int trades_quantity { get; set; }

        /// <summary>
        /// Valor do último trade, seja ele de compra ou de venda
        /// </summary>
        public double last { get; set; }

        /// <summary>
        /// Melhor oferta de compra, ou seja, valor pelo qual se pode vender
        /// </summary>
        public double buy { get; set; }

        /// <summary>
        /// Melhor oferta de venda, ou seja, valor pelo qual se pode comprar
        /// </summary>
        public double sell { get; set; }
        public DateTime date { get; set; }
    }

    /// <summary>
    /// Resumo das últimas 24 horas de negociações
    /// </summary>
    public class Ticker
    {
        public object message { get; set; }
        public Data data { get; set; }
    }

}