using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "New Level", order = 51)]
public class Level : ScriptableObject, IDataWriter, IDataReader
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _preview;

    private bool _isOpen;
    private int _starsCount;

    public string Name => _name;
    public Sprite Preview => _preview;
    public bool IsOpen => _isOpen;
    public int StarsCount => _starsCount;


    public void Read(PlayerData playerData)
    {
    }

    public void Write(PlayerData playerData)
    {
    }
}
