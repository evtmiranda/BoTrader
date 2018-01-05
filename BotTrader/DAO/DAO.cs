using System;
using System.Data;
using System.Data.SqlClient;
using BotTrader.Model;
using System.Configuration;

namespace BotTrader.DAO
{
    internal class DAO
    {
        SqlConnection SqlConn { get; set; }
        SqlCommand SqlComm { get; set; }

        internal DAO()
        {
            SqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["BotTrader"].ToString());
        }

        internal double ConsultarUltimoValorTrade(TipoTrade tipoTrade)
        {
            double resultUltimoValorTrade = 0;

            string script = null;

            switch (tipoTrade)
            {
                case TipoTrade.Compra:
                    script = @"
                        SELECT TOP (1)
                            vlr_compra
                        FROM dbo.tab_trade_compra
                        ORDER BY dat_registro DESC";
                    break;
                case TipoTrade.Venda:
                    script = @"
                        SELECT TOP (1)
                            vlr_venda
                        FROM dbo.tab_trade_venda
                        ORDER BY dat_registro DESC";
                    break;
            }

            SqlConn.Open();

            SqlComm = new SqlCommand(script, SqlConn)
            {
                CommandType = CommandType.Text,
                CommandText = script
            };

            try
            {
                resultUltimoValorTrade = Convert.ToDouble(SqlComm.ExecuteScalar());

                return resultUltimoValorTrade;
            }
            catch (Exception ex)
            {
                InserirLog(ex);
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                if (SqlConn != null)
                {
                    if (SqlConn.State == ConnectionState.Open)
                    {
                        SqlConn.Close();
                    }
                }

                if (SqlComm != null)
                {
                    SqlComm.Dispose();
                }
            }


        }

        internal void InserirLog(Exception ex)
        {
            try
            {
                string script = @"INSERT INTO dbo.tab_log
                                (
                                    stack_trace,
                                    message
                                )
                                VALUES
                                (
                                    @stack_trace,
                                    @message
                                )";

                if(SqlConn.State == ConnectionState.Closed)
                {
                    SqlConn.Open();
                }

                SqlComm.CommandType = CommandType.Text;
                SqlComm.CommandText = script;

                SqlComm.Parameters.Clear();

                if(ex.StackTrace == null)
                {
                    SqlComm.Parameters.AddWithValue("@stack_trace", DBNull.Value);
                }
                else
                {
                    SqlComm.Parameters.AddWithValue("@stack_trace", ex.StackTrace);
                }
                
                
                SqlComm.Parameters.AddWithValue("@message", ex.Message);

                SqlComm.ExecuteNonQuery();
            }
            finally
            {
                if (SqlConn != null)
                {
                    if (SqlConn.State == ConnectionState.Open)
                    {
                        SqlConn.Close();
                        SqlConn.Dispose();
                    }
                }

                if (SqlComm != null)
                {
                    SqlComm.Dispose();
                }
            }

        }

    }
}
