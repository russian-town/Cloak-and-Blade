public class AbilityCommand : Command
{
    private Ability _ability;

    public void Initialize(Ability ability)
    {
        _ability = ability;
    }

    public override void Prepare()
    {
        _ability.Prepare();
    }

    public override void Execute()
    {
        _ability.Cast();
    }

    public override void Cancel()
    {
        _ability.Cancel();
    }
}
