using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BotTrader.Model.Trades;
using BotTrader.Service;
using Newtonsoft.Json;

namespace BotTrader.DAO
{
    class TradesDAO
    {
        string script;
        DAO dao;
        SqlParameter[] arrayParametros;
        SqlDataReader dataReader;

        public TradesDAO()
        {
            dao = new DAO();
        }

        internal void Inserir(Trades listaTrades)
        {
            script = @"
                IF(SELECT COUNT(*) FROM dbo.tab_bitcoin_trade_trade WHERE active_order_code = @active_order_code AND passive_order_code = @passive_order_code AND date = @date) = 0
                BEGIN
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
                    )
                END";

            for (int i = listaTrades.data.trades.Count - 1; i >= 0; i--)
            {
                arrayParametros = new SqlParameter[]
                {
                    new SqlParameter("@type", listaTrades.data.trades[i].type),
                    new SqlParameter("@amount", listaTrades.data.trades[i].amount),
                    new SqlParameter("@unit_price", listaTrades.data.trades[i].unit_price),
                    new SqlParameter("@active_order_code", listaTrades.data.trades[i].active_order_code),
                    new SqlParameter("@passive_order_code", listaTrades.data.trades[i].passive_order_code),
                    new SqlParameter("@date", listaTrades.data.trades[i].date.AddHours(-2))
                };

                dao.Inserir(script, arrayParametros);
            }

        }

        internal string ConsultarUltimaDataProcessamento()
        {
            script = @"SELECT COALESCE(FORMAT(MAX(date), 'yyyy-MM-ddThh:mm:ss-03:00'), '2017-01-01T00:00:00-03:00') as data FROM dbo.tab_bitcoin_trade_trade;";

            dataReader = dao.Consultar(script);

            var r = new Serializacao().Serializar(dataReader);
            string json = JsonConvert.SerializeObject(r, Formatting.None).Replace("[","").Replace("]","");

            DataUltimoProcessamento maiorData = JsonConvert.DeserializeObject<DataUltimoProcessamento>(json);

            dataReader.Close();

            return maiorData.data;
        }
    }
}
