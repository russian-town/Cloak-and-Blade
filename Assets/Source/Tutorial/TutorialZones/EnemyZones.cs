using UnityEngine;

namespace Source.Tutorial.TutorialZones
{
    public class EnemyZones : TutorialZone
    {
        [SerializeField] private Bootstrap _bootstrap;

        public override void Interact()
        {
            base.Interact();
            _bootstrap.SpawnEnemy();
        }
    }
}
