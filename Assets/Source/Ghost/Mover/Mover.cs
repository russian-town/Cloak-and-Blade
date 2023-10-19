using System.Collections;
using UnityEngine;

public abstract class Mover : MonoBehaviour, IPauseHandler
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    private GhostAnimationHandler _animationHandler;
    private Cell _startCell;
    private float _pauseSpeed = 1;

    public Coroutine StartMoveCoroutine { get; private set; }
    public Cell CurrentCell { get; private set; }

    public void Initialize(Cell startCell, GhostAnimationHandler animationHandler)
    {
        _startCell = startCell;
        CurrentCell = _startCell;
        _animationHandler = animationHandler;
    }

    public void Move(Cell targetCell, float moveSpeed, float rotationSpeed)
    {
        if (StartMoveCoroutine == null)
            StartMoveCoroutine = StartCoroutine(StartMoveTo(targetCell, moveSpeed, rotationSpeed));
    }

    public void SetPause(bool isPause) => _pauseSpeed = isPause ? 0 : 1;

    public IEnumerator Rotate(Cell targetCell, float rotationSpeed)
    {
        Vector3 rotationTarget = targetCell.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(rotationTarget, Vector3.up);

        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * _pauseSpeed * Time.deltaTime);
            yield return null;
        }
    }

    protected virtual IEnumerator StartMoveTo(Cell targetCell, float moveSpeed, float rotationSpeed)
    {
        yield return StartCoroutine(Rotate(targetCell, rotationSpeed));
        _animationHandler.PlayFlyAnimation();

        while (transform.localPosition != targetCell.transform.localPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetCell.transform.localPosition, Time.deltaTime * _pauseSpeed * moveSpeed);
            yield return null;
        }

        _animationHandler.StopFlyAnimation();

        CurrentCell = targetCell;
        _startCell = targetCell;
        StartMoveCoroutine = null;
    }
}
