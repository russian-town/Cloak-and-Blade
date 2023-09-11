using System.Collections;

public class SkipCommand : Command
{
    private Player _player;

    public SkipCommand(Player player)
    {
        _player = player;
    }

    public override void Cancel()
    {
    }

    public override void Prepare()
    {
        _player.SkipTurn();
    }

    protected override IEnumerator Action(Cell clickedCell)
    {
        yield return null;
    }
}
