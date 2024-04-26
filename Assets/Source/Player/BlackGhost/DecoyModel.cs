using UnityEngine;

namespace Source.Player.BlackGhost
{
    public class DecoyModel : MonoBehaviour
    {
        public void Hide()
            => gameObject.SetActive(false);

        public void Show()
            => gameObject.SetActive(true);
    }
}
