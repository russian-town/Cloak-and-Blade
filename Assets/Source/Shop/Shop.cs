using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private HorizontalLayoutGroup _parent;
    [SerializeField] private Character[] _characters;
    [SerializeField] private CharacterView _characterView;
    [SerializeField] private Wallet _wallet;

    private void Start()
    {
        foreach (Character character in _characters)
        {
            CharacterView characterView = Instantiate(_characterView, _parent.transform);
            characterView.Initialize(character.Icon, character.Price);
        }
    }

    private void TrySell()
    {

    }
}
