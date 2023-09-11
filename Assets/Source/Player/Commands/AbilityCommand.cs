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

    public override void Execute(Cell clickedCell)
    {
        IsExecuting = true;
        _ability.Cast(clickedCell);
    }

    public override void Cancel()
    {
        IsExecuting = false;
        _ability.Cancel();
    }
}
