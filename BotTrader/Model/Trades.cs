using System;
using System.Collections.Generic;
using static BotTrader.Model.Matematica;

namespace BotTrader.Model.Trades
{
    public class Pagination
    {
        public int total_pages { get; set; }
        public int current_page { get; set; }
        public int page_size { get; set; }
        public int registers_count { get; set; }
    }

    public class Trade
    {
        public string type { get; set; }
        public decimal amount { get; set; }
        public decimal unit_price { get; set; }
        public string active_order_code { get; set; }
        public string passive_order_code { get; set; }
        public DateTime date { get; set; }
    }

    public class Data
    {
        public Pagination pagination { get; set; }
        public IList<Trade> trades { get; set; }
    }

    /// <summary>
    /// Lista de trades
    /// </summary>
    public class Trades
    {
        public object message { get; set; }
        public Data data { get; set; }
    }

    /// <summary>
    /// Dados para consulta dos dados de trade na API da bitcointrade
    /// </summary>
    public class DadosConsultaTrade
    {
        /// <summary>
        /// Formato: yyyy-MM-ddThh:mm:ss-03:00
        /// </summary>
        public string DataInicial { get; set; }

        /// <summary>
        /// Formato: yyyy-MM-ddThh:mm:ss-03:00
        /// </summary>
        public string DataFinal { get; set; }
        public int TamanhoPagina { get; set; }
        public int NumeroPagina { get; set; }
    }

    public class DadosDataUltimoProcessamento
    {
        public string data { get; set; }
    }

    public class DadosTipoUltimaNegociacao
    {
        /// <summary>
        /// buy ou sell
        /// </summary>
        public string TipoUltimaNegociacao { get; set; }
    }

    public class DadosUltimoValorVenda
    {
        public decimal vlr_venda { get; set; }
    }

    public class DadosUltimoValorCompra
    {
        public decimal vlr_compra { get; set; }
    }

    public class DadosAnaliseCompra
    {
        /// <summary>
        /// Indica o percentual médio de crescimento do valor da compra nas últimas 3 horas.
        /// Se o percentual for >= 1%, se torna interessante comprar, pois o valor está em uma crescente continua.
        /// </summary>
        public decimal PercentualMedioCrescimentoValorCompraUltimas3Horas { get; set; }

        /// <summary>
        /// Indica em percentual o quanto o valor para compra está maior que o valor da última venda efetuada.
        /// Se o percentual for menor ou igual a -0,99 se torna interessante comprar, pois o valor para compra está menor que o valor da última venda.
        /// </summary>
        public decimal PercentualCrescimentoValorCompraEmRelacaoValorUltimaVenda { get; set; }

        /// <summary>
        /// Indica o percentual de crescimento da quantidade de compras da última hora em relação a hora anterior.
        /// </summary>
        public decimal PercentualCrescimentoQtdComprasUltimaHora { get; set; }

        /// <summary>
        /// Indica o percentual de crescimento da quantidade de trades(compra e venda) da última hora em relação a hora anterior.
        /// </summary>
        public decimal PercentualCrescimentoQtdTradesUltimaHora { get; set; }

        public decimal ValorUltimaCompra { get; set; }
        public decimal ValorUltimaVenda { get; set; }
    }

    public class DadosAnaliseVenda
    {
        /// <summary>
        /// Indica o percentual de ganho na venda, comparando o valor de venda atual com o valor da última compra efetuada.
        /// </summary>
        public decimal PercentualGanhoVenda { get; set; }

        /// <summary>
        /// Indica o percentual de crescimento da quantidade de vendas da última hora em relação a hora anterior.
        /// </summary>
        public decimal PercentualCrescimentoQtdVendasUltimaHora { get; set; }

        /// <summary>
        /// Indica o percentual de crescimento da quantidade de trades(compra e venda) da última hora em relação a hora anterior.
        /// </summary>
        public decimal PercentualCrescimentoQtdTradesUltimaHora { get; set; }

        public decimal ValorUltimaCompra { get; set; }
        public decimal ValorUltimaVenda { get; set; }
    }

    public class DadosConsultaTradeBD
    {
        /// <summary>
        /// buy ou sell
        /// </summary>
        public string Tipo { get; set; }

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
