using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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

        public void Inserir(Trades listaTrades)
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

        public string ConsultarTipoUltimaNegociacao()
        {
            try
            {
                script = @"
                    IF((SELECT MAX(dat_registro) FROM dbo.tab_trade_compra) > (SELECT MAX(dat_registro) FROM dbo.tab_trade_venda))
                    BEGIN
	                    SELECT 'buy' AS tipoUltimaNegociacao
                    END
                    ELSE
                    BEGIN
	                    SELECT 'sell' AS tipoUltimaNegociacao
                    END";

                dataReader = dao.Consultar(script);

                var r = new Serializacao().Serializar(dataReader);
                string json = JsonConvert.SerializeObject(r, Formatting.None);

                DadosTipoUltimaNegociacao tipoUltimaNegociacao = JsonConvert.DeserializeObject<List<DadosTipoUltimaNegociacao>>(json).First();

                return tipoUltimaNegociacao.TipoUltimaNegociacao;
            }
            finally
            {
                dataReader.Close();
            }
        }

        /// <summary>
        /// Consulta os trades de acordo com os filtros enviados e retorna uma lista de trades
        /// </summary>
        /// <param name="dadosConsultaTradeBD">Filtros para consulta de trades</param>
        public List<Trade> Consultar(DadosConsultaTradeBD dadosConsultaTradeBD)
        {
            try
            {
                script = @"
                SELECT
                    [type],
                    amount,
                    unit_price,
                    active_order_code,
                    passive_order_code,
                    [date]
                FROM dbo.tab_bitcoin_trade_trade
                WHERE 1 = 1";

                if (dadosConsultaTradeBD.Tipo != null)
                {
                    script += " AND [type] = @type";
                    arrayParametros = new SqlParameter[]
                    {
                        new SqlParameter("@type", dadosConsultaTradeBD.Tipo),
                        new SqlParameter("@data_inicial", dadosConsultaTradeBD.DataInicial),
                        new SqlParameter("@data_final", dadosConsultaTradeBD.DataFinal)
                    };
                }
                else
                {
                    arrayParametros = new SqlParameter[]
                    {
                        new SqlParameter("@data_inicial", dadosConsultaTradeBD.DataInicial),
                        new SqlParameter("@data_final", dadosConsultaTradeBD.DataFinal)
                    };
                }

                script += " AND [date] BETWEEN @data_inicial AND @data_final";

                if (!string.IsNullOrEmpty(dadosConsultaTradeBD.NomeCampoOrdenacao))
                {
                    if (!string.IsNullOrEmpty(dadosConsultaTradeBD.NomeCampoOrdenacao) && !string.IsNullOrEmpty(dadosConsultaTradeBD.TipoOrdenacao))
                    {
                        script += string.Format(" ORDER BY {0} {1}", dadosConsultaTradeBD.NomeCampoOrdenacao, dadosConsultaTradeBD.TipoOrdenacao);
                    }
                    else
                    {
                        script += string.Format(" ORDER BY {0}", dadosConsultaTradeBD.NomeCampoOrdenacao);
                    }
                }

                dataReader = dao.Consultar(script, arrayParametros);

                if (!dataReader.HasRows)
                    return null;

                var r = new Serializacao().Serializar(dataReader);
                string json = JsonConvert.SerializeObject(r, Formatting.None);

                List<Trade> listaTrade = JsonConvert.DeserializeObject<List<Trade>>(json);

                return listaTrade;
            }
            finally
            {
                dataReader.Close();
            }

        }

        public string ConsultarUltimaDataProcessamento()
        {
            try
            {
                script = @"SELECT COALESCE(FORMAT(MAX(date), 'yyyy-MM-ddTHH:mm:ss-00:00'), '2017-01-01T00:00:00-00:00') as data FROM dbo.tab_bitcoin_trade_trade;";

                dataReader = dao.Consultar(script);

                var r = new Serializacao().Serializar(dataReader);
                string json = JsonConvert.SerializeObject(r, Formatting.None);

                DadosDataUltimoProcessamento maiorData = JsonConvert.DeserializeObject<List<DadosDataUltimoProcessamento>>(json).First();

                return maiorData.data;
            }
            finally
            {
                dataReader.Close();
            }

        }

        public DadosUltimoValorVenda ConsultarUltimoValorVenda()
        {
            try
            {
                script = @"
                SELECT TOP 1
	                vlr_venda
                FROM dbo.tab_trade_venda
                ORDER BY
	                dat_registro DESC";

                dataReader = dao.Consultar(script);

                if (!dataReader.HasRows)
                {
                    Comunicacao.EnviarMensagem("a tabela dbo.tab_trade_venda não possui histórico de negociação.");
                    return null;
                }

                var r = new Serializacao().Serializar(dataReader);
                string json = JsonConvert.SerializeObject(r, Formatting.None);

                return JsonConvert.DeserializeObject<List<DadosUltimoValorVenda>>(json).First();
            }
            finally
            {
                dataReader.Close();
            }

        }

        public DadosUltimoValorCompra ConsultarUltimoValorCompra()
        {
            try
            {
                script = @"
                SELECT TOP 1
	                vlr_compra
                FROM dbo.tab_trade_compra
                ORDER BY
	                dat_registro DESC";

                dataReader = dao.Consultar(script);

                if (!dataReader.HasRows)
                {
                    Comunicacao.EnviarMensagem("a tabela dbo.tab_trade_compra não possui histórico de negociação.");
                    return null;
                }

                var r = new Serializacao().Serializar(dataReader);
                string json = JsonConvert.SerializeObject(r, Formatting.None);

                return JsonConvert.DeserializeObject<List<DadosUltimoValorCompra>>(json).First();
            }
            finally
            {
                dataReader.Close();
            }

        }

    }
}
