using UEngine;
using UEngine.GameFramework;
using UnityEngine;

public class ActorObject : MonoBehaviour
{
    public AActor actor;
    public UWorld game;
    public WaypointGroup waypointGroup;
    public void Start()
    {

    }
    public AActor AttachLogic()
    {
        actor = new AActor();
        actor.actorObject = this;
        actor.Init(game);
       

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
    }

}
