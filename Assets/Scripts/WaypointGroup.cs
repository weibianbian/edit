using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

[Serializable]
public class WaypointGroup
{
    [LabelText("路点")]
    [ShowInInspector]
    [HideReferenceObjectPicker]
    public List<WaypointData> waypoints = new List<WaypointData>();
}
