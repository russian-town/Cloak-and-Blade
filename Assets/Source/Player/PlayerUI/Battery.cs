using Coffee.UIExtensions;
using UnityEngine;

namespace Source.Player.PlayerUI
{
    public class Battery : MonoBehaviour
    {
        [SerializeField] private UIParticle _charge;

        public void Enable() => _charge.gameObject.SetActive(true);

        public void Disable() => _charge.gameObject.SetActive(false);
    }
}
