using UnityEngine;

[RequireComponent(typeof(TMPro.TMP_Text))]
public class TutorialText : MonoBehaviour
{
    [SerializeField] private bool _isTutorialTrigger;

    private TMPro.TMP_Text _text;

    public bool IsTutorialTrigger => _isTutorialTrigger;
    public string Line => _text.text;

    private void Awake()
    {
        _text = GetComponent<TMPro.TMP_Text>();
    }
}
