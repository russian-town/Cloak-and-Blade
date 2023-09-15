using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BearTrap : InteractiveObject
{
    private Animator _animator;

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        _animator = GetComponent<Animator>();
    }

    public override void Interact()
    {
        print("Game Over");
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
