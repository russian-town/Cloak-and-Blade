using System.Collections;
using UnityEngine;

public class AbilityCommand : Command
{
    private readonly Ability _ability;

    public Ability Ability => _ability;

    public AbilityCommand(Ability ability)
    {
        _ability = ability;
    }

    protected override IEnumerator PrepareAction()
    {
        _ability.Prepare();
        yield return null;
    }

    public override void Cancel()
    {
        _ability.Cancel();
    }

    protected override IEnumerator ExecuteAction(Cell clickedCell)
    {
        yield return new WaitUntil(() => _ability.Cast(clickedCell));
    }
}
