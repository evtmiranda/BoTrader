using System;

namespace BotTrader.Model
{
    public class Data
    {
        public decimal high { get; set; }
        public decimal low { get; set; }
        public double volume { get; set; }
        public int trades_quantity { get; set; }
        public decimal last { get; set; }

        //Valor para compra - ordem mercado
        public decimal sell { get; set; }

        //Valor para venda - ordem mercado
        public decimal buy { get; set; }
        public DateTime date { get; set; }
    }

    public class ResultRequestBitCoinTrade
    {
        public object message { get; set; }
        public Data data { get; set; }
    }

    public class ResultGetLastPurchaseValue
    {
        public decimal value { get; set; }
    }
}
