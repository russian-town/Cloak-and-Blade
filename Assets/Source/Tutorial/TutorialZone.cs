using UnityEngine;
using UnityEngine.UI;

public class TutorialZone : InteractiveObject
{
    [SerializeField] private Canvas _view;
    [SerializeField] private Canvas _playerUI;
    [SerializeField] private Button _closeButton;

    private bool _isZonePassed;

    private void Start()
    {
        _view.gameObject.SetActive(false);
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

        _view.gameObject.SetActive(true);
        _playerUI.gameObject.SetActive(false);
        _isZonePassed = true;
    }

    public override void Interact()
    {
        _view.gameObject.SetActive(false);
        _playerUI.gameObject.SetActive(true);
        _closeButton.onClick.RemoveListener(() => Interact());
    }

    protected override void Disable()
    {

    }
}
