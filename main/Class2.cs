using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace main
{
    public class Root
    {
        public DateTime Date { get; set; }
        public DateTime PreviousDate { get; set; }
        public string PreviousURL { get; set; }
        public DateTime Timestamp { get; set; }
        public List<Order> Valute { get; set; }
    }

    public class Valute
    {
        public Order USD { get; set; }
        public Order EUR { get; set; }
        public Order VND { get; set; }
        public Order KZT { get; set; }
        public Order CAD { get; set; }
        public Order CNY { get; set; }
        public Order UAH { get; set; }
        public Order CZK { get; set; }
    }

    public class Order
    {
        public string ID { get; set; }
        public string NumCode { get; set; }
        public string CharCode { get; set; }
        public int Nominal { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public double Previous { get; set; }

    }

    public class ReadAndParseJsonFileWithNewtonsoftJson
    {
        private readonly string response;

        public ReadAndParseJsonFileWithNewtonsoftJson(string sampleJsonFilePath)
        {
            response = sampleJsonFilePath;
        }
    }


}
