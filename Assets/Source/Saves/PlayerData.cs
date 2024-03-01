using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int Stars;
    public Player CurrentPlayer;
    public List<string> Characters = new List<string>();
    public List<bool> IsBought = new List<bool>();
    public List<bool> IsSelect = new List<bool>();
    public List<string> UpgradeSetters = new List<string>();
    public List<int> UpgradeLevels = new List<int>();
    public bool IsTutorialCompleted;
    public float MasterSliderValue = 1;
    public float SoundSliderValue = 1;
    public float MusicSliderValue = 1;
    public List<string> FinishedLevelNames = new List<string>();
    public List<string> OpenedLevelNames = new List<string>();
    public List<int> FinishedLevelsStarsCount = new List<int>();
    public string CurrentLanguague;
}
