using System.Collections;

public class TheWorldCommand : Command
{
    private TheWorld _theWorld;

    public TheWorldCommand(TheWorld theWorld)
    {
        _theWorld = theWorld;
        _theWorld.Initialize();
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
