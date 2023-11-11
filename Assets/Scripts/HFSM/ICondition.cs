using RailShootGame;
using System;

namespace HFSMRuntime
{
    public interface ICondition
    {
        bool Test(UWorld g, AActor e);
    }
    public class RandomTimerCondition : ICondition
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

        public bool Test(UWorld g, AActor e)
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
    //public class SoundSensorCondition : ICondition
    //{
       
    //}
}

