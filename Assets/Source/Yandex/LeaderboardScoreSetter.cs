using Agava.YandexGames;
using UnityEngine;

public class LeaderBoardScoreSetter : MonoBehaviour
{
    public void SetPlayerScore(int starsCount)
    {
        int previousScore = 0;

        Leaderboard.GetPlayerEntry(Constants.LeaderBoardName, (result) =>
        {
            if (result != null)
            {
                previousScore = result.score;
                Leaderboard.SetScore(Constants.LeaderBoardName, previousScore += starsCount);
            }
            else
            {
                Leaderboard.SetScore(Constants.LeaderBoardName, starsCount);
            }
        });
    }
}
