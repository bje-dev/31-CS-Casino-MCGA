using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tragamonedas
{
    public static class JSONbuilder
    {
        public static string serialize(object objeto)
        {
            string jsonString = "";
            jsonString = JsonConvert.SerializeObject(objeto);
            return jsonString;
        }
    }
}
