using ConsoleApp1.Dominio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Casino
    {
        private static string connectionStringBackoffice = "";
        private static string connectionStringBitacora = "";
        private int idCasino;
        private double _newMax = 0;
        private double _newMin = 0;
        private string jsonLog = "";
        private string fullPath = "";
        private string getData = "";
        private List<Terminal> listaNegra = new List<Terminal>();


        public Casino(int id, string backoffice, string bitacora)
        {
            connectionStringBackoffice = backoffice;
            connectionStringBitacora = bitacora;
            idCasino = id;
        }

        private string buildjsonLog(string op, int casino, string desc, bool resultado)
        {
            string _result = "";
            Logger log = new Logger();

            log.operacion = op;
            log.componente = "Servidor del casino";
            log.casino = casino;
            log.descripcion = desc;
            log.resultado = resultado;

            _result = JsonConvert.SerializeObject(log);

            return _result;
        }

        public int getId()
        {
            return idCasino;
        }
        private void actualizarMaxMin(Terminal t)
        {
            _newMax = t.maximo;
            _newMin = t.minimo;
            jsonLog = buildjsonLog("Actualiza maximo y minimo", idCasino, $"Nuevo maximo: {_newMax} y nuevo minimo: {_newMin}", false);
            try
            {
                serviceCaller.callService(connectionStringBitacora, "POST", jsonLog);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al conectarse escribir en bitacora");
            }
        }

        private void terminalOffline(Terminal t)
        {
            listaNegra.Add(t);
            jsonLog = buildjsonLog("Maquina bajada", idCasino, $"Maquina en la lista negra: {t.nroMaquina}", false);
            try
            {
                serviceCaller.callService(connectionStringBitacora, "POST", jsonLog);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al conectarse escribir en bitacora");
            }
        }
        private void actualizarTerminal(Terminal t)
        {
            t.maximo = _newMax;
            t.minimo = _newMin;

            foreach (Terminal terminal in listaNegra)
            {
                if (terminal.nroMaquina == t.nroMaquina)
                {
                    t.accion = "FINALIZADA";
                }
            }
        }
        private string jugada()
        {
            string _response = "";
            try
            {
                //Llamar al BACKOFFICE para saber si gana o no
                fullPath = string.Concat(connectionStringBackoffice, "Jugada/resultado");
                getData = serviceCaller.callService(fullPath, "GET", "");
                Console.WriteLine($"La jugada es: {getData}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al conectarse con backoffice");
            }
            try
            {
                bool resu = false;
                if (getData == "true") { resu = true; }
                jsonLog = buildjsonLog("Jugada", idCasino, $"La jugada es: {getData}",
                    resu);
                serviceCaller.callService(connectionStringBitacora, "POST", jsonLog);
                _response = getData;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al conectarse escribir en bitacora");
            }
            return _response;
        }
        private string sesionCerrada(Terminal t)
        {
            string _response = "";
            try
            {
                //Llamar al BACKOFFICE para avisarle del cierre de sesion
                fullPath = string.Concat(connectionStringBackoffice, "Terminal/finalizar");
                serviceCaller.callService(fullPath, "POST", JsonConvert.SerializeObject(new BackofficeLicenciaRequest { id = t.nroMaquina, idcasino = t.nroCasino }));

                //Y se loguea en bitacora
                jsonLog = buildjsonLog("Fin de Sesion", idCasino, $"Terminal finalizada: {t.nroMaquina}", true);
                serviceCaller.callService(connectionStringBitacora, "POST", jsonLog);
                _response = "true";
            }
            catch
            {
                _response = "true";
            }
            return _response;
        }
        private string login(Terminal t)
        {
            string _response = "";

            try
            {
                if (t.nroCasino == idCasino)
                {
                    //Llamar al BACKOFFICE para saber si puede registrar o no la maquina
                    fullPath = string.Concat(connectionStringBackoffice, "Terminal/iniciar");
                    getData = serviceCaller.callService(fullPath, "POST", JsonConvert.SerializeObject(new BackofficeLicenciaRequest { id = t.nroMaquina, idcasino = t.nroCasino }));
                    if ((getData == "true") || (getData == ""))
                    {
                        jsonLog = buildjsonLog("Inicio de Sesion", idCasino, $"Terminal Iniciada: {t.nroMaquina}", true);
                        serviceCaller.callService(connectionStringBitacora, "POST", jsonLog);
                        _response = "true";
                    }
                    else
                    {
                        _response = "false";
                    }

                    //Y para saber cuales son los maximos y minimos por defecto
                    fullPath = string.Concat(connectionStringBackoffice, "Casino/apuestaminmax");
                    getData = serviceCaller.callService(fullPath, "POST", JsonConvert.SerializeObject(new BackofficeMaxMinRequest { idcasino = t.nroCasino }));
                    BackofficeMaxMinResponse backofficeMaxMin = JsonConvert.DeserializeObject<BackofficeMaxMinResponse>(getData);
                    _newMax = backofficeMaxMin.maxapuesta;
                    _newMin = backofficeMaxMin.minapuesta;


                    ////mientras tanto, maximos y minimos hardcodeados
                    //_newMax = 10000000;
                    //_newMin = 0;

                    ////mientras tanto, siempre puede registrarla
                    //_response = "true";
                }
                else
                {
                    _newMax = 0;
                    _newMin = 0;
                    _response = "false";
                }
            }
            catch
            {
                _response = "false";
            }

            return _response;
        }
        private string retirarGanancias(Terminal t)
        {
            string _response = "";

            //Avisarle al backoffice de la apuesta y que devuelva el factor multiplicante
            fullPath = string.Concat(connectionStringBackoffice, "Operacion/pago");
            var res = serviceCaller.callService(fullPath, "POST", JsonConvert.SerializeObject(new BackofficeApuestaRequest
            {
                idcasino = t.nroCasino,
                idterminal = t.nroMaquina,
                monto = t.apuesta
            }));

            jsonLog = buildjsonLog("Retirar Dinero", idCasino, $"Se retira el monto {t.apuesta} de la terminal {t.nroMaquina}", true);
            try
            {
                serviceCaller.callService(connectionStringBitacora, "POST", jsonLog);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al conectarse escribir en bitacora");
            }

            return _response;
        }
        private string apuesta(Terminal t)
        {
            string _response = "";

            string apuesta;
            double apuestaInt;

            //Calcula cuanto gana o pierde en base a la jugada
            if (t.accion == "apuestaGanador")
            {

                //Avisarle al backoffice de la apuesta y que devuelva el factor multiplicante
                fullPath = string.Concat(connectionStringBackoffice, "Jugada/apuesta");
                var res = serviceCaller.callService(fullPath, "POST", JsonConvert.SerializeObject(new BackofficeApuestaRequest
                {
                    idcasino = t.nroCasino,
                    idterminal = t.nroMaquina,
                    monto = t.apuesta
                }));

                var jugadaApuesta = JsonConvert.DeserializeObject<BackofficeApuestaResponse>(res);

                apuestaInt = t.apuesta;
                apuestaInt = apuestaInt * jugadaApuesta.factor;
            }
            else
            {
                apuestaInt = 0;
            }

            //Devolver resultados
            _response = apuestaInt.ToString();

            return _response;
        }

        private string informarDeposito(Terminal t)
        {
            string _response = "";
            try
            {
                //Avisarle al backoffice del deposito
                fullPath = string.Concat(connectionStringBackoffice, "Operacion/deposito");
                var res = serviceCaller.callService(fullPath, "POST", JsonConvert.SerializeObject(new BackofficeApuestaRequest
                {
                    idcasino = t.nroCasino,
                    idterminal = t.nroMaquina,
                    monto = t.apuesta
                }));
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            jsonLog = buildjsonLog("Deposito", idCasino, $"Se deposita el monto {t.apuesta} de la terminal {t.nroMaquina}", true);
            try
            {
                serviceCaller.callService(connectionStringBitacora, "POST", jsonLog);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al conectarse escribir en bitacora");
            }


            return _response;
        }

        public string procesarTerminal(Terminal t)
        {
            string _response = "";


            //Chequea que se esta haciendo
            if (t.accion == "deposito")
            {
                informarDeposito(t);
            }
            if (t.accion == "actualizarMaxMin")
            {
                actualizarMaxMin(t);
            }
            if (t.accion == "terminalOffline")
            {
                terminalOffline(t);
            }
            if (t.accion == "actualizarTerminal")
            {
                actualizarTerminal(t);

                _response = JsonConvert.SerializeObject(t);
                Console.WriteLine($"Se actualiza maximo: {_newMax}, se actualiza minimo: {_newMin}");
            }
            if (t.accion == "jugada")
            {
                _response = jugada();
            }

            if (t.accion == "SESIONCERRADA")
            {
                _response = sesionCerrada(t);
            }

            if (t.accion == "login")
            {
                _response = login(t);
            }
            if ((t.accion.Contains("apuesta")))
            {
                _response = apuesta(t);
            }
            if ((t.accion.Contains("retirarGanancias")))
            {
                _response = retirarGanancias(t);
            }

            return _response;
        }
    }
}
