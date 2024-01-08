﻿using Independent_Reader_GUI.Models;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Tsp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Services
{
    /// <summary>
    /// Service to manage the API
    /// </summary>
    internal class APIManager
    {
        private HttpClient client;
        private bool connected;

        public APIManager()
        {
            client = new HttpClient();
        }

        public HttpClient Client
        {
            get { return client; }
        }

        public bool Connected
        {
            get { return connected; }
        }

        public async Task CheckAPIConnection()
        {
            try
            {
                // TODO: Obtain the base url somewhere else
                HttpResponseMessage _ = await client.GetAsync("http://127.0.0.1:8000/");
                connected = _.IsSuccessStatusCode;
            }
            catch
            {
                connected = false;
            }
        }

        public async Task<TResponse> GetAsync<TResponse>(string endpoint)
        {
            try
            {
                // Get the response from the client based on the endpoint in the API
                HttpResponseMessage response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                // Read in the response data
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(responseBody);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"Error getting data to {endpoint} due to {ex.Message}");
                throw new ApplicationException($"Error getting data from {endpoint}", ex);
                //await GetAsync<TResponse>(endpoint);
            }
            return default(TResponse);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest content)
        {
            try
            {
                string jsonContent = JsonSerializer.Serialize(content);
                HttpContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(endpoint, httpContent);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(responseBody);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"Error positng data to {endpoint} due to {ex.Message}");
                throw new ApplicationException($"Error positng data to {endpoint}", ex);
                //await PostAsync<TRequest, TResponse>(endpoint, content);
            }
        }

        /// <summary>
        /// Dipose the HttpClient when the APIManager is disposed
        /// </summary>
        public void Disponse()
        {
            // Dispose of the client if it is open
            client?.Dispose();
        }
    }
}
