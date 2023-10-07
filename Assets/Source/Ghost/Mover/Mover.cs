using System.Collections;
using UnityEngine;

public abstract class Mover : MonoBehaviour, IPauseHandler
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    private float _currentMoveSpeed;
    private float _currentRotationSpeed;
    private GhostAnimationHandler _animationHandler;
    private Cell _startCell;
    private float _startMoveSpeed;
    private float _startRotationSpeed;
    private bool _isPaused;

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

    public void SetPause(bool isPause)
    {
        _isPaused = isPause;
    }

    protected virtual IEnumerator StartMoveTo(Cell targetCell, float moveSpeed, float rotationSpeed)
    {
        Vector3 rotationTarget = targetCell.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(rotationTarget, Vector3.up);

        while (transform.rotation != targetRotation)
        {
            yield return new WaitUntil(() => _isPaused == false);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        _animationHandler.PlayFlyAnimation();

        while (transform.localPosition != targetCell.transform.localPosition)
        {
            yield return new WaitUntil(() => _isPaused == false);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetCell.transform.localPosition, Time.deltaTime * moveSpeed);
            yield return null;
        }

        _animationHandler.StopFlyAnimation();

        CurrentCell = targetCell;
        _startCell = targetCell;
        StartMoveCoroutine = null;
    }
}
