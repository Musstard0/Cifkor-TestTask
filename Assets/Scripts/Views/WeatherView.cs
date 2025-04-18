// Views/WeatherView.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class WeatherView : MonoBehaviour, IWeatherView
{
    [SerializeField] private Image weatherIcon;
    [SerializeField] private TextMeshProUGUI temperatureText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    public void UpdateWeather(WeatherModel model)
    {
        temperatureText.text = $"Сегодня - {model.Temperature}F";
        descriptionText.text = model.Description;
        StartCoroutine(LoadIcon(model.IconUrl));
    }

    private IEnumerator LoadIcon(string url)
    {
        using var request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var texture = DownloadHandlerTexture.GetContent(request);
            weatherIcon.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
    }
}
