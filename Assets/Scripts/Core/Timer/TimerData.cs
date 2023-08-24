namespace RailShootGame
{
    public class TimerData
    {
        public bool bLoop;
        public bool bRequiresDelegate;
        public ETimerStatus Status;
        public float Rate;
        public double ExpireTime;
        public ITimerDelegate TimerDelegate;
        public TimerHandle Handle;
    }
}

