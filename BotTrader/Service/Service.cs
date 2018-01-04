using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BotTrader.Model;
using IronPython.Hosting;
using RestSharp;

namespace BotTrader.Service
{
    internal class Service
    {
        public ResultRequestBitCoinTrade GetTicker()
        {
            ResultRequestBitCoinTrade resultRequestBitCoinTrade;

            var client = new RestClient("https://api.bitcointrade.com.br/");
            var request = new RestRequest("v1/public/BTC/ticker", Method.GET);
            var queryResult = client.Execute<List<ResultRequestBitCoinTrade>>(request).Data;

            resultRequestBitCoinTrade = queryResult.First();

            return resultRequestBitCoinTrade;
        }

        /// <summary>
        /// Compara o valor de compra e de venda com o último valor de compra e envia uma mensagem caso a variação seja maior que X
        /// </summary>
        /// <param name="resultRequestBitCoinTrade">Dados de valores atuais de trade</param>
        /// <param name="resultGetLastPurchaseValue">Valor da última compra</param>
        internal void Think(ResultRequestBitCoinTrade resultRequestBitCoinTrade, ResultGetLastBuyValue resultGetLastBuyValue, ResultGetLastSaleValue resultGetLastSaleValue)
        {
            decimal valueCompareVariation = Convert.ToDecimal(ConfigurationManager.AppSettings.Get("valueVariation"));
            decimal valueCompareVariationAlarm = Convert.ToDecimal(ConfigurationManager.AppSettings.Get("valueVariationAlarm"));

            decimal buyValueVariation = CalculateVariance(resultRequestBitCoinTrade.data.buy, resultGetLastBuyValue.value);
            decimal sellValueVariation = CalculateVariance(resultRequestBitCoinTrade.data.sell, resultGetLastSaleValue.value);
            string message;

            if (sellValueVariation <= - valueCompareVariation)
            {
                message = string.Format("bom momento para comprar: o valor para compra está {0}% menor que o valor da sua última venda.", sellValueVariation.ToString("#.##"));
                SendMessage(message, true);
            }

            if (buyValueVariation >= valueCompareVariation)
            {
                message = string.Format("bom momento para vender: o valor para venda está {0}% maior que o valor da última compra.", buyValueVariation.ToString("#.##"));
                SendMessage(message, false);
            }


            message = "valor para compra: R$ " + resultRequestBitCoinTrade.data.sell;
            SendMessage(message, false);
            message = "valor última compra: R$ " + resultGetLastBuyValue.value;
            SendMessage(message, false);

            message = "valor compra X valor última venda: " + sellValueVariation.ToString("#.##") + "%";
            SendMessage(message, false);

            message = "valor para venda: R$ " + resultRequestBitCoinTrade.data.buy;
            SendMessage(message, false);
            message = "valor última venda: R$ " + resultGetLastSaleValue.value;
            SendMessage(message, false);

            message = "valor venda X valor última compra: " + buyValueVariation.ToString("#.##") + "%";
            SendMessage(message, false);
        }

        private decimal CalculateVariance(decimal a, decimal b)
        {
            decimal difference = a - b;
            decimal variation = difference / a;
            return variation * 100;
        }

        private void SendMessage(string message, bool allChannels)
        {
            string messageLocal = DateTime.UtcNow.ToLocalTime() + " " + message;
            Console.WriteLine(messageLocal);

            if (!allChannels)
                return;

            SendMessageSlack(messageLocal);
        }

        private void SendMessageSlack(string message)
        {
            //Process resultadoExecucao = Process.Start(ConfigurationManager.AppSettings.Get("pathArchivePython"), message);
            //Process.Start("cmd", ConfigurationManager.AppSettings.Get("pathArchivePython"));

            string YourApplicationPath = ConfigurationManager.AppSettings.Get("pathArchivePython");
            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                WorkingDirectory = Path.GetDirectoryName(YourApplicationPath),
                Arguments = "/c START python " + Path.GetFileName(YourApplicationPath) + " " + "\"" + message + "\"",
                UseShellExecute = false
            };
            
            Process.Start(processInfo);
        }



    }

}
