using Lean.Localization;
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
    [SerializeField] private Button _descriptionButton;
    [SerializeField] private TMP_Text _selectText;

    private LeanLocalization _lean;
    private Character _character;
    private Description _description;
    private string _select;
    private string _selected;

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

    public void Render(Sprite icon, int price, Character character, Description description, LeanLocalization lean)
    {
        _lean = lean;
        Translate();
        _image.sprite = icon;
        _priceText.text = price.ToString();
        _character = character;
        _sellButton.gameObject.SetActive(true);
        _selectButton.gameObject.SetActive(false);
        _description = description;
        _description.Hide();
    }

    public void UpdateView()
    {
        TryLockBuyCharacter();
        TryLockSelectCharacter();
    }

    private void TryLockBuyCharacter()
    {
        if (_character.IsBought)
        {
            _sellButton.gameObject.SetActive(false);
            _selectButton.gameObject.SetActive(true);
        }
    }

    private void TryLockSelectCharacter()
    {
        if (_character.IsSelect)
        {
            _selectButton.interactable = false;
            _selectText.text = _selected;
        }
        else
        {
            _selectButton.interactable = true;
            _selectText.text = _select;
        }
    }

    private void Translate()
    {
        if (_lean.CurrentLanguage == Constants.English)
            _selected = Constants.EnglishSelected;

        if (_lean.CurrentLanguage == Constants.Russian)
            _selected = Constants.RussianSelected;

        if (_lean.CurrentLanguage == Constants.Turkish)
            _selected = Constants.TurkishSelected;

        if (_lean.CurrentLanguage == Constants.English)
            _select = Constants.EnglishSelect;

        if (_lean.CurrentLanguage == Constants.Russian)
            _select = Constants.RussianSelect;

        if (_lean.CurrentLanguage == Constants.Turkish)
            _select = Constants.TurkishSelect;
    }

    private void OnDescriptionButtonClicked() => ShowDescription();

    private void ShowDescription() => _description.Show();
}
