using System;
using System.Collections.Generic;
using System.Linq;
using BotTrader.DAO;
using BotTrader.Model.Orders;
using BotTrader.Model.Ticker;
using BotTrader.Model.Trades;
using Newtonsoft.Json;
using RestSharp;

namespace BotTrader.Service
{
    internal class RequisicaoRest
    {
        public Ticker GetTicker()
        {
            try
            {
                Ticker ticker;

                var client = new RestClient("https://api.bitcointrade.com.br/");
                var request = new RestRequest("v1/public/BTC/ticker", Method.GET);
                var queryResult = client.Execute(request);

                if (queryResult == null)
                {
                    return null;
                }

                ticker = JsonConvert.DeserializeObject<Ticker>(queryResult.Content);

                return ticker;
            }
            catch (Exception ex)
            {
                Comunicacao.EscreverNaTela("Ocorreu um erro ao consultar o ticker: " + ex.Message);
                new LogDAO().Inserir(ex);
                return null;
            }
        }

        public Orders GetOrders()
        {
            try
            {
                Orders orders;

                var client = new RestClient("https://api.bitcointrade.com.br/");
                var request = new RestRequest("v1/public/BTC/orders", Method.GET);
                var queryResult = client.Execute(request);

                if (queryResult == null)
                {
                    return null;
                }

                orders = JsonConvert.DeserializeObject<Orders>(queryResult.Content);

                return orders;
            }
            catch (Exception ex)
            {
                Comunicacao.EscreverNaTela("Ocorreu um erro ao consultar as ordens: " + ex.Message);
                new LogDAO().Inserir(ex);
                return null;
            }

        }

        public Trades GetTrades(DadosConsultaTrade dadosConsultaTrade)
        {
            try
            {
                Trades trades;

                var client = new RestClient("https://api.bitcointrade.com.br/");
                var request = new RestRequest("v1/public/BTC/trades", Method.GET);
                request.AddQueryParameter("start_time", dadosConsultaTrade.DataInicial);
                request.AddQueryParameter("end_time", dadosConsultaTrade.DataFinal);
                request.AddQueryParameter("page_size", dadosConsultaTrade.TamanhoPagina.ToString());
                request.AddQueryParameter("current_page", dadosConsultaTrade.NumeroPagina.ToString());

                var queryResult = client.Execute(request);

                if (queryResult == null)
                {
                    return null;
                }

                trades = JsonConvert.DeserializeObject<Trades>(queryResult.Content);

                return trades;
            }
            catch (Exception ex)
            {
                Comunicacao.EscreverNaTela("Ocorreu um erro ao consultar os trades: " + ex.Message);
                new LogDAO().Inserir(ex);
                return null;
            }
        }
    }
}
