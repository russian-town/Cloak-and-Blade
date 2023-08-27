using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Cell _startCell;
    private Cell _wayPoint;

    public void Initialize(Cell startCell)
    {
        _wayPoint = startCell;
        _startCell = startCell;
    }
}
