using UnityEngine;

public class MenuPlayerModel : MonoBehaviour
{
    [SerializeField] private Player _player;

    public Player Player => _player;
}
