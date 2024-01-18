using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : LevelExit
{
    [SerializeField] private GameObject _linza;
    [SerializeField] private Camera _camera;
    [SerializeField] private Heart _heart;

    private Coroutine _startFinalCutSceneCoroutine;

    public bool IsActive { get; private set; }

    public override bool RequiredItemFound()
    {
        if(_heart.IsActive)
            return true;

        return false;
    }

    public override bool TryOpen()
    {
        if (IsActive)
            return false;

        if (_heart.IsActive)
        {
            IsActive = true;
            _linza.SetActive(true);
            return true;
        }

        return false;
    }

    private IEnumerator FinalCutsceneCoroutine()
    {
        yield return null;
    }
}
