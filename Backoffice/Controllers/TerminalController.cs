using Backoffice.Models;
using Backoffice.Tools.DAL.Tools;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace Backoffice.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TerminalController : Controller
    {
       

        [HttpPost("finalizar")]
        public IActionResult FinalizarTerminal([FromBody] Terminal terminalRequest)
        {

            
            bool sesion=false;
            try
            {
                
                
                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@IdTerminal", terminalRequest.id),
             new SqlParameter("@IdCasino", terminalRequest.idcasino),
              new SqlParameter("@Sesion", sesion),
                };

                SqlHelper.ExecuteNonQuery("UPDATE [dbo].[Terminales] SET sesion = @Sesion WHERE id = @IdTerminal AND id_casino = @IdCasino", System.Data.CommandType.Text, parameters);


               
                return Ok();
            }
            catch (Exception ex)
            {
                   return BadRequest("Error al finalizar el juego: " + ex.Message);
            }
        }


        [HttpPost("iniciar")]
        public IActionResult IniciarTerminal([FromBody] Terminal terminalRequest)
        {
            bool licencia;
            bool sesion;

            try
            {
                using (var db = SqlHelper.ExecuteReader("SELECT licencia, sesion FROM [dbo].[Terminales] WHERE id = @IdTerminal AND id_casino = @IdCasino", System.Data.CommandType.Text,
                    new Microsoft.Data.SqlClient.SqlParameter[]
                    {
                new Microsoft.Data.SqlClient.SqlParameter("@IdTerminal", terminalRequest.id),
                new Microsoft.Data.SqlClient.SqlParameter("@IdCasino", terminalRequest.idcasino)
                    }))
                {
                    if (db.Read())
                    {
                        object[] x = new object[db.FieldCount];
                        db.GetValues(x);

                        licencia = db.GetBoolean(0);
                        sesion = db.GetBoolean(1);


                        if (licencia == true)
                        {

                            if (sesion==false)
                            {
                                sesion = true;

                                SqlParameter[] parameters = new SqlParameter[]
                                {
                        new SqlParameter("@IdTerminal", terminalRequest.id),
                        new SqlParameter("@IdCasino", terminalRequest.idcasino),
                        new SqlParameter("@Sesion", sesion),
                                };

                                SqlHelper.ExecuteNonQuery("UPDATE [dbo].[Terminales] SET sesion = @Sesion WHERE id = @IdTerminal AND id_casino = @IdCasino", System.Data.CommandType.Text, parameters);

                                return Ok();
                            }
                            else
                            {
                                return BadRequest("La terminal ya tiene la sesion activa.");
                            }
                        }
                        else
                        {
                            return BadRequest("La terminal no tiene licencia activa. Comuniquese con su proveedor.");
                            
                        }

                    }
                    else
                    {
                        return BadRequest("No se encontró una terminal con los datos proporcionados.");
                    }
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest("Error al iniciar la terminal: " + ex.Message);
            }
        }



        [HttpPost("activar")]
        public IActionResult AltaTerminal([FromBody] Terminal terminalRequest)
        {

           
            bool licencia = true;
            try
            {


                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@IdTerminal", terminalRequest.id),
             new SqlParameter("@IdCasino", terminalRequest.idcasino),
              new SqlParameter("@Licencia", licencia),
                };

                SqlHelper.ExecuteNonQuery("UPDATE [dbo].[Terminales] SET licencia = @Licencia WHERE id = @IdTerminal AND id_casino = @IdCasino", System.Data.CommandType.Text, parameters);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Error al activar la licencia: " + ex.Message);
            }
        }

        [HttpPost("desactivar")]
        public IActionResult BajaTerminal([FromBody] Terminal terminalRequest)
        {


            bool licencia = false;
            try
            {


                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@IdTerminal", terminalRequest.id),
             new SqlParameter("@IdCasino", terminalRequest.idcasino),
              new SqlParameter("@Licencia", licencia),
                };

                SqlHelper.ExecuteNonQuery("UPDATE [dbo].[Terminales] SET licencia = @Licencia WHERE id = @IdTerminal AND id_casino = @IdCasino", System.Data.CommandType.Text, parameters);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Error al desactivar la licencia: " + ex.Message);
            }
        }



    }
}
    
                        