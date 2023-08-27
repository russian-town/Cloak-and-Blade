using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Cell _startCell;

    public event UnityAction MoveEnded;

    public void Initialize(Cell startCell)
    {
        _startCell = startCell;
    }

    public void Move(Cell targetCell)
    {
        if (targetCell == _startCell.East || targetCell == _startCell.West || targetCell == _startCell.North || targetCell == _startCell.South)
        {
            StartCoroutine(StartMoveTo(targetCell));
        }
    }

    private IEnumerator StartMoveTo(Cell targetCell)
    {
        while(transform.localPosition != targetCell.transform.localPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetCell.transform.localPosition, Time.deltaTime * _speed);
            yield return null;
        }

        _startCell = targetCell;
        MoveEnded?.Invoke();
    }
}
