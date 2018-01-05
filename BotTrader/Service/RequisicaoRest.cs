using System.Collections.Generic;
using System.Linq;
using BotTrader.Model.Orders;
using BotTrader.Model.Ticker;
using BotTrader.Model.Trades;
using RestSharp;

namespace BotTrader.Service
{
    internal class RequisicaoRest
    {
        public Ticker GetTicker()
        {
            Ticker Ticker;

            var client = new RestClient("https://api.bitcointrade.com.br/");
            var request = new RestRequest("v1/public/BTC/ticker", Method.GET);
            var queryResult = client.Execute<List<Ticker>>(request).Data;

            if (queryResult == null)
            {
                return null;
            }

            Ticker = queryResult.First();

            return Ticker;
        }

        public Orders GetOrders()
        {
            Orders orders;

            var client = new RestClient("https://api.bitcointrade.com.br/");
            var request = new RestRequest("v1/public/BTC/orders", Method.GET);
            var queryResult = client.Execute<List<Orders>>(request).Data;

            if (queryResult == null)
            {
                return null;
            }

            orders = queryResult.First();

            return orders;
        }

        public Trades GetTrades()
        {
            Trades trades;

            var client = new RestClient("https://api.bitcointrade.com.br/");
            var request = new RestRequest("v1/public/BTC/trades", Method.GET);
            request.AddQueryParameter("start_time", "2018-01-04T00:00:00-03:00");
            request.AddQueryParameter("end_time", "2018-01-04T23:59:59-03:00");
            request.AddQueryParameter("page_size", "10000");
            request.AddQueryParameter("current_page", "1");

            var queryResult = client.Execute<List<Trades>>(request).Data;

            if (queryResult == null)
            {
                return null;
            }

            trades = queryResult.First();

            return trades;
        }
    }
}
