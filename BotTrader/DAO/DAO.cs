﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BotTrader.Service;

namespace BotTrader.DAO
{
    public class DAO
    {
        protected SqlConnection SqlConn { get; set; }
        protected SqlCommand SqlComm { get; set; }
        protected LogDAO LogDAO { get; set; }

        public DAO()
        {
            SqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["BotTrader"].ToString());
            LogDAO = new LogDAO();

            SqlComm = new SqlCommand
            {
                CommandTimeout = Convert.ToInt32(
                    TimeSpan.FromMinutes(
                        Convert.ToInt16(
                            ConfigurationManager.AppSettings.Get("MinutosTimeoutProcessoBD")
                            )).TotalMilliseconds),
                Connection = SqlConn,
                CommandType = CommandType.Text
            };
        }

        public void Inserir(string script, SqlParameter[] arrayParametros = null)
        {
            try
            {
                if (SqlConn.State == ConnectionState.Closed)
                    SqlConn.Open();

                if (arrayParametros != null)
                {
                    SqlComm.Parameters.Clear();
                    SqlComm.Parameters.AddRange(arrayParametros);
                }

                SqlComm.CommandText = script;

                SqlComm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogDAO.Inserir(ex);
                Comunicacao.EscreverNaTela(ex.Message);
            }
        }

        public SqlDataReader Consultar(string script, SqlParameter[] arrayParametros = null)
        {
            try
            {
                if (SqlConn.State == ConnectionState.Closed)
                    SqlConn.Open();

                if (arrayParametros != null)
                {
                    SqlComm.Parameters.Clear();
                    SqlComm.Parameters.AddRange(arrayParametros);
                }

                SqlComm.CommandText = script;

                return SqlComm.ExecuteReader();
            }
            catch (Exception ex)
            {
                LogDAO.Inserir(ex);
                Comunicacao.EscreverNaTela(ex.Message);
                return null;
            }
        }

        ~DAO(){
            if(SqlConn != null)
            {
                SqlConn.Close();
            }

            if(SqlComm != null)
            {
                SqlComm.Dispose();
            }
        }

    }
}
