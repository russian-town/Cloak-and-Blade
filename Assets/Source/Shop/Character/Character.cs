using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "New character", order = 51)]
public class Character : ScriptableObject
{
    [SerializeField] private Type _type;
    [SerializeField] private int _price;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Player _player;
    [SerializeField] private MenuModel _menuPlayerModelTemplate;

    [SerializeField] private bool _isBought;
    [SerializeField] private bool _isSelect;

    private MenuModel _menuModel;

    public Type Type => _type;
    public int Price => _price;
    public Sprite Icon => _icon;
    public bool IsBuyed => _isBought;
    public bool IsSelect => _isSelect;
    public Player Player => _player;
    public MenuModel MenuModelTemplate => _menuPlayerModelTemplate;

    public void Buy() => _isBought = true;

    public void Select() => _isSelect = true;

    public void UnSelect() => _isSelect = false;
}

public enum Type
{
    None,
    Default
}
