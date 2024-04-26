using System;
using Source.Saves;
using UnityEngine;

namespace Source.Shop
{
    public class Wallet : IDataReader, IDataWriter, IInitializable
    {
        [SerializeField] private int _stars;

        public event Action<int> StarsChanged;

        public int Stars => _stars;

        public void Initialize()
        {
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
}
