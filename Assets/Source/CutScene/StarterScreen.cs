using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup), typeof(AudioSource))]
public class StarterScreen : MonoBehaviour, IInitializable
{
    [SerializeField] private Button _pressAnyWhere;
    [SerializeField] private CanvasGroup _startText;
    [SerializeField] private CutsceneScenario _cutsceneScenario;
    [SerializeField] private CanvasGroup _name;
    [SerializeField] private AudioClip _startClickClip;

    private AudioSource _audioSource;
    private CanvasGroup _canvasGroup;

    public void Initialize()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _audioSource = GetComponent<AudioSource>();
        _pressAnyWhere.onClick.AddListener(OnButtonClick);
        _pressAnyWhere.enabled = true;
        _startText.alpha = 1.0f;
        _name.alpha = 1.0f;
    }

    private void OnButtonClick()
    {
        _pressAnyWhere.onClick.RemoveListener(OnButtonClick);
        _audioSource.PlayOneShot(_startClickClip);
        _canvasGroup.alpha = 0f;
        _cutsceneScenario.StartCutscene();
    }
}
