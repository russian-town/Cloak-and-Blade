using System.Collections;
using UnityEngine;

public abstract class Mover : MonoBehaviour, IPauseHandler
{
    private GhostAnimationHandler _animationHandler;
    private Cell _startCell;
    private float _pauseSpeed = 1;
    private Coroutine _movingCoroutine;
    private Coroutine _rotatingCoroutine;

    public Cell CurrentCell { get; private set; }

    public void Initialize(Cell startCell, GhostAnimationHandler animationHandler)
    {
        _startCell = startCell;
        CurrentCell = _startCell;
        _animationHandler = animationHandler;
    }

    public void SetPause(bool isPause)
        => _pauseSpeed = isPause ? 0 : 1;

    public Coroutine StartRotate(Cell targetCell, float rotationSpeed)
    {
        if (_rotatingCoroutine != null)
            StopCoroutine(_rotatingCoroutine);

        return _rotatingCoroutine = StartCoroutine(Rotate(targetCell, rotationSpeed));
    }

    public Coroutine StartMoveTo(Cell targetCell, float moveSpeed, float rotationSpeed)
    {
        if (_movingCoroutine != null)
            StopCoroutine(_movingCoroutine);

        return _movingCoroutine =StartCoroutine(MoveTo(targetCell, moveSpeed, rotationSpeed));
    }

    protected virtual IEnumerator MoveTo(Cell targetCell, float moveSpeed, float rotationSpeed)
    {
        yield return StartRotate(targetCell, rotationSpeed);
        _animationHandler.PlayFlyAnimation();

        while (transform.localPosition != targetCell.transform.localPosition)
        {
            transform.localPosition = Vector3.MoveTowards
                (
                transform.localPosition,
                targetCell.transform.localPosition,
                Time.deltaTime * _pauseSpeed * moveSpeed
                );
            yield return null;
        }

        _animationHandler.StopFlyAnimation();
        CurrentCell = targetCell;
        _startCell = targetCell;
        _movingCoroutine = null;
    }

    private IEnumerator Rotate(Cell targetCell, float rotationSpeed)
    {
        Vector3 targetDirection = new Vector3(targetCell.transform.position.x, 0, targetCell.transform.position.z);
        Vector3 rotationTarget = targetDirection - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(rotationTarget, Vector3.up);

        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards
                (
                transform.rotation,
                targetRotation,
                rotationSpeed * _pauseSpeed * Time.deltaTime
                );
            yield return null;
        }

        _rotatingCoroutine = null;
    }
}
