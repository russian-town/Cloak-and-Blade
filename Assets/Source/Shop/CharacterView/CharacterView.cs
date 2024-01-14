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
    [SerializeField] private List<StarAnimationHandler> _stars;
    [SerializeField] private List<ChainDOTanimation> _chains;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _fallingChainsSound;
    [SerializeField] private CharacterViewSoundHandler _soundHandler;
    [SerializeField] private SelectedImageFader _selectedImage;

    private Character _character;
    private Description _description;
    private UpgradeSetter _upgradeSetter;
    private Wallet _wallet;
    private WaitForSeconds _pauseBetweenChainFall = new WaitForSeconds(.3f);
    private Coroutine _chainFallCoroutine;

    public event Action<Character, CharacterView> SellButtonClicked;
    public event Action<Character, CharacterView> SelectButtonClicked;

    public CharacterViewSoundHandler SoundHandler => _soundHandler;

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


    public void Render(Character character, Description description, Wallet wallet)
    {
        _character = character;
        _upgradeSetter = _character.UpgradeSetter;
        _description = description;
        _description.Hide();
        _wallet = wallet;
        _selectedImage.SetColor(_character.EffectColor);
    }

    public void UpdateView()
    {
        TryLockBuyCharacter();
        TryLockSelectCharacter();

        if (_stars.Count == 0)
            return;

        ResetStarsView();
        UpdateStars();
    }

    public void RemoveChains()
    {
        if (_chainFallCoroutine != null)
            StopCoroutine(_chainFallCoroutine);

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

    public void UnlockCharacter() => _image.sprite = _character.UnlockedIcon;

    private void ResetStarsView()
    {
        for (int i = 0; i < Constants.MaxLevel; i++)
            _stars[i].gameObject.SetActive(false);
    }

    private IEnumerator RemoveChainsWithPause()
    {
        _source.clip = _fallingChainsSound;

        foreach (var chain in _chains)
        {
            _source.Play();
            chain.PlayFallAnimation();
            yield return _pauseBetweenChainFall;
        }

        _chainFallCoroutine = null;
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
            _image.sprite = _character.UnlockedIcon;

            if (_upgradeSetter.Level < Constants.MaxLevel)
            {
                _sellButton.gameObject.SetActive(false);
                _upgradeButton.gameObject.SetActive(true);
                _priceText.text = _upgradeSetter.Prices[_upgradeSetter.Level].ToString();
            }
        }
        else
        {
            _image.sprite = _character.LockedIcon;

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
            _selectedImage.ChangeAlpha(1);
        else
            _selectedImage.ChangeAlpha(0);
    }

    private void OnUpgradeButtonClicked()
    {
        if (_wallet.Stars >= _upgradeSetter.Prices[_upgradeSetter.Level])
        {
            DisableButtons(Constants.MaxLevel - 1);
            _wallet.DicreaseStars(_upgradeSetter.Prices[_upgradeSetter.Level]);
            _upgradeSetter.Upgrade();
            UpdateStars();
            _stars[_upgradeSetter.Level - 1].PlayAppearAnimation();
            _soundHandler.PlayUpgrade();
        }
        else
        {
            _soundHandler.PlayNegative();
        }
    }

    private void OnDescriptionButtonClicked() => ShowDescription();

    private void ShowDescription() => _description.Show();
}
