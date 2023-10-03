using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private HorizontalLayoutGroup _parent;
    [SerializeField] private Character[] _characters;
    [SerializeField] private CharacterView _characterView;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private PlayersHandler _playersHandler;

    private void Start()
    {
        foreach (Character character in _characters)
        {
            CharacterView characterView = Instantiate(_characterView, _parent.transform);
            characterView.Initialize(character.Icon, character.Price, character);
            characterView.SellButtonClicked += OnSellButtonClick;
        }
    }

    private void OnSellButtonClick(Character character, CharacterView characterView)
    {
        TrySellCharacter(character, characterView);
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
}
