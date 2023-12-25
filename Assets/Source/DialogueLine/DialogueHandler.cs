using System.Collections;
using UnityEngine;
using System;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class DialogueHandler : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _textContainer;
    [SerializeField] private float _baseDelay;
    [SerializeField] private float _fastDelay;
    [SerializeField] private float _startDelay;
    [SerializeField] private float _minVoicePitch;
    [SerializeField] private float _maxVoicePitch;
    [SerializeField] private AudioClip _voiceSampleBeep;
    [SerializeField] private List<MainButton> _mainButtons = new List<MainButton>();
    [SerializeField] private Button _nextLineButton;

    private Coroutine _dialogueCoroutine;
    private WaitForSeconds _waitDelay;
    private WaitForSeconds _waitStartDelay;
    private AudioSource _source;
    private TutorialZone _currentTutorialZone;
    private TutorialText _currentTutorialText;

    private void OnEnable()
    {
        _nextLineButton.onClick.AddListener(OnNextLineButtonClick);
    }

    private void OnDisable()
    {
        _nextLineButton.onClick.RemoveListener(OnNextLineButtonClick);
    }

    private void Start()
    {
        _waitDelay = new WaitForSeconds(_baseDelay);
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
        _currentTutorialZone = tutorialZone;
        _currentTutorialText = _currentTutorialZone.GetText();
        _dialogueCoroutine = StartCoroutine(WriteLine());
    }

    private void WriteNextDialogue()
    {
        if (_dialogueCoroutine != null)
        {
            StopCoroutine(_dialogueCoroutine);
            _dialogueCoroutine = null;
        }

        _dialogueCoroutine = StartCoroutine(WriteLine());
    }

    private bool CheckIfTextIsTrigger()
    {
        if (_currentTutorialText.IsTutorialTrigger)
        {
            _currentTutorialZone.Element.Show();
            return true;
        }

        return false;
    }

    private IEnumerator WriteLine()
    {
        yield return _waitStartDelay;
        WipeText();
        CheckIfTextIsTrigger();

        for (int i = 0; i < _currentTutorialText.Line.Length; i++)
        {
            _source.pitch = Random.Range(_minVoicePitch, _maxVoicePitch);

            if (!char.IsPunctuation(_currentTutorialText.Line[i]) && !char.IsWhiteSpace(_currentTutorialText.Line[i]))
                _source.PlayOneShot(_voiceSampleBeep);

            _textContainer.text += _currentTutorialText.Line[i];
            yield return _waitDelay;
        }

        _waitDelay = new WaitForSeconds(_baseDelay);
        _dialogueCoroutine = null;
    }

    private void WipeText()
    {
        if (_textContainer.text != null)
            _textContainer.text = null;
    }

    private void OnNextLineButtonClick()
    {
        if (_textContainer.text != _currentTutorialText.Line)
        {
            if (_dialogueCoroutine != null)
            {
                _waitDelay = new WaitForSeconds(_fastDelay);
                return;
            }
        }

        if (_currentTutorialZone == null)
            return;

        TutorialText tutorialText = _currentTutorialZone.GetNextText();

        if (tutorialText != null)
        {
            _currentTutorialText = tutorialText;
            WriteNextDialogue();
        }
    }
}
