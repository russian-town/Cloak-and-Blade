using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialZone : InteractiveObject
{
    [SerializeField] private ScreenAnimationHandler _view;
    [SerializeField] private PlayerView _playerUI;
    [SerializeField] private Button _closeButton;

    private bool _isZonePassed;

    public event UnityAction ZonePassed;

    private void Start()
    {
        _closeButton.onClick.AddListener(() => Interact());
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(() => Interact());
    }

    public override void Prepare()
    {
        if (!CheckInteractionPossibility() || _isZonePassed)
            return;

        _view.FadeIn();
        _playerUI.Hide();
        _isZonePassed = true;
    }

    public override void Interact()
    {
        _view.FadeOut();
        _playerUI.Show();
        _closeButton.onClick.RemoveListener(() => Interact());
        ZonePassed?.Invoke();
    }

    protected override void Disable()
    {

    }
}
