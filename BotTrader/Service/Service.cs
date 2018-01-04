using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotTrader.Model;

namespace BotTrader.Service
{
    internal class Service
    {
        public ResultRequestBitCoinTrade GetTicker()
        {
            return null;
        }

        /// <summary>
        /// Compara o valor de compra e de venda com o último valor de compra e envia uma mensagem caso a variação seja maior que X
        /// </summary>
        /// <param name="resultRequestBitCoinTrade">Dados de valores atuais de trade</param>
        /// <param name="resultGetLastPurchaseValue">Valor da última compra</param>
        internal void Think(ResultRequestBitCoinTrade resultRequestBitCoinTrade, ResultGetLastPurchaseValue resultGetLastPurchaseValue)
        {
            decimal valueCompareVariation = Convert.ToDecimal(ConfigurationManager.AppSettings.Get("valueVariation"));
            decimal valueBuyVariation = CalculateVariance(resultRequestBitCoinTrade.data.buy, resultGetLastPurchaseValue.value);
            decimal valueSellVariation = CalculateVariance(resultRequestBitCoinTrade.data.buy, resultGetLastPurchaseValue.value);
            string message;

            if (valueBuyVariation >= valueCompareVariation)
            {
                message = string.Format("O valor para compra está {0}% maior que o valor da última compra.", valueBuyVariation);
                SendMessage(message);
            }
            else if (valueBuyVariation <= -valueCompareVariation)
            {
                message = string.Format("O valor para compra está {0}% menor que o valor da última compra.", valueBuyVariation);
                SendMessage(message);
            }


            if (valueSellVariation >= valueCompareVariation)
            {
                message = string.Format("O valor para venda está {0}% maior que o valor da última compra.", valueSellVariation);
                SendMessage(message);
            }
            else if (valueSellVariation <= -valueCompareVariation)
            {
                message = string.Format("O valor para venda está {0}% menor que o valor da última compra.", valueSellVariation);
                SendMessage(message);
            }
        }

        private decimal CalculateVariance(decimal a, decimal b)
        {
            decimal difference = a - b;
            decimal variation = difference / a;
            return variation;
        }

        private void SendMessage(string message)
        {
            SendMessageSlack(message);
            Console.WriteLine(message);
        }

        private void SendMessageSlack(string message)
        {

        }

    }
}
