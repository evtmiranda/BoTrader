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
        public double amount { get; set; }
        public double unit_price { get; set; }
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
}
