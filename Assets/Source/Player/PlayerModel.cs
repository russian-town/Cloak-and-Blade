using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [SerializeField] private PlayerModel _modelToTransit;

    public bool Enabled => gameObject.activeInHierarchy;

    public void Hide() => gameObject.SetActive(false);

    public void Show() => gameObject.SetActive(true);

    public void SwitchModels()
    {
        Hide();
        _modelToTransit.Show();
    }
}
