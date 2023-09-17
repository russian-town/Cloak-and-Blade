using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Mover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    private GhostAnimationHandler _animationHandler;
    private Cell _startCell;

    public Coroutine StartMoveCoroutine { get; private set; }
    public Cell CurrentCell { get; private set; }

    public void Initialize(Cell startCell, GhostAnimationHandler animationHandler)
    {
        _startCell = startCell;
        CurrentCell = _startCell;
        _animationHandler = animationHandler;
    }

    public void Move(Cell targetCell)
    {
        if (StartMoveCoroutine == null)
            StartMoveCoroutine = StartCoroutine(StartMoveTo(targetCell));
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
