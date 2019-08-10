using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebServiceEmpresa.Controllers
{
    public class BdController : Controller
    {
        // GET: Bd
        OracleConnection connection = null;
        OracleCommand command = null;
        OracleDataReader reader = null;

        private string oradb = "Data Source=localhost:1521/xe;User Id=ws_empresa;Password=ws_empresa";

        public DataTable ExecuteQuery(string query)
        {
            connection = new OracleConnection(oradb);
            try
            {
                connection.Open();
                command = new OracleCommand(query, connection);
                command.CommandType = CommandType.Text;
                OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(command);
                DataTable dt = new DataTable();
                oracleDataAdapter.Fill(dt);
                command.Dispose();
                oracleDataAdapter.Dispose();
                return dt;
            }
            catch (Exception)
            {

                return new DataTable();
            }
            finally
            {
                connection.Close();
            }
        }


        public int ExecSP(string SP, params object[] parametros)
        {
            try
            {
                connection = new OracleConnection(oradb);
                command = new OracleCommand(SP, connection);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                OracleCommandBuilder.DeriveParameters(command);
                int cuenta = 0;
                bool parametroRetorno = false;
                int retorno = 0;

                foreach (OracleParameter pr in command.Parameters)
                {
                    if (pr.ParameterName != "P_RETORNO")
                    {
                        pr.Value = parametros[cuenta];
                        cuenta++;
                    }
                    else
                    {
                        pr.ParameterName.Equals("P_RETORNO");
                        pr.Direction = ParameterDirection.Output;
                        parametroRetorno = true;
                    }
                }
                command.ExecuteNonQuery();
                if (parametroRetorno)
                {
                    retorno = Convert.ToInt32(command.Parameters["P_RETORNO"].Value);
                }
                connection.Close();
                command.Dispose();
                return retorno;
            }
            catch (Exception)
            {

                return 0;
            }
        }
    }
}