using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RailShootGame
{
    public class SpawnZone : MonoBehaviour
    {
        public enum EState
        {
            E_WAITING_FOR_START,
            E_SPAWNING_ENEMIES,
            E_IN_PROGRESS,
            E_FINISHED,
        }
        public EState state = EState.E_WAITING_FOR_START;
        public SpawnPointEnemy[] spawnPoints = null;
        private List<AActor> enemiesAlive = new List<AActor>();
        public bool IsActive() { return enemiesAlive.Count > 0; }
        public int GetEnemyCount() { return enemiesAlive.Count; }
        private void OnTriggerEnter(Collider other)
        {
            if (state != EState.E_WAITING_FOR_START)
                return;
            Debug.Log("触发");
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                SpawnPointEnemy pointEnemy = spawnPoints[i];
                AActor actor = GameManager.ActorManager.SpawnActor(pointEnemy.actorType, pointEnemy.spawnPoint.transform.position);
                enemiesAlive.Add(actor);
            }

        }
        public void Update()
        {
            if (state != EState.E_IN_PROGRESS)
                return;
            if (enemiesAlive.Count == 0)
            {
                state = EState.E_FINISHED;
            }
        }
        void OnDrawGizmos()
        {
            BoxCollider b = GetComponent<BoxCollider>();
            if (b != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(b.transform.position + b.center, b.size);
            }
            if (spawnPoints != null)
            {
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    if (b != null)
                        Gizmos.DrawLine(b.transform.position + b.center, spawnPoints[i].spawnPoint.transform.position);
                    else
                        Gizmos.DrawLine(transform.position, spawnPoints[i].spawnPoint.transform.position);
                }
            }
        }
    }
}

