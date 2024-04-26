using UnityEngine;
using UnityEngine.UI;

namespace Source.InteractiveObjects.InteractiveObjectView
{
    public class InteractiveObjectView : MonoBehaviour
    {
        [SerializeField] private Button _interactButton;

        public Button InteractButton => _interactButton;

        private void Start()
            => Hide();

        public void Show()
            => gameObject.SetActive(true);

        public void Hide()
            => gameObject.SetActive(false);
    }
}
