using System.Collections.Generic;
using Source.Root;
using Source.Shop;
using UnityEngine;

namespace Source.Upgrader
{
    public class Upgrader : MonoBehaviour
    {
        private Wallet _wallet;
        private List<Description> _descriptions = new List<Description>();
        private Description _description;

        public void Initialize(Description description)
        {
            _description = description;
            _descriptions.Add(_description);
        }

        public void OnUpgradeButtonClicked(UpgradeSetter upgradeSetter, Description description)
        {
            if (upgradeSetter.Level == Constants.MaxLevel)
                return;

            if (_wallet.Stars >= upgradeSetter.Prices[upgradeSetter.Level])
            {
                _wallet.DicreaseStars(upgradeSetter.Prices[upgradeSetter.Level]);
                upgradeSetter.Upgrade();
            }
        }

        public void SetWallet(Wallet wallet) => _wallet = wallet;
    }
}
