using System.Collections;
using UnityEngine;
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
    [SerializeField] private Gameboard _board;
    [SerializeField] private Sebastian _sebastian;
    [SerializeField] private Image _dialogueSkipIcon;
    [SerializeField] private Sprite _fastForwardImage;
    [SerializeField] private Sprite _nextImage;

    private Coroutine _dialogueCoroutine;
    private WaitForSeconds _waitDelay;
    private WaitForSeconds _waitStartDelay;
    private AudioSource _source;
    private TutorialZone _currentTutorialZone;
    private TutorialText _currentTutorialText;
    private float _baseVolume;
    private bool _isWritten = false;

    private void OnEnable()
    {
        _nextLineButton.onClick.AddListener(OnNextLineButtonClick);
    }

    private void OnDisable()
    {
        _nextLineButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        _waitDelay = new WaitForSeconds(_baseDelay);
        _waitStartDelay = new WaitForSeconds(_startDelay);
        _dialogueSkipIcon.gameObject.SetActive(false);
        _source = GetComponent<AudioSource>();
        _baseVolume = _source.volume;
        _board.Disable();
    }
     
    public void WriteDialogue(TutorialZone tutorialZone)
    {
        if(_dialogueCoroutine != null)
        {
            StopCoroutine(_dialogueCoroutine);
            _dialogueCoroutine = null;
        }

        _isWritten = false;
        _dialogueSkipIcon.enabled = true;
        _sebastian.Show();
        WipeText();
        _currentTutorialZone = tutorialZone;
        _currentTutorialText = _currentTutorialZone.GetText();
        _dialogueCoroutine = StartCoroutine(WriteLine());
        _board.Disable();

        foreach (var mainButton in _mainButtons)
            mainButton.Hide();
    }

    public void WriteCongratDialogue()
    {
        foreach (var mainButton in _mainButtons)
            mainButton.Hide();

        _nextLineButton.onClick.RemoveListener(OnNextLineButtonClick);
        _nextLineButton.onClick.AddListener(SkipCongratText);
        _currentTutorialText = _currentTutorialZone.GetNextCongratText();

        if (_dialogueCoroutine != null)
        {
            StopCoroutine(_dialogueCoroutine);
            _dialogueCoroutine = null;
        }

        _isWritten = false;
        _dialogueSkipIcon.enabled = true;
        _sebastian.Show();
        WipeText();
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
            _currentTutorialZone.Element.Show(_currentTutorialZone.Player);
            _dialogueSkipIcon.enabled = false;
            return true;
        }

        return false;
    }

    private IEnumerator WriteLine()
    {
        _dialogueSkipIcon.sprite = _fastForwardImage;
        WipeText();
        _nextLineButton.interactable = false;
        yield return _waitStartDelay;
        _nextLineButton.interactable = true;
        _dialogueSkipIcon.gameObject.SetActive(true);
        _source.volume = _baseVolume;

        for (int i = 0; i < _currentTutorialText.Line.Length; i++)
        {
            _source.pitch = Random.Range(_minVoicePitch, _maxVoicePitch);

            if (!char.IsPunctuation(_currentTutorialText.Line[i]) && !char.IsWhiteSpace(_currentTutorialText.Line[i]))
                _source.PlayOneShot(_voiceSampleBeep);

            _textContainer.text += _currentTutorialText.Line[i];
            yield return _waitDelay;
        }

        CheckIfTextIsTrigger();
        _dialogueSkipIcon.sprite = _nextImage;
        _waitDelay = new WaitForSeconds(_baseDelay);
        _isWritten = true;
        _dialogueCoroutine = null;
    }

    private void WipeText()
    {
        if (_textContainer.text != null)
            _textContainer.text = null;
    }

    private bool TrySpeedUpText()
    {
        if (_textContainer.text != _currentTutorialText.Line)
        {
            if (_dialogueCoroutine != null)
            {
                _source.volume = 0f;
                _waitDelay = new WaitForSeconds(_fastDelay);
                return true;
            }
        }

        return false;
    }

    private void OnNextLineButtonClick()
    {
        if (TrySpeedUpText())
            return;

        if (_currentTutorialZone == null)
            return;

        TutorialText tutorialText = _currentTutorialZone.GetNextText();

        if (tutorialText != null)
        {
            _dialogueSkipIcon.gameObject.SetActive(false);
            _currentTutorialText = tutorialText;
            WriteNextDialogue();
        }
        else if(tutorialText == null && _currentTutorialText.IsTutorialTrigger == false)
        {
            _sebastian.Hide();

            foreach (var mainButton in _mainButtons)
                if (mainButton.IsOpen)
                    mainButton.Show();

            _board.Enable();
        }
    }

    private void SkipCongratText()
    {
        if (TrySpeedUpText())
            return;

        if (_currentTutorialZone == null)
            return;

        TutorialText congratText = _currentTutorialZone.GetNextCongratText();

        if (congratText != null)
        {
            _dialogueSkipIcon.gameObject.SetActive(false);
            _currentTutorialText = congratText;
            WriteNextDialogue();
            return;
        }

        if (_isWritten == false)
            return;

        _nextLineButton.onClick.RemoveListener(SkipCongratText);
        _nextLineButton.onClick.AddListener(OnNextLineButtonClick);
        _sebastian.Hide();

        foreach (var mainButton in _mainButtons)
            if (mainButton.IsOpen)
                mainButton.Show();

        _board.Enable();
    }
}
