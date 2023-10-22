using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Shop : MonoBehaviour, IDataReader, IDataWriter, IInitializable
{
    [SerializeField] private HorizontalLayoutGroup _parent;
    [SerializeField] private List<Character> _characters = new List<Character>();
    [SerializeField] private CharacterView _characterView;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private PlayersHandler _playersHandler;
    [SerializeField] private MenuModelChanger _menuModelChanger;
    [SerializeField] private Upgrader _upgrader;

    private List<CharacterView> _characterViews = new List<CharacterView>();
    private Character _currentSelectedCharacter;
    private CharacterView _currentCharacterView;

    public event UnityAction CharacterSelected;
    public event UnityAction CharacterSold;

    private void OnDisable()
    {
        foreach (var characterView in _characterViews)
        {
            characterView.SelectButtonClicked -= OnSelectButtonClick;
        }
    }

    public void Initialize() => AddCharacterView();

    private void AddCharacterView()
    {
        foreach (var character in _characters)
        {
            CharacterView characterView = Instantiate(_characterView, _parent.transform);
            Description description = Instantiate(character.Description);
            description.Initialize();
            _upgrader.Initialize(description);
            characterView.Render(character.Icon, character.Price, character, description);
            _characterViews.Add(characterView);
            characterView.SellButtonClicked += OnSellButtonClick;
            characterView.SelectButtonClicked += OnSelectButtonClick;
            _menuModelChanger.Create(character);

            if (character.Type == Type.Default)
            {
                character.Buy();

                if (_currentSelectedCharacter == null)
                    SetCurrentCharacter(character, characterView);
            }

            if (_currentSelectedCharacter == character)
            {
                _menuModelChanger.SetSelectedModel(_characterViews.IndexOf(characterView));
                SetCurrentCharacter(character, characterView);
            }

            characterView.UpdateView();
        }
    }

    public void Read(PlayerData playerData)
    {
        _currentSelectedCharacter = playerData.CurrentSelectedCharacter;
    }

    public void Write(PlayerData playerData)
    {
        playerData.CurrentSelectedCharacter = _currentSelectedCharacter;
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
            _wallet.DicreaseMoney(character.Price);
            character.Buy();
            characterView.SellButtonClicked -= OnSellButtonClick;
            CharacterSold?.Invoke();
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

            if (_characterViews.Contains(characterView))
            {
                _menuModelChanger.TryChange(_characterViews.IndexOf(characterView));
            }

            CharacterSelected?.Invoke();
        }
    }
}
