using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using BotTrader.Service;

namespace BotTrader.DAO
{
    internal class LogDAO
    {
        SqlConnection SqlConn { get; set; }
        SqlCommand SqlComm { get; set; }
        string script;

        internal LogDAO()
        {
            SqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["BotTrader"].ToString());
        }
        
        internal void Inserir(Exception ex)
        {
            try
            {
                script = @"INSERT INTO dbo.tab_log
                                (
                                    stack_trace,
                                    message
                                )
                                VALUES
                                (
                                    @stack_trace,
                                    @message
                                )";

                SqlParameter[] arrayParametros = new SqlParameter[] { };

                if (ex.StackTrace == null)
                {
                    arrayParametros[0] = new SqlParameter("@stack_trace", DBNull.Value);
                }
                else
                {
                    arrayParametros[0] = new SqlParameter("@stack_trace", ex.StackTrace);
                }

                arrayParametros[1] = new SqlParameter("@message", ex.Message);

                if (SqlConn.State == ConnectionState.Closed)
                    SqlConn.Open();

                SqlComm = new SqlCommand
                {
                    CommandTimeout = Convert.ToInt32(
                        TimeSpan.FromMinutes(
                            Convert.ToInt16(
                                ConfigurationManager.AppSettings.Get("MinutosTimeoutProcessoBD")
                                )).TotalMilliseconds),
                    Connection = SqlConn,
                    CommandText = script,
                    CommandType = CommandType.Text
                };

                if (arrayParametros != null)
                {
                    SqlComm.Parameters.Clear();
                    SqlComm.Parameters.AddRange(arrayParametros);
                }

                SqlComm.ExecuteNonQuery();
            }
            catch (Exception exLog)
            {
                Comunicacao.EscreverNaTela(exLog.Message);
            }
        }

        ~LogDAO()
        {
            if (SqlConn != null)
            {
                SqlConn.Dispose();
            }

            if (SqlComm != null)
            {
                SqlComm.Dispose();
            }
        }

    }
}
