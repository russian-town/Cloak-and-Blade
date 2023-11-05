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
        {
            SetFocused();
        }
        else
        {
            SetUnfocused();
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus == false)
        {
            SetFocused();
        }
        else
        {
            SetUnfocused();
        }
    }

    private void SetFocused()
    {
        if (enabled == false)
            return;

        if (_game.IsInitialize == false || _audio == null)
            return;

        _game.SetPause();
        _audio.Mute();
    }

    private void SetUnfocused() 
    {
        if (enabled == false)
            return;

        if (_game.IsInitialize == false || _audio == null)
            return;

        _game.Continue();
        _audio.UnMute();
    }
}
