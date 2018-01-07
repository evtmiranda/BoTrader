using System;

namespace BotTrader.Model.Ticker
{
    public class Data
    {
        /// <summary>
        /// Maior valor de negociação nas últimas 24 horas
        /// </summary>
        public decimal high { get; set; }

        /// <summary>
        /// Menor valor de negociação nas últimas 24 horas
        /// </summary>
        public decimal low { get; set; }

        /// <summary>
        /// Quantidade negociada nas últimas 24 horas para a cripto moeda informada
        /// </summary>
        public decimal volume { get; set; }

        /// <summary>
        /// Quantidade de trades realizados nas últimas 24 horas para a cripto moeda informada
        /// </summary>
        public int trades_quantity { get; set; }

        /// <summary>
        /// Valor do último trade, seja ele de compra ou de venda
        /// </summary>
        public decimal last { get; set; }

        /// <summary>
        /// Melhor oferta de compra, ou seja, valor pelo qual se pode vender
        /// </summary>
        public decimal buy { get; set; }

        /// <summary>
        /// Melhor oferta de venda, ou seja, valor pelo qual se pode comprar
        /// </summary>
        public decimal sell { get; set; }
        public DateTime date { get; set; }
    }

    /// <summary>
    /// Resumo das últimas 24 horas de negociação
    /// </summary>
    public class Ticker
    {
        public object message { get; set; }
        public Data data { get; set; }
    }

    public class DadosConsultaTickerBD
    {
        /// <summary>
        /// Quantidade de registros
        /// </summary>
        public int QtdRegistros { get; set; }

        /// <summary>
        /// Formato: yyyyMMdd HH:mm:ss
        /// </summary>
        public string DataInicial { get; set; }

        /// <summary>
        /// Formato: yyyyMMdd HH:mm:ss
        /// </summary>
        public string DataFinal { get; set; }

        /// <summary>
        /// Nome do campo para ordenação de
        /// </summary>
        public string NomeCampoOrdenacao { get; set; }

        /// <summary>
        /// ASC ou DESC
        /// </summary>
        public string TipoOrdenacao { get; set; }
    }

}