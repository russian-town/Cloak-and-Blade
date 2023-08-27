using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Cell _startCell;
    private Cell _wayPoint;
    private Player _player;

    private void OnDisable()
    {
        _player.StepEnded -= OnStepEnded;
    }

    public void Initialize(Cell startCell, Player player)
    {
        _wayPoint = startCell;
        _startCell = startCell;
        _player = player;
        _player.StepEnded += OnStepEnded;
    }

    private void OnStepEnded()
    {
        if (_startCell.NextOnPath == null)
            _startCell = _wayPoint;

        StartCoroutine(StartMove());
    }

    private IEnumerator StartMove()
    {
        while(transform.localPosition != _startCell.NextOnPath.transform.localPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _startCell.NextOnPath.transform.localPosition, Time.deltaTime * _speed);
            yield return null;
        }

        _startCell = _startCell.NextOnPath;
    }
}
