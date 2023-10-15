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

    public Type Type => _type;
    public int Price => _price;
    public Sprite Icon => _icon;
    public bool IsBought => _isBought;
    public bool IsSelect => _isSelect;
    public Player Player => _player;
    public MenuModel MenuModelTemplate => _menuPlayerModelTemplate;

    public void Buy() => _isBought = true;

    public void Select() => _isSelect = true;

    public void UnSelect() => _isSelect = false;

    public void Read(PlayerData playerData)
    {
        //if (playerData.IsBought.ContainsKey(this))
        //{
        //    _isBought = playerData.IsBought[this];
        //    Debug.Log(_isBought);
        //}

        //if (playerData.IsSelect.ContainsKey(this))
        //    _isSelect = playerData.IsSelect[this];
    }

    public void Write(PlayerData playerData)
    {
        //if (playerData.IsBought.ContainsKey(this))
        //    playerData.IsBought[this] = _isBought;
        //else
        //    playerData.IsBought.Add(this, _isBought);

        //if (playerData.IsSelect.ContainsKey(this))
        //    playerData.IsSelect[this] = _isSelect;
        //else
        //    playerData.IsSelect.Add(this, _isSelect);
    }
}

public enum Type
{
    None,
    Default
}
