using System.Collections;
using UnityEngine;

public abstract class Mover : MonoBehaviour, IPauseHandler
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    private GhostAnimationHandler _animationHandler;
    private Cell _startCell;
    private float _startMoveSpeed;
    private float _startRotationSpeed;

    public Coroutine StartMoveCoroutine { get; private set; }
    public Cell CurrentCell { get; private set; }

    public void Initialize(Cell startCell, GhostAnimationHandler animationHandler)
    {
        _startCell = startCell;
        CurrentCell = _startCell;
        _animationHandler = animationHandler;
        _startMoveSpeed = _moveSpeed;
        _startRotationSpeed = _rotationSpeed;
    }

    public void Move(Cell targetCell)
    {
        if (StartMoveCoroutine == null)
            StartMoveCoroutine = StartCoroutine(StartMoveTo(targetCell));
    }

    public void SetPause(bool isPause)
    {
        if (isPause == true)
        {
            _moveSpeed = 0;
            _rotationSpeed = 0;
        }
        else
        {
            _moveSpeed = _startMoveSpeed;
            _rotationSpeed = _startRotationSpeed;
        }

    }

    protected virtual IEnumerator StartMoveTo(Cell targetCell)
    {
        Vector3 rotationTarget = targetCell.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(rotationTarget, Vector3.up);

        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            yield return null;
        }

        _animationHandler.PlayFlyAnimation();

        while (transform.localPosition != targetCell.transform.localPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetCell.transform.localPosition, Time.deltaTime * _moveSpeed);
            yield return null;
        }

        _animationHandler.StopFlyAnimation();

        CurrentCell = targetCell;
        _startCell = targetCell;
        StartMoveCoroutine = null;
    }
}
