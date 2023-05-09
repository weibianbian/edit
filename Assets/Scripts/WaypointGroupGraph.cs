﻿using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class WaypointGroupGraph:SerializedMonoBehaviour
{
    
    [HideInInspector]
    public float arrowHeadLength = 1.0f;
    [Title("xxx")]
    [LabelText("路点列表")]
    [ShowInInspector]
    [HideReferenceObjectPicker]
    public List<WaypointGraph> waypointGrahps = new List<WaypointGraph>();

    public void Awake()
    {
    }
    public void OnDrawGizmos()
    {
        //for (int i = 0; i < waypointGroup.Count - 1; i++)
        //{
        //    Vector3 Start = waypointGroup[i].transform.position;
        //    Vector3 End = waypointGroup[i + 1].transform.position;
        //    var direction = (End - Start).normalized;

        //    var Length = Vector3.Distance(End, Start) - waypointGroup[i + 1].radius - waypointGroup[i].radius;

        //    Start = Start + direction * waypointGroup[i].radius;
        //    End = Start + direction * Length;
        //    DrawArrowHelper.Draw(
        //       Start,
        //       End,
        //       arrowHeadLength
        //       );
        //}
    }
}