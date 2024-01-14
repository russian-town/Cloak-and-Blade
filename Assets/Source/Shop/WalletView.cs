using TMPro;
using UnityEngine;

public class WalletView : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyText;

    private Wallet _wallet;

    private void OnDisable() => _wallet.StarsChanged -= OnStarsChanged;

    public void Initialize(Wallet wallet)
    {
        _wallet = wallet;
        _wallet.StarsChanged += OnStarsChanged;
    }

    private void UpdateText(int stars) => _moneyText.text = stars.ToString();

    private void OnStarsChanged(int stars) => UpdateText(stars);
}
