using System.Collections;

public class AbilityCommand : Command
{
    private Ability _ability;
    
    public bool IsUsed => _ability.CanUse() == false;

    public AbilityCommand(Ability ability, CommandExecuter executer) : base(executer)
    {
        _ability = ability;
    }

    protected override IEnumerator WaitOfExecute()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator ExecuteAction()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator PrepareAction()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnCommandChanged(Command command)
    {
        Cancel();
    }

    protected override void Cancel()
    {
        base.Cancel();
    }
}
