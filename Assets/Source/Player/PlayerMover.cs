using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    private PlayerAnimationHandler _handler;
    private Cell _startCell;

    public Coroutine StartMoveCoroutine { get; private set; }
    public Cell CurrentCell { get; private set; }

    public event UnityAction MoveEnded;

    public void Initialize(Cell startCell, PlayerAnimationHandler animationHandler)
    {
        _startCell = startCell;
        CurrentCell = _startCell;
        _handler = animationHandler;
    }

    public void Move(Cell targetCell)
    {
        if (StartMoveCoroutine == null)
            StartMoveCoroutine = StartCoroutine(StartMoveTo(targetCell));
    }

    public IEnumerator StartMoveTo(Cell targetCell)
    {
        Vector3 rotationTarget = targetCell.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(rotationTarget, Vector3.up);

        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            yield return null;
        }

        _handler.PlayFlyAnimation();

        while (transform.localPosition != targetCell.transform.localPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetCell.transform.localPosition, Time.deltaTime * _speed);
            yield return null;
        }

        _handler.StopFlyAnimation();

        CurrentCell = targetCell;
        _startCell = targetCell;
        StartMoveCoroutine = null;
        MoveEnded?.Invoke();
    }
}
