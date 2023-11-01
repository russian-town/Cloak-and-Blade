using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public bool Enabled => gameObject.activeInHierarchy;

    public void Hide() => gameObject.SetActive(false);

    public void Show() => gameObject.SetActive(true);
}
