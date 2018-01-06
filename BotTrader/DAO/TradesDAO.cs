using System.Data.SqlClient;
using BotTrader.Model.Trades;

namespace BotTrader.DAO
{
    class TradesDAO
    {
        string script;
        DAO dao;
        SqlParameter[] arrayParametros;

        public TradesDAO()
        {
            dao = new DAO();
        }

        internal void Inserir(Trades listaTrades)
        {
            script = @"
                INSERT INTO dbo.tab_bitcoin_trade_trade
                (
                    type,
                    amount,
                    unit_price,
                    active_order_code,
                    passive_order_code,
                    date
                )
                VALUES
                (   @type,       -- type - varchar(100)
                    @amount,     -- amount - decimal(10, 2)
                    @unit_price,     -- unit_price - decimal(10, 2)
                    @active_order_code,       -- active_order_code - varchar(100)
                    @passive_order_code,       -- passive_order_code - varchar(100)
                    @date -- date - datetime
                )";

            for (int i = listaTrades.data.trades.Count - 1; i >= 0; i--)
            {
                arrayParametros = new SqlParameter[]
                {
                                new SqlParameter("@type", listaTrades.data.trades[i].type),
                                new SqlParameter("@amount", listaTrades.data.trades[i].amount),
                                new SqlParameter("@unit_price", listaTrades.data.trades[i].unit_price),
                                new SqlParameter("@active_order_code", listaTrades.data.trades[i].active_order_code),
                                new SqlParameter("@passive_order_code", listaTrades.data.trades[i].passive_order_code),
                                new SqlParameter("@date", listaTrades.data.trades[i].date)
                };

                dao.Inserir(script, arrayParametros);
            }

        }
    }
}
