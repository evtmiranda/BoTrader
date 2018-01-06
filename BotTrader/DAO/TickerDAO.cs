using System.Data.SqlClient;
using BotTrader.Model.Ticker;

namespace BotTrader.DAO
{

    internal class TickerDAO
    {
        string script;
        DAO dao;
        SqlParameter[] arrayParametros;

        public TickerDAO()
        {
            dao = new DAO();
        }

        internal void Inserir(Ticker ticker)
        {
            script = @"
                INSERT INTO dbo.tab_bitcoin_trade_ticker
                (
                    high,
                    low,
                    volume,
                    trades_quantity,
                    last,
                    buy,
                    sell
                )
                VALUES
                (   @high,     -- high - decimal(10, 2)
                    @low,     -- low - decimal(10, 2)
                    @volume,     -- volume - decimal(10, 2)
                    @trades_quantity,        -- trades_quantity - int
                    @last,     -- last - decimal(10, 2)
                    @buy,     -- buy - decimal(10, 2)
                    @sell     -- sell - decimal(10, 2)
                )";

            arrayParametros = new SqlParameter[]
            {
                new SqlParameter("@high", ticker.data.high),
                new SqlParameter("@low", ticker.data.low),
                new SqlParameter("@volume", ticker.data.volume),
                new SqlParameter("@trades_quantity", ticker.data.trades_quantity),
                new SqlParameter("@last", ticker.data.last),
                new SqlParameter("@buy", ticker.data.buy),
                new SqlParameter("@sell", ticker.data.sell)
            };

            dao.Inserir(script, arrayParametros);
        }
    }
}
