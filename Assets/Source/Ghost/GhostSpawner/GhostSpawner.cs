using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    public Ghost Get(Cell spawnCell, Ghost template)
    {
        Ghost ghost = Instantiate(template);
        ghost.transform.localPosition = spawnCell.transform.localPosition;
        return ghost;
    }
}
