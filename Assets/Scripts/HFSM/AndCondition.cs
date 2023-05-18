namespace HFSM
{
    public class AndCondition : ICondition
    {
        public ICondition conditionA;
        public ICondition conditionB;
        public bool Test(Game g,Entity e)
        {
            return conditionA.Test(g,e) && conditionB.Test(g,e);
        }
    }
}

