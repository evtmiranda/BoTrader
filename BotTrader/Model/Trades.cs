using System;
using System.Collections.Generic;

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

    public class DataUltimoProcessamento
    {
        public string data { get; set; }
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
