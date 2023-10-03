using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "New character", order = 51)]
public class Character : ScriptableObject
{
    private int _price;
    private Sprite _icon;

    public int Price => _price;
    public Sprite Icon => _icon;
}
