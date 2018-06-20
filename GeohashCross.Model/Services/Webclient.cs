using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GeohashCross.Model.Services
{
    public static class Webclient
    {
        /// <summary>
        /// Single static instance of HttpClient. HttpClient should use singleton because they are reentrant.
        /// This improves serverside and client performance because sockets can be reused.
        /// Also prevents crashes in situations where many calls are made in a short period of time.
        /// </summary>
        /// <value>The client.</value>
        static HttpClient Client { get;} = new HttpClient();


        const string BaseUrl = "http://geo.crox.net/djia/";
        const string altUrl = "http://carabiner.peeron.com/xkcd/map/data/";



        static Dictionary<DateTime, string> Cache = new Dictionary<DateTime, string>();

        /// <summary>
        /// Gets the dow jones opening price for a given date.
        /// If no date provided it will use UTC today.
        /// </summary>
        /// <returns>The dow jones.</returns>
        /// <param name="date">Date.</param>
        public static async Task<string> GetDowJones(DateTime? date = null)
        {
            try
            {
                date = date.HasValue ? date.Value.Date : DateTime.UtcNow.Date;
                if(Cache.ContainsKey(date.Value))
                {
                    return Cache[date.Value];
                }



                var dateString = date.Value.ToString("yyyy/MM/dd");


                var url = $"{BaseUrl}{dateString}";
                var response = await Client.GetStringAsync(url);

                Cache.Add(date.Value, response);
                return response;
            }
            catch(Exception ex)
            {
                
                Debug.WriteLine($"fail to get: \n{ex}\n{ex.StackTrace}");

            }
            try
            {
                Debug.WriteLine("using alt url");

                date = date.HasValue ? date.Value.Date : DateTime.UtcNow.Date;
                var dateString = date.Value.ToString("yyyy/MM/dd");

                var url = $"{altUrl}{dateString}";
                var response = await Client.GetStringAsync(url);
                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"REjected using hardcoded 25209.29 \n{ex}");
                Cache.Add(date.Value, "25209.29");

                return "25209.29";
            }
        }
    }
}
