using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int Stars;
    public Character CurrentSelectedCharacter;
    public Player CurrentPlayer;
    public List<Character> Characters = new List<Character>();
    public List<bool> IsBought = new List<bool>();
    public List<bool> IsSelect = new List<bool>();
    public List<UpgradeSetter> UpgradeSetters = new List<UpgradeSetter>();
    public List<int> Levels = new List<int>();
    public bool IsTutorialCompleted;
    public float MasterSliderValue;
    public float SoundSliderValue;
    public float MusicSliderValue;
}
