using System.Collections;
using UnityEngine;

public class AbilityCommand : Command
{
    private Ability _ability;

    public AbilityCommand(Ability ability)
    {
        _ability = ability;
    }

    public override void Prepare()
    {
        _ability.Prepare();
    }

    public override void Cancel()
    {
        _ability.Cancel();
    }

    protected override IEnumerator Action(Cell clickedCell)
    {
        yield return new WaitUntil(() => _ability.Cast(clickedCell));
    }
}
