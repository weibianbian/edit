using System.Collections.Generic;
using UEngine.GameFramework;

namespace UEngine
{
    public class ULevel
    {
        public List<AActor> Actors = new List<AActor>();
        public UWorld OwningWorld;
    }
}