using System.Collections.Generic;

namespace GameplayAbilitySystem
{
    public class FGameplayTagQuery
    {
        public List<int> QueryTokenStream;

        public bool IsEmpty()
        {
            return QueryTokenStream.Count == 0;
        }
    }

}

