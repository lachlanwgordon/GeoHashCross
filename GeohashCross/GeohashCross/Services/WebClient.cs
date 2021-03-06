﻿using GeohashCross.Models;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GeohashCross.Services
{
    public static class Webclient
    {
        /// <summary>
        /// Single static instance of HttpClient. HttpClient should use singleton because they are reentrant.
        /// This improves serverside and client performance because sockets can be reused.
        /// Also prevents crashes in situations where many calls are made in a short period of time.
        /// </summary>
        /// <value>The client.</value>
        static HttpClient Client { get; } = new HttpClient();

        const string BaseUrl = "http://geo.crox.net/djia/";

        /// <summary>
        /// Gets the dow jones opening price for a given date.
        /// If no date provided it will use UTC today.
        /// </summary>
        /// <returns>The dow jones.</returns>
        /// <param name="date">Date.</param>
        public static async Task<Response<string>> GetDjia(DateTime date, bool allowCached = true, bool addToCache = true, bool checkConnectivity = true)
        {
            try
            {

                var dateString = date.ToString("yyyy/MM/dd");
                if (allowCached && Preferences.ContainsKey(dateString))
                {
                    return new Response<string>(Preferences.Get(dateString, ""), true, "Loaded DJIA from cache.");
                }


                var url = $"{BaseUrl}{dateString}";
                var response = await Client.GetStringAsync(url);
                if(addToCache)
                {
                    Preferences.Set(dateString, response);
                }
                Debug.WriteLine($"Got DJIA from web {response}");

                return new Response<string>(response, true, "Loaded data from web");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine($"fail to get: \n{ex}\n{ex.StackTrace}");
                if(checkConnectivity && Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
                {
                    return new Response<string>(null, false, "No internet connection available. Please reconnect");
                }

                return new Response<string>(null, false, "Couldn't connect to server or DJIA is not available yet");
            }
        }
    }
}
