using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft;
using Newtonsoft.Json;
using UnityEngine;
using System;
using UnityEditor;

public class BaseData
{

}
public class MoveData : BaseData
{
    public EMoveCommand moveCommand;
}
public class ShootData : BaseData
{

}

public class Node
{
    public virtual void OnNodeActivation(CompositeNode compositeNode)
    {

    }
}

public class TaskNode : Node
{
    [HideReferenceObjectPicker]
    [ValueDropdown("@EditorTool.DecoratorTypes", DrawDropdownForListElements = false)]
    public List<Decorator> decorators = new List<Decorator>();

    public ENodeResult WrappedExecuteTask()
    {
        return ExecuteTask();
    }
    public void WrappedOnTaskFinished(ENodeResult taskResult)
    {

    }
    public void WrappedTickTask(float deltaTime)
    {

    }
    public virtual ENodeResult ExecuteTask()
    {
        return ENodeResult.Succeeded;
    }
}
[LabelText("移动任务")]
public class MoveTaskNode : TaskNode
{
    [HideReferenceObjectPicker]
    public MoveData data = new MoveData();
}
[LabelText("射击任务")]
public class ShootTaskNode : TaskNode
{
    [HideReferenceObjectPicker]
    public ShootData data = new ShootData();
}
public class CompositeNode : Node
{
    [LabelText("状态"), ReadOnly]
    public EStatus status;

    [LabelText("任务列表")]
    [HideReferenceObjectPicker]
    [ValueDropdown("@EditorTool.TaskTypes", DrawDropdownForListElements = false)]
    public List<TaskNode> tasks;
    [HideInInspector]
    public int currentChild;
    [HideInInspector]
    public int overrideChild;
    [HideInInspector]
    public TaskNode activeTask;
    public CompositeNode()
    {
        tasks = new List<TaskNode>();

    }

    public void AddNode(TaskNode node)
    {
        tasks.Add(node);
    }
    public void TaskFinished()
    {

    }
    public int FindChildToExecute()
    {
        int childIdx = GetNextChild(currentChild);
        int RetIdx = (int)ENodeResult.ReturnToParent;
        while (IsValidIndex(childIdx))
        {
            if (DoDecoratorsAllowExecution(childIdx))
            {
                OnChildActivation(childIdx);
                RetIdx = childIdx;
                break;
            }
            childIdx = GetNextChild(childIdx);
        }
        return RetIdx;
    }
    public void OnChildActivation(int childIndex)
    {
        NotifyDecoratorsOnActivation(childIndex);
        currentChild = childIndex;
    }
    public bool DoDecoratorsAllowExecution(int childIndex)
    {
        TaskNode taskNode = tasks[childIndex];
        bool result = true;
        if (taskNode.decorators.Count == 0)
        {
            return result;
        }
        for (int decoratorIndex = 0; decoratorIndex < taskNode.decorators.Count; decoratorIndex++)
        {
            Decorator TestDecorator = taskNode.decorators[decoratorIndex];
            bool bIsAllowed = (TestDecorator != null) ? TestDecorator.WrappedCanExecute() : false;

            if (!bIsAllowed)
            {
                result = false;
                break;
            }
        }
        return result;
    }
    public void NotifyDecoratorsOnActivation(int childIndex)
    {
        TaskNode childNode = tasks[childIndex];
        for (int decoratorIndex = 0; decoratorIndex < childNode.decorators.Count; decoratorIndex++)
        {
            Decorator DecoratorOb = childNode.decorators[decoratorIndex];
            DecoratorOb.WrappedOnNodeActivation(this);
        }
    }
    public bool IsValidIndex(int index)
    {
        return 0 >= index && index < tasks.Count;
    }
    public int GetNextChild(int lastIndex)
    {
        int nextChildIndex = (int)ENodeResult.ReturnToParent;
        if (overrideChild != (int)ENodeResult.NotInitialized)
        {
            nextChildIndex = overrideChild;
            overrideChild = (int)ENodeResult.NotInitialized;
        }
        else
        {
            if (lastIndex + 1 < tasks.Count)
            {
                nextChildIndex = (lastIndex + 1);
            }
        }
        return nextChildIndex;
    }
}
public class CombatCompositeNode : CompositeNode
{
    public CombatCompositeNode()
    {
        status = EStatus.Combat;
    }
}
public class PatrolCompositeNode : CompositeNode
{
    public PatrolCompositeNode()
    {
        status = EStatus.Patrol;
    }
}
public static class EditorTool
{

    public static IEnumerable DecoratorTypes = new ValueDropdownList<Decorator>()
    {
        { "循环次数",new LoopDecorator()},
    };
    public static IEnumerable dataTypes = new ValueDropdownList<CompositeNode>()
    {
        { EStatus.Patrol.ToString(),new PatrolCompositeNode()},
        { EStatus.Combat.ToString(),new PatrolCompositeNode()},
    };
    public static IEnumerable TaskTypes = new ValueDropdownList<TaskNode>()
    {
        { "移动任务", new MoveTaskNode() },
        { "射击任务", new ShootTaskNode() },
    };
}
public class Decorator : Node
{
    [HideInInspector]
    public int childIndex;
    public bool WrappedCanExecute()
    {
        return true;
    }
    public void WrappedOnNodeActivation(CompositeNode compositeNode)
    {
        OnNodeActivation(compositeNode);
    }
}
[LabelText("循环次数")]
public class LoopDecorator : Decorator
{
    public int loopNum = 0;

    public override void OnNodeActivation(CompositeNode compositeNode)
    {
        base.OnNodeActivation(compositeNode);
        if (loopNum > 0)
        {
            compositeNode.overrideChild = childIndex;
        }
    }
}
