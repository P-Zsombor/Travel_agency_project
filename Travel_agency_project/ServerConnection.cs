using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace Travel_agency_project
{
    public class ServerConnection
    {
        public static HttpClient client = new HttpClient();


        public async Task<bool> Registration(string username, string password)
        {
            if (username.Length == 0 && password.Length == 0)
            {
                return false;
            }


            string url = $"http://localhost:3000/register";
            try
            {
                
                var jsonData = new
                {
                    registerUsername = username,
                    registerPassword = password
                };
                

                string json = JsonConvert.SerializeObject(jsonData);
                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.SendAsync(request);

                    string responseText = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        System.Windows.MessageBox.Show($"Hiba történt: {responseText}");
                        return false;
                    }

                    if (!responseText.StartsWith("{"))
                    {
                        return true;
                    }

                    try
                    {
                        JsonResponse responseJson = JsonConvert.DeserializeObject<JsonResponse>(responseText);
                        return true;
                    }
                    catch (JsonException)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show($"Hiba: {e.Message}");
                return false;
            }
        }


        public async Task<string> LoginAsync(string username, string password)
        {
            try
            {
                var requestBody = new
                {
                    loginUsername = username,
                    loginPassword = password
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://localhost:3000/login", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return "Error";
                }

                var responseData = JsonConvert.DeserializeObject<dynamic>(responseBody);
                return responseData.token;
            }
            catch (Exception)
            {
                return "NTE";
            }
          
        }


        public async Task<string> GetProfileDataAsync(string token)
        {
            try
            {
                string url = "http://localhost:3000/users/me";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return json;
                }
                else
                {
                    return $"Hiba: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                return $"Hiba történt: {ex.Message}";
            }
        }



        public async Task<bool> UpdateTrip(int id, string name, string destination, string accommodation, string transport)
        {
            try
            {
                var jsonInfo = new
                {
                    tripName = name,
                    tripDestination = destination,
                    tripAccommodation = accommodation,
                    tropTransport = transport
                };

                string url = $"http://localhost:3000/trips/{id}";

                string jsonStringified = JsonConvert.SerializeObject(jsonInfo);
                HttpContent send = new StringContent(jsonStringified, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(url, send);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteTrip(int id)
        {
            try
            {
                string url = $"http://localhost:3000/trips/{id}";

                HttpResponseMessage response = await client.DeleteAsync(url);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e.Message}");
                return false;
            }
        }

        public async Task<string> SearchName(string name)
        {
            try
            {
                string url = $"http://localhost:3000/trips/name/{name}";

                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string res = await response.Content.ReadAsStringAsync();
                MessageBox.Show(res);
                return res;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e.Message}");
                return null;
            }
        }
    }
}

public class JsonResponse
{
    public string username { get; set; }
    public string password { get; set; }
    public string message { get; set; }
    public string token { get; set; }

}
