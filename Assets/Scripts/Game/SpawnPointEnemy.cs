using UnityEngine;

namespace RailShootGame
{
    public class SpawnPointEnemy : SpawnPoint
    {
        public EActorType actorType;
        public Transform spawnPoint;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawSphere(transform.position, 0.4f);
            if (spawnPoint != null )
            {
                Gizmos.DrawIcon(spawnPoint.position, "SpawnPoint.tif");
            }
        }

    }
}

