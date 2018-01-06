using System.Collections.Generic;

namespace BotTrader.Model.Orders
{
    public class Bid
    {
        public decimal unit_price { get; set; }
        public string code { get; set; }
        public decimal amount { get; set; }
    }

    public class Ask
    {
        public decimal unit_price { get; set; }
        public string code { get; set; }
        public decimal amount { get; set; }
    }

    public class Data
    {
        /// <summary>
        /// Ordens de compra
        /// </summary>
        public List<Bid> bids { get; set; }

        /// <summary>
        /// Ordens de venda
        /// </summary>
        public List<Ask> asks { get; set; }
    }

    public class Orders
    {
        public object message { get; set; }
        public Data data { get; set; }
    }
}
