using UnityEngine;
using UnityEngine.AI;

namespace RailShootGame
{
    public class MovementCompt : ActorComponent
    {

        public NavMeshAgent agent;
        public Vector3 targetPos;
        public Vector3 Velocity;
        public void Init()
        {

        }


        public override void TickComponent(float DeltaTime)
        {
           
        }
        public Vector3 GetActorFeetLocation()
        {
            return Vector3.zero;
        }
    }
}

