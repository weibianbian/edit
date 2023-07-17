using UnityEngine;

namespace RailShootGame
{
    public class SpawnZone : MonoBehaviour
    {
        public enum E_State
        {
            E_WAITING_FOR_START,
            E_SPAWNING_ENEMIES,
            E_IN_PROGRESS,
            E_FINISHED,
        }
        public E_State State = E_State.E_WAITING_FOR_START;
        public SpawnPointEnemy[] SpawnPoints = null;

        private void OnTriggerEnter(Collider other)
        {
            if (State != E_State.E_WAITING_FOR_START)
                return;

        }
    }
}

