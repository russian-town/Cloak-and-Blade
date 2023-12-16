using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerModel : MonoBehaviour
{
    [SerializeField] private DecoyModel[] _decoyModels;
    [SerializeField] private ParticleSystem _transformationEffect;
    [SerializeField] private ParticleSystem _mimicEffect;

    private Animator _animator;
    private DecoyModel _currentDecoy;
    private Vector3 _baseLocalPosition = Vector3.zero;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
    }

    public bool Enabled => gameObject.activeInHierarchy;

    public void Hide() => gameObject.SetActive(false);

    public void Show() => gameObject.SetActive(true);

    public void PlayEffect()
    {
        _mimicEffect.gameObject.SetActive(true);
        _mimicEffect.Play();
        _transformationEffect.Play();
    } 

    public void TransformToDecoy()
    {
        _animator.SetTrigger(Constants.TransformationTrigger);
    }

    public void SwitchBack()
    {
        transform.localPosition = _baseLocalPosition;
        _mimicEffect.gameObject.SetActive(false);
        _currentDecoy.Hide();
        _transformationEffect.Play();
        Show();
        _currentDecoy = null;
    }

    public void PickRandomModel()
    {
        var randomIndex = Random.Range(0, _decoyModels.Length);
        _currentDecoy = _decoyModels[randomIndex];
        _currentDecoy.Show();
    }
}
