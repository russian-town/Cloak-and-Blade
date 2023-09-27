using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BearTrap : InteractiveObject
{
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
    }

    public override void Prepare()
    {
        if (CheckInteractionPossibility())
        {
            _animator.SetTrigger(Constants.TriggeredParameter);
            Interact();
        }
    }
}
