using UnityEngine;
using UnityEngine.UI;

public class GameOverView : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;

    public void Show() => gameObject.SetActive(true);

    public void Hide() => gameObject.SetActive(false);
}
