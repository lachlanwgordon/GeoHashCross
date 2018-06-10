using System;
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
                var dateString = date.Value.ToString("yyyy/MM/dd");

                //remove this
                //dateString = "2005/05/26";

                var url = $"{BaseUrl}{dateString}";
                var response = await Client.GetStringAsync(url);
                var data = JsonConvert.DeserializeObject(response);
                return response;
            }
            catch(Exception ex)
            {
                

            }
            try
            {
                Debug.WriteLine("using alt url");

                date = date.HasValue ? date.Value.Date : DateTime.UtcNow.Date;
                var dateString = date.Value.ToString("yyyy/MM/dd");

                //remove this
                //dateString = "2005/05/26";


                var url = $"{altUrl}{dateString}";
                var response = await Client.GetStringAsync(url);
                var data = JsonConvert.DeserializeObject(response);
                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"REjected using hardcoded 25209.29 \n{ex}");
                return "10458.68";
            }
        }
    }
}
