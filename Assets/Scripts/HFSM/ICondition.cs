namespace HFSM
{
    public interface ICondition
    {
        bool Test(Game g, Entity e);
    }
}

