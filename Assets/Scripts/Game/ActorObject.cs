using BT.Runtime;
using RailShootGame;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ActorObject : MonoBehaviour
{
    public Actor actor;
    public Game game;
    public WaypointGroup waypointGroup;
    public void Start()
    {

    }
    public Actor AttachLogic()
    {
        actor = new Actor();
        actor.actorObject = this;
        actor.Init(game);
        actor.move.agent = gameObject.GetComponent<NavMeshAgent>();
        actor.move.agent.enabled = true;

        return actor;
    }
    //public IEnumerator CoroutineFunc()
    //{

    //    StartCoroutine(CoroutineFunc());
    //    yield return new WaitForSeconds(2);
    //    actor.move.MoveToPosition(new Vector3(100,0,100));
    //}
    public void Update()
    {
        actor?.Update();
    }

}
