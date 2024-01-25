namespace UEngine.Timer
{
    public struct FTimerHandle
    {
        public const int IndexBits = 24;
        public const int SerialNumberBits = 40;
        public const int MaxIndex =(int)(1 << IndexBits);
        public const ulong MaxSerialNumber = (ulong)1 << SerialNumberBits;
        public ulong Handle;
        public bool IsValid()
        {
            return Handle != 0;
        }
        public void Invalidate()
        {
            Handle = 0;
        }
        public int GetIndex()
        {
            return (int)(Handle & (ulong)(MaxIndex - 1));
        }
        public void SetIndexAndSerialNumber(int Index, ulong SerialNumber)
        {
            Handle = (SerialNumber << IndexBits) | ((ulong)(uint)Index);
        }
        public ulong GetSerialNumber()
        {
            return Handle >> IndexBits;
        }
        public static bool operator !=(FTimerHandle a, FTimerHandle b)
        {
            return a.Handle != b.Handle;
        }
        public static bool operator ==(FTimerHandle a, FTimerHandle b)
        {
            return a.Handle == b.Handle;
        }
    }
}

