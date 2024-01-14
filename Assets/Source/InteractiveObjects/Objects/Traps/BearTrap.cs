using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BearTrap : InteractiveObject
{
    [SerializeField] private AudioSource _source;

    private Animator _animator;

    private Cell TrappedCell => CellsInInteractibleRange[0];

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        _animator = GetComponent<Animator>();
        TrappedCell.BecomeTrap();
    }

    public override void Interact()
    {
        Player.Die();
        _source.Play();
    }

    public override void Prepare()
    {
        if (CheckInteractionPossibility())
        {
            _animator.SetTrigger(Constants.TriggeredParameter);
            Interact();
        }
    }

    protected override void Disable() { }
}
