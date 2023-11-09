using UnityEngine;
using Agava.YandexGames;
using Agava.WebUtility;
using System;

public class FocusHandler : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Audio _audio;

    private void OnEnable()
    {
        WebApplication.InBackgroundChangeEvent += OnBackgroundChangeEvent;
    }

    private void OnDisable()
    {
        WebApplication.InBackgroundChangeEvent -= OnBackgroundChangeEvent;
    }

    private void OnBackgroundChangeEvent(bool poop)
    {
        if (poop)
            SetUnFocused();
        else
            SetFocused();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus == false)
            SetUnFocused();
        else
            SetFocused();
    }

    private void SetUnFocused()
    {
        if (enabled == false)
            return;

        if (_game.IsInitialize == false || _audio == null)
            return;

        _game.SetPause();
        _audio.Mute();
    }

    private void SetFocused() 
    {
        if (enabled == false)
            return;

        if (_game.IsInitialize == false || _audio == null)
            return;

        _audio.UnMute();
    }
}
