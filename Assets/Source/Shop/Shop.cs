using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private HorizontalLayoutGroup _parent;
    [SerializeField] private List<Character> _characters = new List<Character>();
    [SerializeField] private CharacterView _characterView;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private PlayersHandler _playersHandler;
    [SerializeField] private MenuModelChanger _menuModelChanger;

    private List<CharacterView> _characterViews = new List<CharacterView>();
    private Character _currentSelectedCharacter;
    private CharacterView _currentCharacterView;

    private void Start()
    {
        AddCharacterView();
    }

    private void OnDisable()
    {
        foreach (var characterView in _characterViews)
        {
            characterView.SelectButtonClicked -= OnSelectButtonClick;
        }
    }

    private void AddCharacterView()
    {
        foreach (var character in _characters)
        {
            CharacterView characterView = Instantiate(_characterView, _parent.transform);
            characterView.Render(character.Icon, character.Price, character);
            _characterViews.Add(characterView);
            characterView.SellButtonClicked += OnSellButtonClick;
            characterView.SelectButtonClicked += OnSelectButtonClick;
            _menuModelChanger.Create(character);

            if (character.Type == Type.Default)
            {
                character.Buy();

                if (_currentSelectedCharacter == null)
                {
                    TrySelectCaracter(character, characterView);
                    _menuModelChanger.SetDefaultModel(_characterViews.IndexOf(characterView));
                }

                Debug.Log("Set default player.");
            }

            characterView.UpdateView();
        }
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
        if (character.Price <= _wallet.Money)
        {
            _wallet.DicreaseMoney(character.Price);
            character.Buy();
            _playersHandler.AddAviablePlayer(character.Player);
            characterView.SellButtonClicked -= OnSellButtonClick;
        }
    }

    private void TrySelectCaracter(Character character, CharacterView characterView)
    {
        if (character.IsBuyed)
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
        }
    }
}
