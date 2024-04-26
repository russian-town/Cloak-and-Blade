using UnityEngine;

namespace Source.Player.PlayerUI.MenuPlayerModel
{
    public class ModelPlace : MonoBehaviour
    {
        public Vector3 Position => transform.position;

        public Quaternion Rotation => transform.rotation;
    }
}
