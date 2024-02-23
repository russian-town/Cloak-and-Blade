using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleTextTyper : MonoBehaviour
{
    [SerializeField] private TMP_Text _textContainer;
    [SerializeField] private RectTransform _letter;
    [SerializeField] private float _delay;
    [SerializeField] private float _dotDelay;
    [SerializeField] private float _commaDelay;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private List<PhraseWithPause> _phrasesWithPause;

    private WaitForSeconds _waitDelay;
    private WaitForSeconds _dotWaitDelay;
    private WaitForSeconds _commaWaitDelay;
    private WaitForSeconds _phraseWait;
    private List<char> _tempCharList;
    private List<char> _poop;

    private void OnEnable()
    {
        /*_textContainer.color = new Color32(13, 13, 48, 0);*/
    }

    private void Start()
    {
        _waitDelay = new WaitForSeconds(_delay);
        _dotWaitDelay = new WaitForSeconds(_dotDelay);
        _commaWaitDelay = new WaitForSeconds(_commaDelay);
        _tempCharList = new List<char>();
    }

    public void TypeText() => StartCoroutine(WriteLine());

    private IEnumerator WriteLine()
    {
        _textContainer.color = new Color32(13, 13, 48, 0);
        _canvasGroup.DOFade(1, 1).SetEase(Ease.InOutSine);
        _letter.DOScale(1, 1).SetEase(Ease.InOutSine);

        yield return _commaWaitDelay;

        for (int i = 0; i < _textContainer.text.Length; i++)
        {
            _textContainer.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            Color32 myColor32 = hexToColor("0D0D30");
            int meshIndex = _textContainer.textInfo.characterInfo[i].materialReferenceIndex;
            int vertexIndex = _textContainer.textInfo.characterInfo[i].vertexIndex;
            Color32[] vertexColors = _textContainer.textInfo.meshInfo[meshIndex].colors32;
            vertexColors[vertexIndex + 0] = myColor32;
            vertexColors[vertexIndex + 1] = myColor32;
            vertexColors[vertexIndex + 2] = myColor32;
            vertexColors[vertexIndex + 3] = myColor32;
            _textContainer.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

            if (!char.IsWhiteSpace(_textContainer.text[i]))
                _tempCharList.Add(_textContainer.text[i]);

            foreach (var phrase in _phrasesWithPause)
            {
                if (phrase.Phrase == string.Concat(_tempCharList))
                {
                    _phraseWait = new WaitForSeconds(phrase.Pause);
                    yield return _phraseWait;
                    _phraseWait = null;
                }
            }

            if (char.IsWhiteSpace(_textContainer.text[i]))
                _tempCharList.Clear();

            if (i < _textContainer.text.Length - 1)
                if (_textContainer.text[i] == '.' && _textContainer.text[i + 1] != '.')
                    yield return _dotWaitDelay;

            if(_textContainer.text[i] == ',' || _textContainer.text[i] == '—')
                yield return _commaWaitDelay;

            yield return _waitDelay;
        }
    }

    private Color32 hexToColor(string hex)
    {
        hex = hex.Replace("0x", "");
        hex = hex.Replace("#", "");
        byte a = 255;
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        if (hex.Length == 8)
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

        return new Color32(r, g, b, a);
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
