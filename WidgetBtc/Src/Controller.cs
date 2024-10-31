using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using RestSharp;
using System.Text.Json;
using Avalonia.Controls.Notifications;
using WidgetBtc;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace Controller
{
    public class Fetch(string url, Window app)
    {
        private readonly RestClient client = new(url);
        private readonly Window application = app;

        public async Task<List<T>?> GetList<T>(string route)
        {
            try
            {
                var request = new RestRequest(route, Method.Get);

                var response = await client.ExecuteAsync(request);
                string? jsonString = response.Content;
                List<T>? json = JsonSerializer.Deserialize<List<T>>(jsonString ?? "");

                return json ?? null;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);

            }
            return null;
        }

        public async Task<T?> Get<T>(string route)
        {
            try
            {
                var request = new RestRequest(route, Method.Get);

                var response = await client.ExecuteAsync(request);
                string? jsonString = response.Content;
                //Console.WriteLine(jsonString);

                var json = JsonSerializer.Deserialize<T>(jsonString ?? "");

                return json;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);

            }
            return default;
        }

        public async Task<string?> GetString(string route)
        {
            try
            {
                var request = new RestRequest(route, Method.Get);
                var response = await client.ExecuteAsync(request);
                string content = response.Content ?? "";

                return content;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);

            }
            return null;

        }


        public async Task<string?> Post(string route)
        {

            var request = new RestRequest(route, Method.Post);

            var response = await client.ExecuteAsync(request);

            string? content = response.Content;

            return content;
        }

    }


    public class Coroutine(int time, Action fn)
    {
        private readonly int time = time;
        private readonly Action fn = fn;

        public async void Start()
        {
            fn();
            await Task.Delay(time);
            Start();
        }

    }
    public class Coin
    {
        public Money? BTCUSD { get; set; }
        public Money? BTCBRL { get; set; }
        public Money? USDBRL { get; set; }

        public class Money
    {
        public string? code { get; set; }       // Ex.: "BTC"
        public string? codein { get; set; }     // Ex.: "USD"
        public string? name { get; set; }       // Ex.: "Bitcoin/Dólar Americano"
        public string? high { get; set; }       // Ex.: "72936.00" (alta)
        public string? low { get; set; }        // Ex.: "71510.00" (baixa)
        public string? varBid { get; set; }     // Ex.: "-515.00" (variação do preço de compra)
        public string? pctChange { get; set; }  // Ex.: "-0.71" (percentual de mudança)
        public string? bid { get; set; }        // Ex.: "71983.00" (preço de compra)
        public string? ask { get; set; }        // Ex.: "71984.00" (preço de venda)
        public string? timestamp { get; set; }     // Ex.: "1730316878" (timestamp UNIX)
        public string? createDate { get; set; } // Ex.: "2024-10-30 16:34:38" (data de criação)
    }
    }
    
}
