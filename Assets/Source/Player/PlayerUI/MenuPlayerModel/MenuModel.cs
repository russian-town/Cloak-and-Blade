using UnityEngine;

namespace Source.Player.PlayerUI.MenuPlayerModel
{
    public class MenuModel : MonoBehaviour
    {
        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);
    }
}
