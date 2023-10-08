using TMPro;
using UnityEngine;

public class WalletView : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyText;

    public void UpdateText(int money) => _moneyText.text = money.ToString();
}
