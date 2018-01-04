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

        internal decimal GetLastTradeValue(Trade trade)
        {
            decimal resultGetLastTradeValue = 0;

            string script = null;

            switch (trade)
            {
                case Trade.Buy:
                    script = @"
                        SELECT TOP (1)
                            vlr_buy
                        FROM dbo.tab_trade_buy
                        ORDER BY dat_registro DESC";
                    break;
                case Trade.Sale:
                    script = @"
                        SELECT TOP (1)
                            vlr_sale
                        FROM dbo.tab_trade_sale
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
                resultGetLastTradeValue = Convert.ToDecimal(SqlComm.ExecuteScalar());

                return resultGetLastTradeValue;
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
                                    message,
                                )";

                SqlConn.Open();

                SqlComm.CommandType = CommandType.Text;
                SqlComm.CommandText = script;

                SqlComm.Parameters.Clear();
                SqlComm.Parameters.AddWithValue("@stack_trace", ex.StackTrace);
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
