using RailShootGame;
using System;

namespace HFSMRuntime
{
    public interface ICondition
    {
        bool Test(Game g, Actor e);
    }
    class RandomTimerCondition : ICondition
    {
        int timer;
        int seconds;
        TimeSpan originalTime;
        Random random = new Random();
        public RandomTimerCondition(TimeSpan intialTime, int seconds)
        {
            originalTime = intialTime;
            this.seconds = seconds;
            timer = random.Next(seconds - 400, seconds);
        }

        public bool Test(Game g, Actor e)
        {
            --timer;
            if (timer == 0)
            {
                timer = random.Next(seconds - 400, seconds);
                return true;
            }
            return false;
        }
    }
}

