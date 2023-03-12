using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
﻿using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System;

namespace Company.Function
{
    public class BanxicoService
    {
        public async Task<List<Rate>> GetExchangeRate(string initialDate, string finishDate)
        {
            List<Rate> rates = new List<Rate>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseUrlServicesBanxico);
                client.DefaultRequestHeaders.Add("Bmx-Token", Constants.BanxicoToken);

                HttpResponseMessage response = await client.GetAsync($"{Constants.BaseUrlServicesBanxico}{Constants.DolarSerie}/datos/{initialDate}/{finishDate}");
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var datos = JObject.Parse(result)["bmx"]["series"][0];
                    try
                    {
                        //return NoContent(new { data = rates, message = $"No se encontrarón tipos de cambios para las fechas {dates.InitialDate} y {dates.FinishDate}" });
                        foreach (var dato in datos["datos"])
                        {
                            rates.Add(new Rate { Date = dato["fecha"].ToString(), Value = dato["dato"].ToString() });
                        }

                    }
                    catch (Exception e)
                    {
                        //LogHelper.Log(LogTarget.File, $"{DateTime.Now} : {e.Message}");
                    }
                }
            }

            return rates;
        }

        public async Task<RateFixing> GetFixing(string locale="es")
        {
            RateFixing rate = new RateFixing();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseUrlServicesBanxico);
                client.DefaultRequestHeaders.Add("Bmx-Token", Constants.BanxicoToken);
                HttpResponseMessage response = await client.GetAsync($"{Constants.BaseUrlServicesBanxico}{Constants.DolarSerie}/datos/oportuno?locale={locale}");
                if (response.IsSuccessStatusCode)
                {                
                    var result = response.Content.ReadAsStringAsync().Result;
                    var datos = JObject.Parse(result)["bmx"]["series"][0]["datos"];
                    rate.Date = datos[0]["fecha"].ToString();
                    rate.Value = float.Parse(datos[0]["dato"].ToString());
                }
                else
                {
                    //LogHelper.Log(LogTarget.File, $"{DateTime.Now} : StatusCode isn't success");
                }
            }
            return rate;
        }

    }
}