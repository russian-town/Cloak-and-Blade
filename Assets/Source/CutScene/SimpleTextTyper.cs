using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

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
    [SerializeField] private string _colorOpenTag;
    [SerializeField] private string _colorCloseTag;
    [SerializeField] private LeanFixer _fixer;

    private WaitForSeconds _waitDelay;
    private WaitForSeconds _dotWaitDelay;
    private WaitForSeconds _commaWaitDelay;
    private WaitForSeconds _phraseWait;
    private List<char> _tempCharPhraseList;
    private List<char> _tempCharTextList;
    private List<char> _initialText;

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
        _canvasGroup.DOFade(1, 1).SetEase(Ease.InOutSine);
        _letter.DOScale(1, 1).SetEase(Ease.InOutSine);

        yield return _commaWaitDelay;

        for (int i = 0; i < _initialText.Count; i++)
        {
            var text = _initialText.ToList();
            text.InsertRange(i, _colorOpenTag.ToCharArray());
            text.AddRange(_colorCloseTag.ToCharArray());

            _textContainer.text = string.Join("", text);

            if(i > 0)
            {
                if (!char.IsWhiteSpace(_initialText[i - 1]))
                    _tempCharPhraseList.Add(_initialText[i - 1]);

                foreach (var phrase in _phrasesWithPause)
                {
                    if (phrase.Phrase == string.Concat(_tempCharPhraseList))
                    {
                        _phraseWait = new WaitForSeconds(phrase.Pause);
                        yield return _phraseWait;
                        _phraseWait = null;
                    }
                }

                if (char.IsWhiteSpace(_initialText[i - 1]))
                    _tempCharPhraseList.Clear();

                if (i < _initialText.Count)
                    if (_initialText[i - 1] == '.' && _initialText[i] != '.')
                        yield return _dotWaitDelay;

                if (_initialText[i - 1] == ',' || _initialText[i - 1] == '—')
                    yield return _commaWaitDelay;
            }

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
