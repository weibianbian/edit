using RailShootGame;

namespace HFSMRuntime
{
    public class AndCondition : ICondition
    {
        public ICondition conditionA;
        public ICondition conditionB;
        public bool Test(Game g, Actor e)
        {
            return conditionA.Test(g,e) && conditionB.Test(g,e);
        }
    }
}

