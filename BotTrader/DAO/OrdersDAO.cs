using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotTrader.Model.Orders;

namespace BotTrader.DAO
{
    class OrdersDAO
    {
        string script;
        DAO dao;
        SqlParameter[] arrayParametros;

        public OrdersDAO()
        {
            dao = new DAO();
        }

        internal void Inserir(Orders listaOrders)
        {
            script = @"
                IF(SELECT COUNT(*) FROM dbo.tab_bitcoin_trade_order_ask WHERE code = @code) = 0
                BEGIN

                    INSERT INTO dbo.tab_bitcoin_trade_order_ask
                    (
                        unit_price,
                        code,
                        amount
                    )
                    VALUES
                    (   @unit_price, -- unit_price - decimal(10, 2)
                        @code,   -- code - varchar(100)
                        @amount  -- amount - decimal(10, 2)
                        )

                END";

            //Percorre a lista do último registro para o primeiro
            for (int i = listaOrders.data.asks.Count-1; i >= 0; i--)
            {
                arrayParametros = new SqlParameter[]
                {
                    new SqlParameter("@unit_price", listaOrders.data.asks[i].unit_price),
                    new SqlParameter("@code", listaOrders.data.asks[i].code),
                    new SqlParameter("@amount", listaOrders.data.asks[i].amount)
                };

                dao.Inserir(script, arrayParametros);
            }

            script = @"
                IF(SELECT COUNT(*) FROM dbo.tab_bitcoin_trade_order_bid WHERE code = @code) = 0
                BEGIN

                    INSERT INTO dbo.tab_bitcoin_trade_order_bid
                    (
                        unit_price,
                        code,
                        amount
                    )
                    VALUES
                    (   @unit_price, -- unit_price - decimal(10, 2)
                        @code,   -- code - varchar(100)
                        @amount  -- amount - decimal(10, 2)
                        )

                END";

            for (int i = listaOrders.data.bids.Count - 1; i >= 0; i--)
            {
                arrayParametros = new SqlParameter[]
                {
                                new SqlParameter("@unit_price", listaOrders.data.bids[i].unit_price),
                                new SqlParameter("@code", listaOrders.data.bids[i].code),
                                new SqlParameter("@amount", listaOrders.data.bids[i].amount)
                };

                dao.Inserir(script, arrayParametros);
            }

        }
    }
}
