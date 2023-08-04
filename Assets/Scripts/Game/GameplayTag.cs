using Sirenix.Utilities;
using System;
using System.Collections.Generic;

namespace RailShootGame
{
    public class GameplayTag
    {
        public string TagName;

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
    public class GameplayTagNode
    {
        public string Tag;
        public GameplayTagContainer CompleteTagWithParents;
        public List<GameplayTagNode> ChildTags;
        public GameplayTagNode ParentNode;
    }
    public class GameplayTagContainer { }
    public class GameplayTagsManager
    {
        public GameplayTagNode GameplayRootTag;
        public Dictionary<GameplayTag, GameplayTagNode> GameplayTagNodeMap = new Dictionary<GameplayTag, GameplayTagNode>();

        public void ConstructGameplayTagTree()
        {
            GameplayRootTag = new GameplayTagNode();

        }

    }
}

