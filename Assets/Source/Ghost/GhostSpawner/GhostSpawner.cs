using Source.Gameboard.Cell;
using UnityEngine;

namespace Source.Ghost.GhostSpawner
{
    public class GhostSpawner : MonoBehaviour
    {
        public Ghost Get(Cell spawnCell, Ghost template)
        {
            Ghost ghost = Instantiate(template);
            ghost.transform.localPosition = spawnCell.transform.localPosition;
            return ghost;
        }
    }
}
