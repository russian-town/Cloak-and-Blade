using Lean.Localization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Shop : MonoBehaviour, IInitializable
{
    [SerializeField] private HorizontalLayoutGroup _parent;
    [SerializeField] private List<Character> _characters = new List<Character>();
    [SerializeField] private CharacterView _characterView;
    [SerializeField] private PlayersHandler _playersHandler;
    [SerializeField] private MenuModelChanger _menuModelChanger;
    [SerializeField] private Upgrader _upgrader;
    [SerializeField] private LeanLocalization _lean;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _shakingChainsSound;
    [SerializeField] private Camera _camera;

    private List<CharacterView> _characterViews = new List<CharacterView>();
    private Character _currentSelectedCharacter;
    private CharacterView _currentCharacterView;
    private Wallet _wallet;
    private bool _isInitialized;
    private Character _defaultCharacter;
    private CharacterView _defaultCharacterView;

    public event UnityAction CharacterSelected;
    public event UnityAction CharacterSold;

    private void OnDisable()
    {
        foreach (var characterView in _characterViews)
            characterView.SelectButtonClicked -= OnSelectButtonClick;
    }

    public void SetWallet(Wallet wallet)
    {
        _wallet = wallet;
        _upgrader.SetWallet(_wallet);
        AddCharacterView();
        _isInitialized = true;
    }

    public void CloseDescriptions()
    {
        foreach (var character in _characters)
            character.CloseDescription();
    }

    public void Initialize(){}

    private void AddCharacterView()
    {
        foreach (var character in _characters)
        {
            CharacterView characterView = Instantiate(_characterView, _parent.transform);
            Description description = Instantiate(character.Description);
            _upgrader.Initialize(description);
            characterView.Render(character, description, _wallet, _camera);
            _characterViews.Add(characterView);
            characterView.SellButtonClicked += OnSellButtonClick;
            characterView.SelectButtonClicked += OnSelectButtonClick;
            _menuModelChanger.Create(character);

            if (character.Type == Type.Default)
            {
                _defaultCharacter = character;
                _defaultCharacterView = characterView;
                character.Buy();
            }

            if (character.IsSelect)
            {
                _currentSelectedCharacter = character;
                SetCurrentCharacter(character, characterView);
                _menuModelChanger.SetSelectedModel(_characterViews.IndexOf(characterView));
            }

            characterView.UpdateView();
            characterView.TryHideChains();
            characterView.DisableButtons(Constants.MaxLevel);
        }

        if (_currentSelectedCharacter == null)
        {
            SetCurrentCharacter(_defaultCharacter, _defaultCharacterView);
            _menuModelChanger.SetSelectedModel(_characterViews.IndexOf(_defaultCharacterView));
        }
    }

    private void SetCurrentCharacter(Character character, CharacterView characterView)
    {
        _menuModelChanger.SetSelectedModel(_characterViews.IndexOf(characterView));
        TrySelectCaracter(character, characterView);
        _characterView = characterView;
    }

    private void OnSellButtonClick(Character character, CharacterView characterView)
    {
        TrySellCharacter(character, characterView);
    }

    private void OnSelectButtonClick(Character character, CharacterView characterView)
    {
        TrySelectCaracter(character, characterView);
    }

    private void TrySellCharacter(Character character, CharacterView characterView)
    {
        if (character.Price <= _wallet.Stars)
        {
            _wallet.DicreaseStars(character.Price);
            character.Buy();
            characterView.SellButtonClicked -= OnSellButtonClick;
            characterView.RemoveChains();
            characterView.SoundHandler.PlayUnlock();
            characterView.UnlockCharacter();
            TrySelectCaracter(character, characterView);
            CharacterSold?.Invoke();
        }
        else
        {
            _source.clip = _shakingChainsSound;
            _source.Play();
            characterView.ShakeChaings();
        }
    }

    private void TrySelectCaracter(Character character, CharacterView characterView)
    {
        if (character.IsBought)
        {
            _currentSelectedCharacter?.UnSelect();
            _currentCharacterView?.UpdateView();
            character.Select();
            _playersHandler.SetCurrentPlayer(character.Player);
            _currentSelectedCharacter = character;
            _currentCharacterView = characterView;
            _currentCharacterView.UpdateView();

            if (_isInitialized)
                characterView.SoundHandler.PlayPositive();

            if (_characterViews.Contains(characterView))
                _menuModelChanger.TryChange(_characterViews.IndexOf(characterView));

            CharacterSelected?.Invoke();
        }
        else
        {
            _source.clip = _shakingChainsSound;
            _source.Play();
            characterView.ShakeChaings();
        }
    }
}
