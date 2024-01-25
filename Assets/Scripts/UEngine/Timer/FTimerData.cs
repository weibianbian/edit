namespace UEngine.Timer
{
    public class FTimerData
    {
        public bool bLoop;
        public bool bRequiresDelegate;
        public ETimerStatus Status;
        public float Rate;
        public double ExpireTime;
        public ITimerDelegate TimerDelegate;
        public FTimerHandle Handle;
    }
}

