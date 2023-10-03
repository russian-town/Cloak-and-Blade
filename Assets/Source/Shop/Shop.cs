using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private HorizontalLayoutGroup _parent;
    [SerializeField] private CharacterSetter[] _characterSetters;
    [SerializeField] private CharacterView _characterView;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private PlayersHandler _playersHandler;
    [SerializeField] private ModelsScroll _modelsScroll;

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
        foreach (var characterSetter in _characterSetters)
        {
            CharacterView characterView = Instantiate(_characterView, _parent.transform);
            characterView.Render(characterSetter.Character.Icon, characterSetter.Character.Price, characterSetter.Character);
            _characterViews.Add(characterView);
            characterView.SellButtonClicked += OnSellButtonClick;
            characterView.SelectButtonClicked += OnSelectButtonClick;
            MenuPlayerModel menuPlayerModel = Instantiate(characterSetter.Character.MenuPlayerModel, characterSetter.ModelPlace.transform);
            _modelsScroll.Initialize(menuPlayerModel);

            if (characterSetter.Character.Type == Type.Default)
            {
                characterSetter.Character.Buy();

                if (_currentSelectedCharacter == null)
                {
                    TrySelectCaracter(characterSetter.Character, characterView);
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
        if(character.IsBuyed)
        {
            _currentSelectedCharacter?.UnSelect();
            _currentCharacterView?.UpdateView();
            character.Select();
            _modelsScroll.SwitchCurrentModel(character.MenuPlayerModel);
            _playersHandler.SetCurrentPlayer(character.Player);
            _currentSelectedCharacter = character;
            _currentCharacterView = characterView;
            _currentCharacterView.UpdateView();
        }
    }
}

[System.Serializable]
public class CharacterSetter
{
    [SerializeField] private Character _caracter;
    [SerializeField] private MenuPlayerModel _menuPlayerModel;
    [SerializeField] private ModelPlace _modelPlace;

    public Character Character => _caracter;
    public ModelPlace ModelPlace => _modelPlace;
}
