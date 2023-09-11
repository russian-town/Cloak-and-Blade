using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Button _move;
    [SerializeField] private Button _ability;
    [SerializeField] private Button _skip;
    
    private Player _player;

    private void OnDestroy()
    {
        Hide();
    }

    public void Initialize(Player player)
    {
        _player = player;
        Show();
    }

    public void Show()
    {
        _move.onClick.AddListener(OnMoveClick);
        _ability.onClick.AddListener(OnAbilityClick);
        _skip.onClick.AddListener(OnSkipClick);
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        _move.onClick.RemoveListener(OnMoveClick);
        _ability.onClick.RemoveListener(OnAbilityClick);
        _skip.onClick.RemoveListener(OnSkipClick);
        gameObject.SetActive(false);
    }

    private void OnMoveClick()
    {
        _player.PrepareMove();
    }
    
    private void OnAbilityClick()
    {
        _player.PrepareAbility();
    }
    
    private void OnSkipClick() => _player.PrepareSkip();
}
