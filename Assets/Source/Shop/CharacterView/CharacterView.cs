using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Button _sellButton;
    [SerializeField] private Button _selectButton;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Button _descriptionButton;
    [SerializeField] private List<Image> _stars;

    private Character _character;
    private Description _description;
    private UpgradeSetter _upgradeSetter;

    public event UnityAction<Character, CharacterView> SellButtonClicked;
    public event UnityAction<Character, CharacterView> SelectButtonClicked;

    private void OnEnable()
    {
        _sellButton.onClick.AddListener(() => SellButtonClicked?.Invoke(_character, this));
        _sellButton.onClick.AddListener(TryLockBuyCharacter);
        _selectButton.onClick.AddListener(() => SelectButtonClicked?.Invoke(_character, this));
        _selectButton.onClick.AddListener(TryLockSelectCharacter);
        _descriptionButton.onClick.AddListener(OnDescriptionButtonClicked);
    }

    private void OnDisable()
    {
        _sellButton.onClick.RemoveListener(() => SellButtonClicked?.Invoke(_character, this));
        _sellButton.onClick.RemoveListener(TryLockBuyCharacter);
        _selectButton.onClick.RemoveListener(() => SelectButtonClicked?.Invoke(_character, this));
        _selectButton.onClick.RemoveListener(TryLockSelectCharacter);
        _descriptionButton.onClick.RemoveListener(OnDescriptionButtonClicked);
    }
    

    public void Render(Sprite icon, Character character, Description description)
    {
        _image.sprite = icon;
        _character = character;
        _upgradeSetter = _character.UpgradeSetter;
        _description = description;
        _description.Hide();
    }

    private void ResetView()
    {
        for (int i = 0; i < _upgradeSetter.Level; i++)
        {
            _stars[i].gameObject.SetActive(false);
        }
    }

    public void UpdateView()
    {
        TryLockBuyCharacter();
        TryLockSelectCharacter();

        if (_stars.Count == 0)
            return;

        ResetView();

        if (_upgradeSetter.Level == 0)
        {
            _priceText.text = _upgradeSetter.Prices[0].ToString();
            return;
        }

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
            _sellButton.gameObject.SetActive(false);
            _upgradeButton.gameObject.SetActive(true);

            _priceText.text = _upgradeSetter.Prices[_upgradeSetter.Level].ToString();
        }
        else
        {
            _priceText.text = _character.Price.ToString();
        }
    }

    private void TryLockSelectCharacter()
    {
        if (_character.IsSelect)
            _selectButton.interactable = false;

        else
            _selectButton.interactable = true;
    }

    private void OnDescriptionButtonClicked() => ShowDescription();

    private void ShowDescription() => _description.Show();
}
