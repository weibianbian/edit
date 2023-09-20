using System.Collections.Generic;

namespace RailShootGame
{
    public class FGameplayTagNode
    {
        public string Tag;
        public FGameplayTagContainer CompleteTagWithParents = new FGameplayTagContainer();
        public List<FGameplayTagNode> ChildTags = new List<FGameplayTagNode>();
        public FGameplayTagNode ParentNode;

        public FGameplayTagNode(string InTag, string InFullTag, FGameplayTagNode InParentNode)
        {
            ParentNode = InParentNode;
            Tag = InTag;

            CompleteTagWithParents.GameplayTags.Add(new FGameplayTag(InFullTag));
        }
        public FGameplayTagNode()
        {

        }
        public FGameplayTag GetCompleteTag()
        {
            if (CompleteTagWithParents.GameplayTags.Count > 0)
            {
                return CompleteTagWithParents.GameplayTags[0];
            }
            else
            {
                return FGameplayTag.EmptyTag;
            }
        }
        public string GetSimpleTagName()
        {
            return Tag;
        }
        public List<FGameplayTagNode> GetChildTagNodes()
        {
            return ChildTags;
        }
    }
}

