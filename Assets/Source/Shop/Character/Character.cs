using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "New character", order = 51)]
public class Character : ScriptableObject
{
    [SerializeField] private int _price;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Player _player;

    private bool _isBuyed;

    public int Price => _price;
    public Sprite Icon => _icon;
    public bool IsBuyed => _isBuyed;
    public Player Player => _player;

    public void Buy() => _isBuyed = true;
}
