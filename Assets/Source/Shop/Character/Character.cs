using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "New character", order = 51)]
public class Character : ScriptableObject, IDataReader, IDataWriter
{
    [SerializeField] private Type _type;
    [SerializeField] private int _price;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Player _player;
    [SerializeField] private MenuModel _menuPlayerModelTemplate;
    [SerializeField] private bool _isBought;
    [SerializeField] private bool _isSelect;
    [SerializeField] private Description _description;
    [SerializeField] private UpgradeSetter _upgradeSetter;

    public Type Type => _type;
    public int Price => _price;
    public Sprite Icon => _icon;
    public bool IsBought => _isBought;
    public bool IsSelect => _isSelect;
    public Player Player => _player;
    public MenuModel MenuModelTemplate => _menuPlayerModelTemplate;
    public Description Description => _description;
    public UpgradeSetter UpgradeSetter => _upgradeSetter;

    public void Buy() => _isBought = true;

    public void Write(PlayerData playerData)
    {
        if (playerData.Characters.Contains(this))
        {
            int index = playerData.Characters.IndexOf(this);
            playerData.IsBought[index] = _isBought;
            playerData.IsSelect[index] = _isSelect;
        }
        else
        {
            playerData.Characters.Add(this);
            playerData.IsBought.Add(_isBought);
            playerData.IsSelect.Add(_isSelect);
        }
    }

    public void Read(PlayerData playerData)
    {
        if (playerData.Characters.Contains(this))
        {
            int index = playerData.Characters.IndexOf(this);
            _isBought = playerData.IsBought[index];
            _isSelect = playerData.IsSelect[index];
        }
    }

    public void Select() => _isSelect = true;

    public void UnSelect() => _isSelect = false;
}

public enum Type
{
    None,
    Default
}
