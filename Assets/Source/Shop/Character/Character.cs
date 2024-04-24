using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "New character", order = 51)]
public class Character : ScriptableObject, IDataReader, IDataWriter
{
    [SerializeField] private CharacterType _type;
    [SerializeField] private int _price;
    [SerializeField] private Sprite _unlockedIcon;
    [SerializeField] private Sprite _lockedIcon;
    [SerializeField] private Player _player;
    [SerializeField] private MenuModel _menuPlayerModelTemplate;
    [SerializeField] private bool _isBought = false;
    [SerializeField] private bool _isSelect = false;
    [SerializeField] private Description _description;
    [SerializeField] private UpgradeSetter _upgradeSetter;
    [SerializeField] private Color _effectColor;

    public CharacterType Type => _type;

    public int Price => _price;

    public Sprite UnlockedIcon => _unlockedIcon;

    public Sprite LockedIcon => _lockedIcon;

    public bool IsBought => _isBought;

    public bool IsSelect => _isSelect;

    public Player Player => _player;

    public MenuModel MenuModelTemplate => _menuPlayerModelTemplate;

    public Description Description => _description;

    public UpgradeSetter UpgradeSetter => _upgradeSetter;

    public Color EffectColor => _effectColor;

    public void Buy() => _isBought = true;

    public void Write(PlayerData playerData)
    {
        for (int i = 0; i < playerData.Characters.Count; i++)
        {
            if (playerData.Characters[i] == name)
            {
                playerData.IsBought[i] = _isBought;
                playerData.IsSelect[i] = _isSelect;
                return;
            }
        }

        playerData.Characters.Add(name);
        playerData.IsBought.Add(_isBought);
        playerData.IsSelect.Add(_isSelect);
    }

    public void Read(PlayerData playerData)
    {
        for (int i = 0; i < playerData.Characters.Count; i++)
        {
            if (playerData.Characters[i] == name)
            {
                _isBought = playerData.IsBought[i];
                _isSelect = playerData.IsSelect[i];
                return;
            }
        }
    }

    public void CloseDescription() => _description.Hide();

    public void Select() => _isSelect = true;

    public void UnSelect() => _isSelect = false;
}
