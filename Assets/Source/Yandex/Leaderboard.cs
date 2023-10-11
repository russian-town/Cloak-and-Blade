using Agava.WebUtility;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup _leaderBoardView;
    [SerializeField] private LeaderBoardUnit _leaderBoardUnitTemplate;
    [SerializeField] private LeaderBoardUnit _thePlayer;
    [SerializeField] private int _maxLeaderboardUnits;
    [SerializeField] private Sprite _defaultProfilePicture;
    [SerializeField] private Image _blackBacking;

    private CanvasGroup _canvasGroup;

    public LeaderBoardUnit ThePlayer => _thePlayer;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0;
        ClearLeaderBoard();
    }

    public void OnOpenLeaderBoardButtonClick()
    {
        if (PlayerAccount.IsAuthorized)
        {
            OpenLeaderBoard();
        }
        else
        {
            PlayerAccount.Authorize(OpenLeaderBoard, null);

            if (PlayerAccount.IsAuthorized)
                PlayerAccount.RequestPersonalProfileDataPermission();
        }
    }

    public void CloseLeaderBoard()
    {
        _blackBacking.gameObject.SetActive(true);
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }

    private void OpenLeaderBoard()
    {
        _blackBacking.gameObject.SetActive(true);
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        ShowPlayer();
        BuildLeaderBoard();
    }

    private void ShowPlayer()
    {
        Leaderboard.GetPlayerEntry(Constants.LeaderBoardName, (result) => { SetPlayer(_thePlayer, result); });
    }

    private void BuildLeaderBoard()
    {
        ClearLeaderBoard();

        Leaderboard.GetEntries(Constants.LeaderBoardName, (result) =>
        {
            if (result.entries.Length >= _maxLeaderboardUnits)
            {
                for (int i = 0; i < _maxLeaderboardUnits; i++)
                {
                    LeaderBoardUnit tempUnit = Instantiate(_leaderBoardUnitTemplate, _leaderBoardView.transform);
                    SetPlayer(tempUnit, result.entries[i]);
                }
            }
            else
            {
                foreach (var entry in result.entries)
                {
                    LeaderBoardUnit tempUnit = Instantiate(_leaderBoardUnitTemplate, _leaderBoardView.transform);
                    SetPlayer(tempUnit, entry);
                }
            }
        });
    }

    private void ClearLeaderBoard()
    {
        if (_leaderBoardView.transform.childCount > 0)
            for (var i = _leaderBoardView.transform.childCount - 1; i >= 0; i--)
                Object.Destroy(_leaderBoardView.transform.GetChild(i).gameObject);
    }

    private void SetPlayer(LeaderBoardUnit tempUnit, LeaderboardEntryResponse entry)
    {
        if (string.IsNullOrEmpty(entry.player.publicName) == false)
        {
            tempUnit.SetProfileImage(entry.player.profilePicture);
            tempUnit.SetValues(tempUnit.Avatar, entry.rank, entry.player.publicName, entry.score);
        }
        else
        {
            tempUnit.SetDefaultProfilePicture();

            if (YandexGamesSdk.Environment.i18n.lang == "en")
                tempUnit.SetValues(tempUnit.Avatar, entry.rank, Constants.AnonymousEnglish, entry.score);

            if (YandexGamesSdk.Environment.i18n.lang == "ru")
                tempUnit.SetValues(tempUnit.Avatar, entry.rank, Constants.AnonymousRussian, entry.score);

            if (YandexGamesSdk.Environment.i18n.lang == "tr")
                tempUnit.SetValues(tempUnit.Avatar, entry.rank, Constants.AnonymousTurkish, entry.score);
        }
    }
}