using System.Collections.Generic;

namespace RailShootGame
{
    public class FGameplayTagContainer
    {
        public List<GameplayTag> GameplayTags=new List<GameplayTag>();
        public List<GameplayTag> ParentTags = new List<GameplayTag>();

        public int Count()
        {
            return GameplayTags.Count;
        }

    }
}

