using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Description : MonoBehaviour
{
    [SerializeField] private TMP_Text _curentPriceText;
    [SerializeField] private List<Image> _levelMarkers = new List<Image>();
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private UpgradeSetter _upgradeSetter;
    [SerializeField] private ScreenAnimationHandler _animationHandler;

    public event UnityAction<UpgradeSetter, Description> UpgradeButtonClicked;

    private void OnDisable()
    {
        _upgradeButton.onClick.RemoveListener(() => UpgradeButtonClicked?.Invoke(_upgradeSetter, this));
    }

    public void Initialize()
    {
        UpdateView();
    }

    public void UpdateView()
    {
        if (_levelMarkers.Count == 0)
            return;

        ResetView();

        if (_upgradeSetter.Level == 0)
        {
            _curentPriceText.text = _upgradeSetter.Prices[0].ToString();
            return;
        }

        for (int i = 0; i < _upgradeSetter.Level; i++)
        {
            _levelMarkers[i].gameObject.SetActive(true);

            if (i + 1 >= _upgradeSetter.Prices.Count)
                break;

            _curentPriceText.text = _upgradeSetter.Prices[i + 1].ToString();
        }
    }

    private void ResetView()
    {
        for (int i = 0; i < _upgradeSetter.Level; i++)
        {
            _levelMarkers[i].gameObject.SetActive(false);
        }
    }

    public void Show()
    {
        _animationHandler.ScreenFadeIn();
        _upgradeButton.onClick.AddListener(() => UpgradeButtonClicked?.Invoke(_upgradeSetter, this));
    }

    public void Hide()
    {
        _animationHandler.ScreenFadeOut();
        _upgradeButton.onClick.RemoveListener(() => UpgradeButtonClicked?.Invoke(_upgradeSetter, this));
    }
}
