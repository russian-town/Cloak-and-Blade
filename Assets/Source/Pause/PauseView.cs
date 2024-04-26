using System;
using Source.Player.PlayerUI;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Pause
{
    public class PauseView : ViewPanel
    {
        [SerializeField] private Button _continueButton;

        private Pause _pause;

        public event Action ContionueButtonClicked;

        public void Initialize(Pause pause)
        {
            _pause = pause;
            _pause.Enabled += OnEnabled;
            _pause.Disabled += OnDisabled;
        }

        public override void Subscribe()
        {
            base.Subscribe();
            _continueButton.onClick.AddListener(() => ContionueButtonClicked?.Invoke());
        }

        protected override void Unsubscribe()
        {
            base.Unsubscribe();
            _continueButton.onClick.RemoveListener(() => ContionueButtonClicked?.Invoke());
            _pause.Enabled -= OnEnabled;
            _pause.Disabled -= OnDisabled;
        }

        private void OnDisabled()
            => Hide();

        private void OnEnabled()
            => Show();
    }
}
