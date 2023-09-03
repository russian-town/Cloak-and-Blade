using UnityEngine;

public class CellView : MonoBehaviour
{
    private ParticleSystem _enemySightEffectTemplate;
    private ParticleSystem _enemySightEffect;

    public void Initialize(ParticleSystem enemySightEffectTemplate)
    {
        _enemySightEffectTemplate = enemySightEffectTemplate;
    }

    public void PlayEnemySightEffect()
    {
        if (_enemySightEffect == null)
        {
            print("Initializing effect");
            _enemySightEffect = Instantiate(_enemySightEffectTemplate);
            _enemySightEffect.transform.position = transform.position;
            _enemySightEffect.Play();
        }
        else
        {
            print("Effect already initialized");
            _enemySightEffect.Play();
        }
    }

    public void StopEnemySightEffect() => _enemySightEffect.Stop();
}
