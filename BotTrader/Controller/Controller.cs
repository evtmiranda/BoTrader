using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotTrader.Model;

namespace BotTrader.Controller
{
    class Controller
    {
        public void Process()
        {

            Service.Service service = new Service.Service();
            DAO.DAO dao = new DAO.DAO();

            ResultRequestBitCoinTrade resultRequestBitCoinTrade = service.GetTicker();
            ResultGetLastPurchaseValue resultGetLastPurchaseValue = dao.GetLastPurchaseValue();

            service.Think(resultRequestBitCoinTrade, resultGetLastPurchaseValue);

        }
    }
}
