using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public void Hide() => gameObject.SetActive(false);

    public void Show() => gameObject.SetActive(true);
}
