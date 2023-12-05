using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;

public class CharacterView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Button _sellButton;
    [SerializeField] private Image _priceTag;
    [SerializeField] private Button _selectButton;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Button _descriptionButton;
    [SerializeField] private List<Image> _stars;
    [SerializeField] private List<ChainDOTanimation> _chains;

    private Character _character;
    private Description _description;
    private UpgradeSetter _upgradeSetter;
    private Wallet _wallet;
    private WaitForSeconds _pauseBetweenChainFall = new WaitForSeconds(.3f);
    private Coroutine _chainFallCoroutine;

    public event Action<Character, CharacterView> SellButtonClicked;
    public event Action<Character, CharacterView> SelectButtonClicked;

    private void OnEnable()
    {
        _sellButton.onClick.AddListener(() => SellButtonClicked?.Invoke(_character, this));
        _sellButton.onClick.AddListener(TryLockBuyCharacter);
        _selectButton.onClick.AddListener(() => SelectButtonClicked?.Invoke(_character, this));
        _selectButton.onClick.AddListener(TryLockSelectCharacter);
        _descriptionButton.onClick.AddListener(OnDescriptionButtonClicked);
        _upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
    }

    private void OnDisable()
    {
        _sellButton.onClick.RemoveListener(() => SellButtonClicked?.Invoke(_character, this));
        _sellButton.onClick.RemoveListener(TryLockBuyCharacter);
        _selectButton.onClick.RemoveListener(() => SelectButtonClicked?.Invoke(_character, this));
        _selectButton.onClick.RemoveListener(TryLockSelectCharacter);
        _descriptionButton.onClick.RemoveListener(OnDescriptionButtonClicked);
        _upgradeButton.onClick.RemoveListener(OnUpgradeButtonClicked);
    }
    

    public void Render(Sprite icon, Character character, Description description, Wallet wallet)
    {
        _image.sprite = icon;
        _character = character;
        _upgradeSetter = _character.UpgradeSetter;
        _description = description;
        _description.Hide();
        _wallet = wallet;
    }

    private void ResetView()
    {
        for (int i = 0; i < Constants.MaxLevel; i++)
            _stars[i].gameObject.SetActive(false);
    }

    public void UpdateView()
    {
        TryLockBuyCharacter();
        TryLockSelectCharacter();

        if (_stars.Count == 0)
            return;

        ResetView();
        UpdateStars();
    }

    public void RemoveChains()
    {
        _chainFallCoroutine = StartCoroutine(RemoveChainsWithPause());
    }

    public void ShakeChaings()
    {
        foreach (var chain in _chains)
            chain.PlayShakeAnimation();
    }

    public void DisableButtons(int levelValue)
    {
        if (_upgradeSetter.Level == levelValue)
        {
            print("trying to disable buttons");
            _priceTag.gameObject.SetActive(false);
            _sellButton.gameObject.SetActive(false);
            _upgradeButton.gameObject.SetActive(false);
        }
    }

    public void TryHideChains()
    {
        if (_character.IsBought)
            foreach (var chain in _chains)
                chain.gameObject.SetActive(false);
        else
            foreach (var chain in _chains)
                chain.gameObject.SetActive(true);
    }

    private IEnumerator RemoveChainsWithPause()
    {
        foreach(var chain in _chains)
        {
            chain.PlayFallAnimation();
            yield return _pauseBetweenChainFall;
        }
    }

    private void UpdateStars()
    {
        for (int i = 0; i < _upgradeSetter.Level; i++)
        {
            _stars[i].gameObject.SetActive(true);

            if (i + 1 >= _upgradeSetter.Prices.Count)
                break;

            _priceText.text = _upgradeSetter.Prices[i + 1].ToString();
        }
    }

    private void TryLockBuyCharacter()
    {
        if (_character.IsBought)
        {
            if (_upgradeSetter.Level < Constants.MaxLevel)
            {
                _sellButton.gameObject.SetActive(false);
                _upgradeButton.gameObject.SetActive(true);
                _priceText.text = _upgradeSetter.Prices[_upgradeSetter.Level].ToString();
            }
        }
        else
        {
            if (_upgradeSetter.Level < Constants.MaxLevel)
            {
                _sellButton.gameObject.SetActive(true);
                _upgradeButton.gameObject.SetActive(false);
                _priceText.text = _character.Price.ToString();
            }
        }
    }

    private void TryLockSelectCharacter()
    {
        if (_character.IsSelect)
            _selectButton.interactable = false;

        else
            _selectButton.interactable = true;
    }

    private void OnUpgradeButtonClicked()
    {
        if (_wallet.Stars >= _upgradeSetter.Prices[_upgradeSetter.Level])
        {
            DisableButtons(Constants.MaxLevel - 1);
            _wallet.DicreaseStars(_upgradeSetter.Prices[_upgradeSetter.Level]);
            _upgradeSetter.Upgrade();
            UpdateStars();
        }
    }

    private void OnDescriptionButtonClicked() => ShowDescription();

    private void ShowDescription() => _description.Show();
}
