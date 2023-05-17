namespace HFSM
{
    public class AndCondition : Condition
    {
        public Condition conditionA;
        public Condition conditionB;
        public override bool Test()
        {
            return conditionA.Test() && conditionB.Test();
        }
    }
}

