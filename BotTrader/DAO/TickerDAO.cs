using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using BotTrader.Model.Ticker;
using BotTrader.Service;
using Newtonsoft.Json;

namespace BotTrader.DAO
{

    public class TickerDAO
    {
        string script;
        DAO dao;
        SqlParameter[] arrayParametros;
        SqlDataReader dataReader;

        public TickerDAO()
        {
            dao = new DAO();
        }

        public void Inserir(Ticker ticker)
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

        public List<Data> Consultar(DadosConsultaTickerBD dadosConsultaTickerBD)
        {
            try
            {
                if (dadosConsultaTickerBD.QtdRegistros > 0)
                {
                    script = string.Format(@"
                    SELECT TOP {0}
	                    high,
	                    low,
	                    volume,
	                    trades_quantity,
	                    last,
	                    buy,
	                    sell,
	                    date 
                    FROM dbo.tab_bitcoin_trade_ticker", dadosConsultaTickerBD.QtdRegistros);
                }
                else
                {
                    script = @"
                    SELECT
	                    high,
	                    low,
	                    volume,
	                    trades_quantity,
	                    last,
	                    buy,
	                    sell,
	                    date 
                    FROM dbo.tab_bitcoin_trade_ticker";
                }

                if (!string.IsNullOrEmpty(dadosConsultaTickerBD.NomeCampoOrdenacao))
                {
                    if (!string.IsNullOrEmpty(dadosConsultaTickerBD.NomeCampoOrdenacao) && !string.IsNullOrEmpty(dadosConsultaTickerBD.TipoOrdenacao))
                    {
                        script += string.Format(" ORDER BY {0} {1}", dadosConsultaTickerBD.NomeCampoOrdenacao, dadosConsultaTickerBD.TipoOrdenacao);
                    }
                    else
                    {
                        script += string.Format(" ORDER BY {0}", dadosConsultaTickerBD.NomeCampoOrdenacao);
                    }
                }

                dataReader = dao.Consultar(script, arrayParametros);

                var r = new Serializacao().Serializar(dataReader);
                string json = JsonConvert.SerializeObject(r, Formatting.None);

                Data ticker = JsonConvert.DeserializeObject<List<Data>>(json).First();

                List<Data> listaTicker = JsonConvert.DeserializeObject<List<Data>>(json);

                return listaTicker;
            }
            finally
            {
                dataReader.Close();
            }
            
        }
    }
}
