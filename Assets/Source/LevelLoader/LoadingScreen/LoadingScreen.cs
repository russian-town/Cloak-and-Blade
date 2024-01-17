using System.Collections;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private float _speed;

    private CanvasGroup _canvasGroup;

    public void Initialize()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public Coroutine StartFade(float value)
    {
        return StartCoroutine(Fade(value));
    }

    private IEnumerator Fade(float value)
    {
        while (Mathf.Approximately(_canvasGroup.alpha, value) == false)
        {
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, value, Time.deltaTime * _speed);
            yield return null;
        }

        _canvasGroup.blocksRaycasts = false;
    }
}
