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
    private PlayerData _playerData;
    private CloudSave _saver = new CloudSave();

    private void Start()
    {
        if (TryLoadSaveData())
            Debug.Log("Data loaded.");
        else
            Debug.Log("Data is null.");

        AddCharacterView();
    }

    private void OnDisable()
    {
        foreach (var characterView in _characterViews)
        {
            characterView.SelectButtonClicked -= OnSelectButtonClick;
        }
    }

    private bool TryLoadSaveData()
    {
        _playerData = _saver.Load();

        if (_playerData != null)
        {
            Debug.Log(_playerData.CurrentPlayer);
            _playersHandler.SetCurrentPlayer(_playerData.CurrentPlayer);
            Debug.Log(_playerData.CurrentSelectedCharacter);
            _currentSelectedCharacter = _playerData.CurrentSelectedCharacter;
            Debug.Log(_playerData.CurrentCharacterView);
            _currentCharacterView = _playerData.CurrentCharacterView;
            Debug.Log(_playerData.Money);
            return true;
        }

        return false;
    }

    private void SaveData()
    {
        _playerData = new PlayerData(_playersHandler.CurrentPlayer, _currentCharacterView, _currentSelectedCharacter, _wallet.Money);
        Debug.Log(_playerData.CurrentPlayer);
        _saver.Save(_playerData);
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

            SaveData();
        }
    }
}
