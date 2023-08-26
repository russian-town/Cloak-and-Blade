using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private Gameboard _gameboard;
    [SerializeField] private Vector2Int _size;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _gameboard.Initialize(_size);
    }
}
