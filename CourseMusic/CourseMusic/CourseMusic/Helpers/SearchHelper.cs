﻿using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static CourseMusic.Models.Search;
using CourseMusic.Models;

namespace CourseMusic.Helpers;

public static class SearchHelper
{
    public static Token? Token { get; set; }

    public static async Task GetTokenAsync()
    {
        #region SecretVault
        string clientID = "d6c62bd2908e49a68a29c204b9c716e5";

            string clientSecret = "7af3b3575a264224abfc9f22c380bbfb";
        #endregion

        string auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(clientID + ":" + clientSecret));

        List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        };

        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Basic {auth}");
        HttpContent content = new FormUrlEncodedContent(args);

        HttpResponseMessage resp = await client.PostAsync("https://accounts.spotify.com/api/token", content);
        string msg = await resp.Content.ReadAsStringAsync();

        Token = JsonConvert.DeserializeObject<Token>(msg);
    }

    public static Result SearchArtistOrSong(string searchWord)
    {
        var client = new RestClient("https://api.spotify.com/v1/search");
        client.AddDefaultHeader("Authorization", $"Bearer {Token?.Access_token}");
        var request = new RestRequest($"?q={searchWord}&type=artist", Method.Get);
        var response = client.Execute(request);

        if (response.IsSuccessful)
        {
            var result = JsonConvert.DeserializeObject<Result>(response.Content!);
            return result!;
        }
        else
            return null!;
    }
}