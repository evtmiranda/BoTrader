using System;
using System.Threading;
using BotTrader.Model;

namespace BotTrader.Controller
{
    class Controller
    {
        public void Process()
        {
            while (true)
            {
                Service.Service service = new Service.Service();
                DAO.DAO dao = new DAO.DAO();

                ResultGetLastBuyValue resultGetLastBuyValue = new ResultGetLastBuyValue
                {
                    value = dao.GetLastTradeValue(Trade.Buy)
                };

                ResultGetLastSaleValue resultGetLastSaleValue = new ResultGetLastSaleValue
                {
                    value = dao.GetLastTradeValue(Trade.Sale)
                };

                ResultRequestBitCoinTrade resultRequestBitCoinTrade = service.GetTicker();

                service.Think(resultRequestBitCoinTrade, resultGetLastBuyValue, resultGetLastSaleValue);

                Console.WriteLine(DateTime.UtcNow.ToLocalTime() + " fim \n\n");

                Thread.Sleep(Convert.ToInt32(TimeSpan.FromSeconds(10).TotalMilliseconds));
            }
        }
    }
}
