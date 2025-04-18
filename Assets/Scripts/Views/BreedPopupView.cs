using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class BreedPopupView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton?.OnClickAsObservable().Subscribe(_ => ClosePopup());
    }

    public void SetData(BreedModel breed)
    {
        titleText.text = breed.Name;
        descriptionText.text = breed.Description;
    }
    public void Show()
    {

        gameObject.SetActive(true);
    }
    public void ClosePopup()
    {
        // Animate closing (fade out) and then hide the popup.
        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.DOFade(0, 0.3f).OnComplete(() =>
            {
                gameObject.SetActive(false); // Hide the popup after fading out
                cg.alpha = 1; // Reset alpha for next time it opens
            });
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
