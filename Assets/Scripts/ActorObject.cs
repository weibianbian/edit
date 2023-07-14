using BT.Runtime;
using RailShootGame;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ActorObject : MonoBehaviour
{
    public Actor actor;
    public void Start()
    {
        AttachLogic();
    }
    public void AttachLogic()
    {
        actor = new Actor();
        actor.actorObject = this;
        actor.Init();
        actor.move.agent = gameObject.GetComponent<NavMeshAgent>();
        actor.move.agent.enabled = true;
        StartCoroutine(CoroutineFunc());
    }
    public IEnumerator CoroutineFunc()
    {
        actor.Spawn();
        StartCoroutine(CoroutineFunc());
        yield return new WaitForSeconds(2);
        actor.move.MoveToPosition(new Vector3(100,0,100));
    }
    public void Update()
    {
        actor?.Update();
    }

}
