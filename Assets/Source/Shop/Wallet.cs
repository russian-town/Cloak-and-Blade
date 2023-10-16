using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using TMPro;

public class Wallet : MonoBehaviour, IDataReader, IDataWriter
{
    [SerializeField] private int _stars;
    [SerializeField] private WalletView _walletView;
    [SerializeField] private TMP_InputField _inputField;

    public int Stars => _stars;

    private void OnDisable()
    {
        _inputField.onEndEdit.RemoveListener(OnEndEdit);
    }

    public void Initialize()
    {
        _walletView.UpdateText(_stars);
        _inputField.onEndEdit.AddListener(OnEndEdit);
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
