using Sirenix.Utilities;
using System;

namespace RailShootGame
{
    public class GameplayTag
    {
        public string TagName;

        public GameplayTag(string tagName)
        {
            TagName = tagName;
        }
        public GameplayTag()
        {
            TagName=string.Empty;
        }
        public override bool Equals(object obj)
        {
            return obj is GameplayTag tag &&
                   TagName == tag.TagName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TagName);
        }

        public static bool operator ==(GameplayTag a, GameplayTag b)
        {
            return a.TagName == b.TagName;
        }
        public static bool operator !=(GameplayTag a, GameplayTag b)
        {
            return a.TagName != b.TagName;
        }
    }
}

