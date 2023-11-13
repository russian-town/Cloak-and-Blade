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

    public Coroutine StartFade()
    {
        return StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        while (_canvasGroup.alpha > 0)
        {
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 0, Time.deltaTime * _speed);
            yield return null;
        }

        _canvasGroup.blocksRaycasts = false;
    }
}
