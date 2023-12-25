using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogueHandler : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private float _startDelay;
    [SerializeField] private TMPro.TMP_Text _referenceText;
    [SerializeField] private TMPro.TMP_Text _textContainer;
    [SerializeField] private AudioClip _voiceSampleBeep;
    [SerializeField] private float _minVoicePitch;
    [SerializeField] private float _maxVoicePitch;

    private Coroutine _dialogueCoroutine;
    private WaitForSeconds _waitDelay;
    private WaitForSeconds _waitStartDelay;
    private AudioSource _source;
    private char _space = ' ';
    private char _comma = ',';
    private char _dot = '.';
    private char _explanetion = '!';
    private char _question = '?';

    private void Start()
    {
        _waitDelay = new WaitForSeconds(_delay);
        _waitStartDelay = new WaitForSeconds(_startDelay);
        _source = GetComponent<AudioSource>();
    }
     
    public void WriteDialogue()
    {
        if(_dialogueCoroutine != null)
        {
            StopCoroutine(_dialogueCoroutine);
            _dialogueCoroutine = null;
        }

        WipeText();
        _dialogueCoroutine = StartCoroutine(WriteLine());
    }

    private IEnumerator WriteLine()
    {
        yield return _waitStartDelay;

        for(int i = 0; i < _referenceText.text.Length; i++)
        {
            _source.pitch = Random.Range(_minVoicePitch, _maxVoicePitch);

            if (_referenceText.text[i] != _space && 
                _referenceText.text[i] != _comma && 
                _referenceText.text[i] != _dot && 
                _referenceText.text[i] != _question && 
                _referenceText.text[i] != _explanetion
                )
                _source.PlayOneShot(_voiceSampleBeep);

            _textContainer.text += _referenceText.text[i];
            yield return _waitDelay;
        }

        _dialogueCoroutine = null;
    }

    private void WipeText()
    {
        if (_textContainer.text != null)
            _textContainer.text = null;
    }
}
