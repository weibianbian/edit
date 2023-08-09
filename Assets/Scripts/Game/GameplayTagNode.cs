using System.Collections.Generic;

namespace RailShootGame
{
    public class GameplayTagNode
    {
        public string Tag;
        public GameplayTagContainer CompleteTagWithParents=new GameplayTagContainer();
        public List<GameplayTagNode> ChildTags;
        public GameplayTagNode ParentNode;

        public GameplayTagNode(string InTag, string InFullTag, GameplayTagNode InParentNode)
        {
            ParentNode = InParentNode;
            Tag= InTag;

            CompleteTagWithParents.GameplayTags.Add(new GameplayTag(InFullTag));
        }
        public GameplayTagNode()
        {

        }
        public GameplayTag GetCompleteTag()
        {
            return CompleteTagWithParents.GameplayTags[0];
        }
    }
}

