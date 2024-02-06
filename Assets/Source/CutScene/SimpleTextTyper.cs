using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class SimpleTextTyper : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _textContainer;
    [SerializeField] private float _delay;
    [SerializeField] private float _dotDelay;
    [SerializeField] private float _commaDelay;
    [SerializeField] private CanvasGroup _canvasGroup;

    private WaitForSeconds _waitDelay;
    private WaitForSeconds _dotWaitDelay;
    private WaitForSeconds _commaWaitDelay;

    private void Start()
    {
        _waitDelay = new WaitForSeconds(_delay);
        _dotWaitDelay = new WaitForSeconds(_dotDelay);
        _commaWaitDelay = new WaitForSeconds(_commaDelay);
    }

    public void TypeText() => StartCoroutine(WriteLine());

    private IEnumerator WriteLine()
    {
        _textContainer.text = _text.text;
        _canvasGroup.DOFade(1, 1);
        yield return _dotWaitDelay;

        for (int i = 0; i < _text.text.Length; i++)
        {
            Color32 myColor32 = hexToColor("0D0D30");
            int meshIndex = _textContainer.textInfo.characterInfo[i].materialReferenceIndex;
            int vertexIndex = _textContainer.textInfo.characterInfo[i].vertexIndex;
            Color32[] vertexColors = _textContainer.textInfo.meshInfo[meshIndex].colors32;
            vertexColors[vertexIndex + 0] = myColor32;
            vertexColors[vertexIndex + 1] = myColor32;
            vertexColors[vertexIndex + 2] = myColor32;
            vertexColors[vertexIndex + 3] = myColor32;
            _textContainer.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

            if (i < _text.text.Length - 1)
                if (_text.text[i] == '.' && _text.text[i + 1] != '.')
                    yield return _dotWaitDelay;

            if(_text.text[i] == ',' || _text.text[i] == '—')
                yield return _commaWaitDelay;

            yield return _waitDelay;
        }

        yield return _dotWaitDelay;
        _canvasGroup.DOFade(0, 2f).SetEase(Ease.InSine);
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
