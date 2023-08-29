using UnityEngine;

public class CellContent : MonoBehaviour
{
    [SerializeField] private CellContentType _type;

    public CellContentType Type => _type;
    public CellContentSpawner Spawner { get; set; }

    public void Recycle()
    {
        Spawner.Reclaim(this);
    }

    public void BecomeWall() => _type = CellContentType.Wall;

    public void BecomeEmpty() => _type = CellContentType.Empty;

    public void BecomeDestination() => _type = CellContentType.Destination;
}

public enum CellContentType
{
    Empty,
    Destination,
    Wall
}
