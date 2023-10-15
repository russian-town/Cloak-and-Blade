using UnityEngine;

public class Wallet : MonoBehaviour, IDataReader, IDataWriter
{
    [SerializeField] private int _stars;
    [SerializeField] private WalletView _walletView;

    public int Stars => _stars;

    private void Start()
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

    public void Read(PlayerData playerData)
    {
        _stars = playerData.Stars;
    }

    public void Write(PlayerData playerData)
    {
        playerData.Stars = _stars;
    }
}
