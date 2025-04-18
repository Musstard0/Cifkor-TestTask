using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BreedItemView : MonoBehaviour
{
    [SerializeField] private TMP_Text numberText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject loaderGO;   // assign your spinner here
    [SerializeField] private Button button;

    public string BreedId => _breedId;
    private string _breedId;

    public void SetData(int number, BreedModel breed)
    {
        _breedId = breed.Id;
        numberText.text = number.ToString();
        nameText.text = breed.Name;
        SetLoading(false);
    }

    public void SetOnClick(System.Action onClick)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick());
    }

    public void SetLoading(bool loading)
    {
        if (loaderGO != null)
        {
            // fade in/out loader with DOTween
            loaderGO.SetActive(true);
            var cg = loaderGO.GetComponent<CanvasGroup>()
                     ?? loaderGO.AddComponent<CanvasGroup>();
            float from = loading ? 0 : 1;
            float to = loading ? 1 : 0;
            cg.alpha = from;
            cg.DOFade(to, 0.2f)
              .OnComplete(() => loaderGO.SetActive(loading));
        }
    }

}

