using UnityEngine;

public class Screamer : InteractiveObject
{
    [SerializeField] private AudioSource _source;

    private bool _interactable = true;

    public override void Interact()
    {
        _source.Play();
        _interactable = false;
    }

    public override void Prepare()
    {
        if (CheckInteractionPossibility() && _interactable)
            Interact();
    }

    protected override void Disable()
    {}
}
