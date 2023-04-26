using Sirenix.OdinInspector;
using System.Collections.Generic;

public class EventActionData
{
    [LabelText("条件")]
    public List<ConditionData> conditions = new List<ConditionData>();
    [LabelText("延迟时间（秒）")]
    public float delayTime = 0;
    [LabelText("动作ID")]
    public EEventAction actionType= EEventAction.None;
    [LabelText("原地创建")]
    public bool isCreateInPlace = false;
    [LabelText("阻塞")]
    public bool isBlock = true;
    [LabelText("等待条件成立")]
    public bool isWaitForConditionToHold = true;
}
public class ConditionData
{

}