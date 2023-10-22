using System.Collections;

public class AbilityCommand : Command
{
    private Ability _ability;

    public bool IsUsed => _ability.CanUse() == false;

    public AbilityCommand(Ability ability)
    {
        _ability = ability;
    }

    public override IEnumerator WaitOfExecute()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator ExecuteAction(Cell clickedCell)
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator PrepareAction()
    {
        throw new System.NotImplementedException();
    }
}
