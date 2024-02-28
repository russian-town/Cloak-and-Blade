using UnityEngine;
using UnityEngine.Events;

public class Wallet : IDataReader, IDataWriter, IInitializable
{
    [SerializeField] private int _stars;

    public int Stars => _stars;

    public event UnityAction<int> StarsChanged;

    public void Initialize()
    {
        _stars = 100;
        StarsChanged?.Invoke(_stars);
    }

    public void DicreaseStars(int price)
    {
        if (price < 0)
            return;

        _stars -= price;
        StarsChanged?.Invoke(_stars);
    }

    public void AddStars(int stars) 
    {
        if (stars < 0)
            return;

        _stars += stars;
        StarsChanged?.Invoke(_stars);
    }

    public void Read(PlayerData playerData) => _stars = playerData.Stars;

    public void Write(PlayerData playerData) => playerData.Stars = _stars;
}
