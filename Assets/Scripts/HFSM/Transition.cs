using System.Collections.Generic;
using UEngine;
using UEngine.GameFramework;

namespace HFSMRuntime
{
    public interface ITransition
    {
        bool IsTriggered(UWorld g, AActor e);
        State GetTargetState();
        List<IAction> GetActions();
        int GetLevel();
    }

    public class Transition : ITransition
    {
        public List<IAction> actions;
        public int level = 0;
        public State targetState;

        public ICondition condition;
        public Transition(ICondition condition, State targetState, int level)
        {
            this.condition = condition;
            this.targetState = targetState;
            actions = new List<IAction>();
            this.level = level;
        }
        public bool IsTriggered(UWorld g, AActor e)
        {
            return condition.Test(g,e);
        }
        public State GetTargetState()
        {
            return targetState;
        }
        public void AddActions(IAction a)
        {
            actions.Add(a);
        }
        public virtual List<IAction> GetActions()
        {
            return new List<IAction>(0);
        }
        public int GetLevel()
        {
            return this.level;
        }
    }
}

