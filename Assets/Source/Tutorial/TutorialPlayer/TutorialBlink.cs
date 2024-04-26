using Source.Player.Abiilities;
using Source.Tutorial.TutorialZones;

namespace Source.Tutorial.TutorialPlayer
{
    public class TutorialBlink : Blink
    {
        private Side _side;

        public void SetSide(Side side) => _side = side;

        public override void RefillNavigatorCells()
        {
            if (_side == Side.East)
                Navigator.RefillEastAvailableCells(Player.CurrentCell, BlinkRange);
            else
                Navigator.RefillWestAvailableCells(Player.CurrentCell, BlinkRange);
        }
    }
}
