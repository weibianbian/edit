using RailShootGame;
using System;

namespace HFSMRuntime
{
    public interface ICondition
    {
        bool Test(Game g, Actor e);
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
    public class SoundSensorCondition : ICondition
    {
        public bool Test(Game g, Actor e)
        {
            SoundSensor soundSensor = e.sensor.GetSensor(Sensor.ESensorType.Sound) as SoundSensor;
            if (soundSensor != null)
            {
              return  soundSensor.gunSound;

            }
            return false;
        }
    }
}

