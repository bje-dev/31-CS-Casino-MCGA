namespace Backoffice.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using Microsoft.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;

    namespace DAL.Tools
    {
        internal static class SqlHelper
        {
            readonly static string conString;


            static SqlHelper()
            {
               
                conString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DefaultConnection"];

            }
            public static Int32 ExecuteNonQuery(String commandText,
                CommandType commandType, params SqlParameter[] parameters)
            {
                try
                {
                    CheckNullables(parameters);

                    using (SqlConnection conn = new SqlConnection(conString))
                    {
                        using (SqlCommand cmd = new SqlCommand(commandText, conn))
                        {
                             
                            cmd.CommandType = commandType;
                            cmd.Parameters.AddRange(parameters);



                            conn.Open();
                            return cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            private static void CheckNullables(SqlParameter[] parameters)
            {
                foreach (SqlParameter item in parameters)
                {
                    if (item.SqlValue == null)
                    {
                        item.SqlValue = DBNull.Value;
                    }
                }
            }

           
            public static Object ExecuteScalar(String commandText,
                CommandType commandType, params SqlParameter[] parameters)
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand(commandText, conn))
                    {
                        cmd.CommandType = commandType;
                        cmd.Parameters.AddRange(parameters);

                        conn.Open();
                        return cmd.ExecuteScalar();
                    }
                }
            }

            
            public static SqlDataReader ExecuteReader(String commandText,
                CommandType commandType, params SqlParameter[] parameters)
            {
                SqlConnection conn = new SqlConnection(conString);

                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    cmd.CommandType = commandType;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    return reader;
                }
            }
        }
    }
}
