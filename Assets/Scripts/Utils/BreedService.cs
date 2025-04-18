// Models/WeatherModel.cs
// Core/IWeatherService.cs
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class BreedService : IBreedService
{
    // For example purposes, these endpoints are assumed.
    private const string BreedsUrl = "https://dogapi.dog/api/v2/breeds";
    private const string BreedDetailUrl = "https://dogapi.dog/api/v2/breeds/";

    public async UniTask<List<BreedModel>> GetBreedsAsync(CancellationToken token)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(BreedsUrl))
        {
            await request.SendWebRequest().ToUniTask(cancellationToken: token);

            if (request.result != UnityWebRequest.Result.Success)
                throw new Exception($"Error fetching breeds data: {request.error}");

            var json = request.downloadHandler.text;
            Debug.Log(json);

            var root = JObject.Parse(json);
            var jArray = root["data"] as JArray;

            var breeds = new List<BreedModel>();
            for (int i = 0; i < Math.Min(10, jArray.Count); i++)
            {
                var obj = jArray[i];
                var attributes = obj["attributes"];

                breeds.Add(new BreedModel
                {
                    Id = obj["id"]?.ToString(),
                    Name = attributes?["name"]?.ToString(),
                    Description = attributes?["description"]?.ToString() ?? "No description available."
                });
            }

            return breeds;
        }
    }
    public async UniTask<BreedModel> GetBreedDetailAsync(string breedId, CancellationToken token)
    {
        // Construct the URL using the provided breed ID
        string url = $"{BreedDetailUrl}{breedId}"; // Directly append the breedId to the base URL
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            await request.SendWebRequest().ToUniTask(cancellationToken: token);

            if (request.result != UnityWebRequest.Result.Success)
                throw new Exception($"Error fetching breed detail: {request.error}");

            var json = request.downloadHandler.text;

            var obj = JObject.Parse(json);

            // Check if the expected fields are present in the response
            if (obj == null || obj["data"] == null)
            {
                Debug.LogError("No data found in the response.");
                return null; // Return null if no data is found
            }

            var breedData = obj["data"]; // Access the "data" field

            return new BreedModel
            {
                Id = breedData["id"]?.ToString(),
                Name = breedData["attributes"]?["name"]?.ToString(),
                Description = breedData["attributes"]?["description"]?.ToString() ?? "No description available."
            };
        }
    }
}