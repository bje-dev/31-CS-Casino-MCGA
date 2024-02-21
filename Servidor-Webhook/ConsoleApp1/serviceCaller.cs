using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class serviceCaller
    {
        public static string callService(string path, string method, string jsonRequest)
        {
            string _result = "";

            WebRequest request = WebRequest.Create(path);
            request.Method = method;
            request.ContentType = "application/json; charset=utf-8";
            if (jsonRequest != "")
            {
                var data = Encoding.ASCII.GetBytes(jsonRequest);
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            WebResponse response = request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            var getData = reader.ReadToEnd();

            _result = getData;

            return _result;
        }
    }
}
