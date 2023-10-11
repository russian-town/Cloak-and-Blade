using Agava.YandexGames;
using UnityEngine;

public class LeaderBoardScoreSetter : MonoBehaviour
{
    public void SetPlayerScore()
    {
        int previousScore = 0;

        Leaderboard.GetPlayerEntry(Constants.LeaderBoardName, (result) =>
        {
            if (result != null)
            {
                previousScore = result.score;
                Leaderboard.SetScore(Constants.LeaderBoardName, ++previousScore);
            }
            else
            {
                Leaderboard.SetScore(Constants.LeaderBoardName, ++previousScore);
            }
        });
    }
}
