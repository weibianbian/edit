using System.Collections.Generic;

namespace RailShootGame
{
    public class GameplayTagsManager
    {
        public GameplayTagNode GameplayRootTag;
        public Dictionary<GameplayTag, GameplayTagNode> GameplayTagNodeMap = new Dictionary<GameplayTag, GameplayTagNode>();

        public void ConstructGameplayTagTree()
        {
            GameplayRootTag = new GameplayTagNode();

        }
        public void PopulateTreeFromDataTable()
        {

        }
        //处理单条数据
        public void AddTagTableRow()
        {
            string FullTagString = string.Empty;
            string[] SubTags = FullTagString.Split('.');
            int NumSubTags = SubTags.Length;
            for (int SubTagIdx = 0; SubTagIdx < NumSubTags; SubTagIdx++)
            {
                bool bIsExplicitTag = (SubTagIdx == (NumSubTags - 1));
                string ShortTagName = SubTags[SubTagIdx];
                string FullTagName;
                if (bIsExplicitTag)
                {
                    FullTagName = FullTagString;
                }
                else if (SubTagIdx == 0)
                {
                    // Full tag is the same as short tag, and start building full tag string
                    FullTagName = ShortTagName;
                    FullTagString = SubTags[SubTagIdx];
                }
                else
                {
                    // Add .Tag and use that as full tag
                    FullTagString += (".");
                    FullTagString += SubTags[SubTagIdx];

                    FullTagName = FullTagString;
                }
            }
        }
        public GameplayTag RequestGameplayTag(string TagName)
        {
            GameplayTag PossibleTag = new GameplayTag(TagName);
            if (GameplayTagNodeMap.ContainsKey(PossibleTag))
            {
                return PossibleTag;
            }
            return new GameplayTag();
        }
        public void InsertTagIntoNodeArray(string Tag, string FullTag, GameplayTagNode ParentNode)
        {
            GameplayTagNode TagNode = new GameplayTagNode(Tag, FullTag, ParentNode);
            GameplayTag GameplayTag = TagNode.GetCompleteTag();
            GameplayTagNodeMap.Add(GameplayTag, TagNode);
        }
    }
}

