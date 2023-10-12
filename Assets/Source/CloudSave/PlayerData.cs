public class PlayerData
{
    private Player _currentPlayer;
    private CharacterView _currentCharacterView;
    private Character _currentSelectedCharacter;
    private int _money;

    public Player CurrentPlayer => _currentPlayer;
    public Character CurrentSelectedCharacter => _currentSelectedCharacter;
    public CharacterView CurrentCharacterView => _currentCharacterView;
    public int Money => _money;

    public PlayerData(Player currentPlayer, CharacterView currentCharacterView, Character character, int money)
    {
        _currentPlayer = currentPlayer;
        _currentCharacterView = currentCharacterView;
        _currentSelectedCharacter = character;
        _money = money;
    }
}
