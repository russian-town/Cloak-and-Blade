using System.Collections.Generic;
using Agava.YandexGames;
using Source.Root;
using Source.UiAnimations;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Yandex
{
    [RequireComponent(typeof(CanvasGroup))]
    public class LeaderBoard : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _leaderBoardView;
        [SerializeField] private List<LeaderBoardUnit> _leaderBoardUnits;
        [SerializeField] private LeaderBoardUnit _thePlayer;
        [SerializeField] private int _maxLeaderboardUnits;
        [SerializeField] private Sprite _defaultProfilePicture;
        [SerializeField] private Image _blackBacking;
        [SerializeField] private AuthorizationReqScreen _autorizationRequirmentScreen;
        [SerializeField] private ScreenAnimationHandler _animationHandler;

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
                _autorizationRequirmentScreen.Enable();
            }
        }

        public void Authorize()
        {
            PlayerAccount.Authorize(OpenLeaderBoard, null);

            if (PlayerAccount.IsAuthorized)
                PlayerAccount.RequestPersonalProfileDataPermission();
        }

        private void OpenLeaderBoard()
        {
            if (_autorizationRequirmentScreen.CanvasGroup.alpha > .9)
                _autorizationRequirmentScreen.Disable();

            ShowPlayer();
            BuildLeaderBoard();
            _animationHandler.FadeIn();
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
                        _leaderBoardUnits[i].gameObject.SetActive(true);
                        SetPlayer(_leaderBoardUnits[i], result.entries[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < result.entries.Length; i++)
                    {
                        _leaderBoardUnits[i].gameObject.SetActive(true);
                        SetPlayer(_leaderBoardUnits[i], result.entries[i]);
                    }
                }
            });
        }

        private void ClearLeaderBoard()
        {
            if (_leaderBoardView.transform.childCount > 0)
            {
                for (var i = _leaderBoardView.transform.childCount - 1; i >= 0; i--)
                {
                    _leaderBoardView.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
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

                switch (YandexGamesSdk.Environment.i18n.lang)
                {
                    case Constants.En:
                        tempUnit.SetValues(tempUnit.Avatar, entry.rank, Constants.AnonymousEnglish, entry.score);
                        break;
                    case Constants.Ru:
                        tempUnit.SetValues(tempUnit.Avatar, entry.rank, Constants.AnonymousRussian, entry.score);
                        break;
                    case Constants.Tr:
                        tempUnit.SetValues(tempUnit.Avatar, entry.rank, Constants.AnonymousTurkish, entry.score);
                        break;
                    default:
                        tempUnit.SetValues(tempUnit.Avatar, entry.rank, Constants.AnonymousEnglish, entry.score);
                        break;
                }
            }
        }
    }
}