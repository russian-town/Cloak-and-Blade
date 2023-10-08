using System;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class TutorialButtonHider : MonoBehaviour
{
    [SerializeField] private TutorialZone _tutorialZone;

    private CanvasGroup _canvasGroup;

    private void OnEnable()
    {
        _tutorialZone.ZonePassed += OnZonePassed;
    }

    private void OnDisable()
    {
        _tutorialZone.ZonePassed -= OnZonePassed;
    }

    public void Hide() 
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }

    private void Show()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }

    private void OnZonePassed()
    {
        Show();
    }
}
