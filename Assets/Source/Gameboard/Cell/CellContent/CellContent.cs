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
}

public enum CellContentType
{
    Empty,
    Destination,
    Wall
}
