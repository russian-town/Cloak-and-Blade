using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "New Level", order = 51)]
public class Level : ScriptableObject, IDataReader, IDataWriter
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _preview;
    [SerializeField] private bool _isOpen;
    [SerializeField] private bool _isCompleted;

    private int _starsCount;

    public string Name => _name;
    public Sprite Preview => _preview;
    public bool IsOpen => _isOpen;
    public bool IsCompleted => _isCompleted;
    public int StarsCount => _starsCount;

    public void Open() => _isOpen = true;

    public void Read(PlayerData playerData)
    {
        if (playerData.FinishedLevelNames.Contains(Name))
            _isCompleted = true;

        if (playerData.OpenedLevelNames.Contains(Name))
            _isOpen = true;
    }

    public void Write(PlayerData playerData)
    {
        if (playerData.OpenedLevelNames.Contains(Name))
            return;

        if (_isOpen == true)
            playerData.OpenedLevelNames.Add(Name);
    }
}
