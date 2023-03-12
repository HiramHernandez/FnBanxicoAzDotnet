
using System;
using System.Collections.Generic;
﻿using Newtonsoft.Json;

namespace Company.Function
{

     public class Rate
    {
        public string Date { get; set; }
        public string Value { get; set; }
    }

    public class RateFixing
    {
        public string Date { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public float Value { get; set; } = 0f;
    }

    public class BmxModel
    {
        //SeriesBanxicoExchangeRate
        public List<SeriesBanxicoExchangeRate> Series { get; set; }
        public class SeriesBanxicoExchangeRate
        {

            [JsonProperty("idSerie")]
            public string idSerie { get; set; }
            public string titulo { get; set; }

            public List<ExchangeRate> Datos { get; set; }
        }
        
        public class Bmx
        {
            public List<SeriesBanxicoExchangeRate> Series { get; set; }
        }

        public class ExchangeRate
        {
            public string Date { get; set; }
            public string Value { get; set; }
        }

        

    }



}