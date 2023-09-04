using UnityEngine;

public class CellView : MonoBehaviour
{
    private ParticleSystem _enemySightEffectTemplate;
    private ParticleSystem _enemySightEffect;

    public void Initialize(ParticleSystem enemySightEffectTemplate)
    {
        _enemySightEffectTemplate = enemySightEffectTemplate;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void PlayEnemySightEffect()
    {
        if (_enemySightEffect == null)
        {
            _enemySightEffect = Instantiate(_enemySightEffectTemplate);
            _enemySightEffect.transform.position = transform.position;
            _enemySightEffect.Play();
        }
        else
        {
            _enemySightEffect.Play();
        }
    }

    public void StopEnemySightEffect() => _enemySightEffect.Stop();
}
