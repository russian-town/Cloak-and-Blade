using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private int _firstLevelIndex;

    private bool _tutorialCompleted;

    public bool TryLoadFirstLevel()
    {
        if (_tutorialCompleted == false)
        {
            SceneManager.LoadScene(Constants.Tutorial);
            return false;
        }

        int index = Random.Range(_firstLevelIndex, SceneManager.sceneCountInBuildSettings - 1);
        SceneManager.LoadScene(index);
        return true;
    }
}
