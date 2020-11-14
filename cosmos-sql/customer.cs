using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace cosmos_sql
{
    class Customer
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } 
        [JsonProperty(PropertyName ="customerid")]
        public int Customerid { get; set; }
        [JsonProperty(PropertyName = "customername")]
        public string Customername { get; set; }
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        public Customer(int p_id,string p_name,string p_city)
        {
            Customerid = p_id;
            Customername = p_name;
            City=p_city;
        }
    }
}
