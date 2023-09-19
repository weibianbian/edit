using Sirenix.Utilities;
using System;

namespace RailShootGame
{
    public class FGameplayTag
    {
        public string TagName;
        public static FGameplayTag EmptyTag = new FGameplayTag();
        public FGameplayTag(string tagName)
        {
            TagName = tagName;
        }
        public FGameplayTag()
        {
            TagName=string.Empty;
        }
        public override bool Equals(object obj)
        {
            return obj is FGameplayTag tag &&
                   TagName == tag.TagName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TagName);
        }

        public static bool operator ==(FGameplayTag a, FGameplayTag b)
        {
            return a.TagName == b.TagName;
        }
        public static bool operator !=(FGameplayTag a, FGameplayTag b)
        {
            return a.TagName != b.TagName;
        }
    }
}

