using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour, IDataReader
{
    [SerializeField] private int _firstLevelIndex;

    private bool _tutorialCompleted;

    public void Read(PlayerData playerData) => _tutorialCompleted = playerData.IsTutorialCompleted;

    public bool TryLoadFirstLevel()
    {
        //if (_tutorialCompleted == false)
        //{
        //    SceneManager.LoadScene(Constants.Tutorial);
        //    return false;
        //}

        //int index = Random.Range(_firstLevelIndex, SceneManager.sceneCountInBuildSettings - 1);
        SceneManager.LoadScene("Cathedral-1");
        return true;
    }
}
