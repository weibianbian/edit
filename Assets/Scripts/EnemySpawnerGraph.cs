using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerGraph : SerializedMonoBehaviour
{
    public Agent prefab;
    public WaypointGroupGraph waypointGroupGraph;

    public void Awake()
    {
        Agent agent = Instantiate(prefab);

        WaypointGraph wpGraph = waypointGroupGraph.waypointGrahps[0];
        Vector3 spawPos = wpGraph.transform.position;

        agent.SetPos(spawPos);
        agent.MoveTo(spawPos);

        FSMComponentGraph fsmComptGraph= wpGraph.GetComponent<FSMComponentGraph>();

        fsmComptGraph.GenerateFSM(agent);
    }

}
