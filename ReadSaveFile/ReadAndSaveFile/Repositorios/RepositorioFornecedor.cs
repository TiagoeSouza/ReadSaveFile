using ReadAndSaveFile.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ReadAndSaveFile
{
    public class RepositorioFornecedor
    {
        public static bool insert(List<Fornecedor> fornecedores)
        {
            try
            {
                foreach (var forn in fornecedores)
                {
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StrConnection"].ConnectionString))
                    {
                        try
                        {

                            string sql = "INSERT INTO FORNECEDOR(NOME) VALUES (@NOME)";

                            SqlCommand command = new SqlCommand(sql, connection);

                            command.Parameters.Add("@NOME", SqlDbType.VarChar);
                            command.Parameters["@NOME"].Value = forn.NOME;

                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;

        }


        public static List<Fornecedor> Get()
        {
            List<Fornecedor> fornecedores = new List<Fornecedor>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StrConnection"].ConnectionString))
            {
                string sql = "Select * FROM Fornecedor";
                SqlCommand command = new SqlCommand(sql, connection);


                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            Fornecedor forn = new Fornecedor();
                            forn.ID = (int)reader["FORNECEDOR_ID"];
                            forn.NOME = reader["NOME"].ToString();

                            fornecedores.Add(forn);
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
                return fornecedores;
            }
        }
    }
}