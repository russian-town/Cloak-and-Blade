using System.Linq;
using UnityEngine;
using Agava.WebUtility;

public class AutoPixelisation : MonoBehaviour
{
    [SerializeField] private PSXEffects _psxEffect;

    private void Start()
    {
        AdjustEffectToScreenSize();
    }

    private void AdjustEffectToScreenSize()
    {
        if(Enumerable.Range(3000, 10000).Contains(Screen.width))
        {
            _psxEffect.resolutionFactor = 10;
            _psxEffect.vertexInaccuracy = 350;
        }
        else if(Enumerable.Range(1500, 3000).Contains(Screen.width))
        {
            _psxEffect.resolutionFactor = 5;
            _psxEffect.vertexInaccuracy = 0;
        }
        else if (Enumerable.Range(0, 1500).Contains(Screen.width))
        {
            _psxEffect.resolutionFactor = 3;
            _psxEffect.vertexInaccuracy = 0;
        }

        if (Device.IsMobile)
            _psxEffect.vertexInaccuracy = 0;
    }
}
