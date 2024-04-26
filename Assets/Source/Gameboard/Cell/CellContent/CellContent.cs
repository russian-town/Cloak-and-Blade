using UnityEngine;

namespace Source.Gameboard.Cell.CellContent
{
    public class CellContent : MonoBehaviour
    {
        [SerializeField] private CellContentType _type;

        public CellContentType Type => _type;

        public CellContentSpawner.CellContentSpawner Spawner { get; set; }

        public void Recycle()
            => Spawner.Reclaim(this);

        public void BecomeWall()
            => _type = CellContentType.Wall;

        public void BecomeEmpty()
            => _type = CellContentType.Empty;
    }
}
