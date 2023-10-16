using UnityEngine;
using TMPro;

public class Wallet : MonoBehaviour, IDataReader, IDataWriter, IInitializable
{
    [SerializeField] private int _stars;
    [SerializeField] private WalletView _walletView;

    public int Stars => _stars;

    public void Initialize()
    {
        _walletView.UpdateText(_stars);
    }

    public void DicreaseMoney(int price)
    {
        if (price < 0)
            return;

        _stars -= price;
        _walletView.UpdateText(_stars);
    }

    public void OnEndEdit(string text)
    {
        if (int.TryParse(text, out int stars))
        {
            _stars = stars;
            _walletView.UpdateText(stars);
        }
    }

    public void Read(PlayerData playerData)
    {
        _stars = playerData.Stars;
    }

    public void Write(PlayerData playerData)
    {
        playerData.Stars = _stars;
    }
}
