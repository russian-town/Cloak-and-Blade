using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSetter : MonoBehaviour
{
    [SerializeField] private Texture2D _cursorPic;
    [SerializeField] private Vector2 _hotPoint;

    private void Start()
    {
        Cursor.SetCursor(_cursorPic, _hotPoint, CursorMode.Auto);
    }
}
