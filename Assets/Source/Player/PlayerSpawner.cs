using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Player Get(Cell spawnCell, Player template)
    {
        Player player = Instantiate(template);
        player.transform.localPosition = spawnCell.transform.localPosition;
        return player;
    }
}
