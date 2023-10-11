using UnityEngine;

public class LevelFinishScreen : MonoBehaviour
{
    public void Show() => gameObject.SetActive(true);

    public void Hide() => gameObject.SetActive(false);
}
