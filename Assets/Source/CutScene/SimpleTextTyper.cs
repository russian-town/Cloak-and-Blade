using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class SimpleTextTyper : MonoBehaviour
{
    [SerializeField] private TMP_Text _textContainer;
    [SerializeField] private RectTransform _letter;
    [SerializeField] private float _delay;
    [SerializeField] private float _dotDelay;
    [SerializeField] private float _commaDelay;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private List<PhraseWithPause> _phrasesWithPause;
    [SerializeField] private FocusHandler _focusHandler;
    [SerializeField] private String _colorOpenTag;
    [SerializeField] private String _colorCloseTag;
    [SerializeField] private LeanFixer _fixer;

    private WaitForSeconds _waitDelay;
    private WaitForSeconds _dotWaitDelay;
    private WaitForSeconds _commaWaitDelay;
    private WaitForSeconds _phraseWait;
    private List<char> _tempCharPhraseList;
    private List<char> _tempCharTextList;
    private List<char> _initialText;
    private int _writtenChars;

    private void Start()
    {
        _waitDelay = new WaitForSeconds(_delay);
        _dotWaitDelay = new WaitForSeconds(_dotDelay);
        _commaWaitDelay = new WaitForSeconds(_commaDelay);
        _tempCharPhraseList = new List<char>();
        _textContainer.text = _fixer.GetLocalisedText();
        _initialText = _textContainer.text.ToList();
        _tempCharTextList = _initialText.ToList();
        _tempCharTextList.InsertRange(0, _colorOpenTag);
        _tempCharTextList.AddRange(_colorCloseTag);
        _textContainer.text = string.Join("", _tempCharTextList);
    }

    public void TypeText() => StartCoroutine(WriteLine());

    private IEnumerator WriteLine()
    {
        /*_textContainer.color = new Color32(13, 13, 48, 100);*/
        _canvasGroup.DOFade(1, 1).SetEase(Ease.InOutSine);
        _letter.DOScale(1, 1).SetEase(Ease.InOutSine);

        yield return _commaWaitDelay;

        for (int i = 0; i < _textContainer.text.Length; i++)
        {
            print(_textContainer.text[i + 16]);
            var text = _initialText.ToList();
            text.InsertRange(i, _colorOpenTag.ToCharArray());
            text.AddRange(_colorCloseTag.ToCharArray());

            _textContainer.text = string.Join("", text);

            if (!char.IsWhiteSpace(_textContainer.text[i + 16]))
                _tempCharPhraseList.Add(_textContainer.text[i + 16]);

            foreach (var phrase in _phrasesWithPause)
            {
                if (phrase.Phrase == string.Concat(_tempCharPhraseList))
                {
                    _phraseWait = new WaitForSeconds(phrase.Pause);
                    yield return _phraseWait;
                    _phraseWait = null;
                }
            }

            if (char.IsWhiteSpace(_textContainer.text[i + 16]))
                _tempCharPhraseList.Clear();

            if (i < _textContainer.text.Length - 1)
                if (_textContainer.text[i + 16] == '.' && _textContainer.text[i + 17] != '.')
                    yield return _dotWaitDelay;

            if(_textContainer.text[i + 16] == ',' || _textContainer.text[i + 16] == '—')
                yield return _commaWaitDelay;

            yield return _waitDelay;
        }
    }
}

[Serializable]
public class PhraseWithPause
{
    [SerializeField] private string _phrase;
    [SerializeField] private float _pause;

    public string Phrase => _phrase;
    public float Pause => _pause;
}
