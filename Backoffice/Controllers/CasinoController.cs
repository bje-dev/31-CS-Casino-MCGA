using Backoffice.Models;
using Backoffice.Tools.DAL.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Backoffice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CasinoController : ControllerBase
    {
        [HttpPost]
        [Route("apuestaminmax")]
        public IActionResult ConfigApuestaMinMax([FromBody] Casino casinoRequest)
        {

            try
            {


                decimal minApuesta = 0;
                decimal maxApuesta = 0;

                using (var db = SqlHelper.ExecuteReader("SELECT id_casino, apuesta_min, apuesta_max FROM [dbo].[Apuestasmaxmin] WHERE id_casino = @IdCasino", System.Data.CommandType.Text,
                    new Microsoft.Data.SqlClient.SqlParameter[]
                      {
                    new Microsoft.Data.SqlClient.SqlParameter("@IdCasino", casinoRequest.idcasino)
                      })
                  )

                {

                    if (db.Read())
                    {

                        minApuesta = Convert.ToDecimal(db["apuesta_min"]);
                        maxApuesta = Convert.ToDecimal(db["apuesta_max"]);

                        return Ok(new { minapuesta = minApuesta, maxapuesta = maxApuesta });
                    }
                    else
                    {

                        return NotFound();
                    }


                }

            }
            catch (Exception ex)
            {

                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }


        [HttpPost("premiominmax")]
        public IActionResult ConfigPremioMinMax([FromBody] Casino casinoRequest)
        {
            decimal minPremio = 0;
            decimal maxPremio = 0;


            try
            {
                using (SqlDataReader reader = SqlHelper.ExecuteReader("SELECT premio_min, premio_max FROM [dbo].[Premiosmaxmin] WHERE id_casino = @IdCasino", System.Data.CommandType.Text,
             new Microsoft.Data.SqlClient.SqlParameter[]
               {
                    new Microsoft.Data.SqlClient.SqlParameter("@IdCasino", casinoRequest.idcasino)
               }))

                {

                    if (reader.Read())
                    {

                        minPremio = Convert.ToDecimal(reader["premio_min"]);
                        maxPremio = Convert.ToDecimal(reader["premio_max"]);

                        return Ok(new { minpremio = minPremio, maxpremio = maxPremio });
                    }
                    else
                    {
                        return NotFound();
                    }
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }

        }


    }
}
