using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : LevelExit
{
    [SerializeField] private GameObject _linza;
    //[SerializeField] private InteractiveObjectView _view;
    [SerializeField] private Camera _camera;
    [SerializeField] private Heart _heart;

    private Coroutine _startFinalCutSceneCoroutine;

    public bool IsActive { get; private set; }

    public override void Interact()
    {
        //if (IsActive)
        //    return;

        //IsActive = true;
        //_linza.SetActive(true);
    }

    public override void Prepare()
    {
        //if (CheckInteractionPossibility())
        //{
        //    if(_heart.IsActive)
        //    {
        //        if (_startFinalCutSceneCoroutine != null)
        //            return;

        //        _startFinalCutSceneCoroutine = StartCoroutine(FinalCutsceneCoroutine());
        //    }
        //}
    }

    protected override void Disable()
    {
        //throw new System.NotImplementedException();
    }

    private IEnumerator FinalCutsceneCoroutine()
    {
        yield return null;
    }
}
