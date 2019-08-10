using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebServiceEmpresa.Models;

namespace WebServiceEmpresa.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NegocioController : ApiController
    {
        BdController bd = new BdController();

        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/negocio/ValidarEmpleado/{rut}")]
        public Convenio ValidarEmpleado(int rut) {
            try
            {
                Convenio convenio = null;
                string query = "SELECT * FROM EMPLEADO emp JOIN CONVENIO con on emp.EMPLEADO_ID = con.EMPLEADO where con.VIGENTE = 1 AND emp.RUT =" + rut;
                DataTable dt = new DataTable();
                dt = bd.ExecuteQuery(query);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        convenio = new Convenio {
                            ConvenioId = int.Parse(item["CONVENIO_ID"].ToString()),
                            Saldo = int.Parse(item["SALDO_DISPONIBLE"].ToString()),
                            Vigente = int.Parse(item["VIGENTE"].ToString()),
                            Empleado = int.Parse(item["EMPLEADO"].ToString())
                        };
                    }
                    return convenio;
                }
                return null;
                
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/negocio/TransaccionRealizada/{rutempleado}/{montotransaccion}")]
        public int InsertarTransaccion(int rutempleado, int montotransaccion)
        {
                object[] parametros = new object[2];
                int ejecutar = 0;
                parametros[0] = rutempleado.ToString();
                parametros[1] = montotransaccion.ToString();
                ejecutar = bd.ExecSP("PROCEDURE_TRANSACCION", parametros);
                return ejecutar;
        }
    }
}
