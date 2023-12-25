using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class MainButton : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    public bool IsOpen {  get; private set; }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }

    public void Hide() 
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }

    public void Open() => IsOpen = true;
}
