using System.Collections.Generic;
using UnityEngine;

public class Upgrader : MonoBehaviour
{
    private Wallet _wallet;
    private List<Description> _descriptions = new List<Description>();
    private Description _description;

    private void OnDisable()
    {
        foreach (var description in _descriptions)
            description.UpgradeButtonClicked -= OnUpgradeButtonClicked;
    }

    public void Initialize(Description description)
    {
        _description = description;
        _descriptions.Add(_description);
        _description.UpgradeButtonClicked += OnUpgradeButtonClicked;
    }

    public void OnUpgradeButtonClicked(UpgradeSetter upgradeSetter, Description description)
    {
        if (upgradeSetter.Level == Constants.MaxLevel)
            return;

        if (_wallet.Stars >= upgradeSetter.Prices[upgradeSetter.Level])
        {
            _wallet.DicreaseStars(upgradeSetter.Prices[upgradeSetter.Level]);
            upgradeSetter.Upgade();
            description.UpdateView();
        }
    }

    public void SetWallet(Wallet wallet) => _wallet = wallet;
}
