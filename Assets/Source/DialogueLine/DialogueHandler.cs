using System.Collections;
using UnityEngine;
using System;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class DialogueHandler : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _textContainer;
    [SerializeField] private float _delay;
    [SerializeField] private float _startDelay;
    [SerializeField] private float _minVoicePitch;
    [SerializeField] private float _maxVoicePitch;
    [SerializeField] private AudioClip _voiceSampleBeep;

    private Coroutine _dialogueCoroutine;
    private WaitForSeconds _waitDelay;
    private WaitForSeconds _waitStartDelay;
    private AudioSource _source;

    private void Start()
    {
        _waitDelay = new WaitForSeconds(_delay);
        _waitStartDelay = new WaitForSeconds(_startDelay);
        _source = GetComponent<AudioSource>();
    }
     
    public void WriteDialogue(TutorialZone tutorialZone)
    {
        if(_dialogueCoroutine != null)
        {
            StopCoroutine(_dialogueCoroutine);
            _dialogueCoroutine = null;
        }

        WipeText();
        _dialogueCoroutine = StartCoroutine(WriteLine(tutorialZone));
    }

    private IEnumerator WriteLine(TutorialZone tutorialZone)
    {
        foreach(var referenceText in tutorialZone.ReferenceTexts)
        {
            yield return _waitStartDelay;
            WipeText();

            if (referenceText.IsTutorialTrigger)
                tutorialZone.Element.Show();

            for (int i = 0; i < referenceText.Line.Length; i++)
            {
                _source.pitch = Random.Range(_minVoicePitch, _maxVoicePitch);

                if (!char.IsPunctuation(referenceText.Line[i]) && !char.IsWhiteSpace(referenceText.Line[i]))
                    _source.PlayOneShot(_voiceSampleBeep);

                _textContainer.text += referenceText.Line[i];
                yield return _waitDelay;
            }

            _dialogueCoroutine = null;
        }
    }

    private void WipeText()
    {
        if (_textContainer.text != null)
            _textContainer.text = null;
    }
}
