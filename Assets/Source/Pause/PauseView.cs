using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseView : ViewPanel
{
    [SerializeField] private Button _continueButton;

    public event Action ContionueButtonClicked;

    public override void Initialize()
    {
        base.Initialize();
        _continueButton.onClick.AddListener(() => ContionueButtonClicked?.Invoke());
    }

    protected override void Dismiss()
    {
        base.Dismiss();
        _continueButton.onClick.RemoveListener(() => ContionueButtonClicked?.Invoke());
    }
}
