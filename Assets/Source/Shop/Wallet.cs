using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private int _money;
    [SerializeField] private WalletView _walletView;

    public int Money => _money;

    private void Start()
    {
        _walletView.UpdateText(_money);
    }

    public void DicreaseMoney(int price)
    {
        if (price > 0)
            _money -= price;

        _walletView.UpdateText(_money);
    }
}
