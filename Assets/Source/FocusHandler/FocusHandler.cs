using System;
using System.Collections.Generic;
using Agava.WebUtility;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.Events;

public class FocusHandler : MonoBehaviour
{
    [SerializeField] private Audio _audio;
    [SerializeField] private List<TimecodeTracker> _trackers;

    private IActiveScene _scene;

    public event Action<bool> FocusChaned;

    private void OnEnable()
    {
        WebApplication.InBackgroundChangeEvent += OnBackgroundChangeEvent;
        Application.focusChanged += OnFocus;
    }

    private void OnDisable()
    {
        WebApplication.InBackgroundChangeEvent -= OnBackgroundChangeEvent;
        Application.focusChanged -= OnFocus;
    }

    public void SetActiveScene(IActiveScene scene)
        => _scene = scene;

    private void OnBackgroundChangeEvent(bool poop)
    {
        FocusChaned?.Invoke(poop == false);

        if (poop)
            SetUnFocused();
        else
            SetFocused();
    }

    private void OnFocus(bool focus)
    {
        FocusChaned?.Invoke(focus);

        if (focus == false)
            SetUnFocused();
        else
            SetFocused();
    }

    private void SetUnFocused()
    {
        if (enabled == false)
            return;

        if (_audio == null)
            return;

        foreach (var tracker in _trackers)
            tracker.CashTimecode();

        _audio.Mute();

        if (_scene == null)
            return;

        _scene.SetPause();
    }

    private void SetFocused()
    {
        if (enabled == false)
            return;

        if (_audio == null)
            return;

        foreach (var tracker in _trackers)
            tracker.SetTimecode();

        _audio.UnMute();

        if (_scene == null)
            return;

        if (_scene is IAutoContinuer continuer)
            continuer.Continue();
    }
}
