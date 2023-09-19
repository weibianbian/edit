using System.Collections.Generic;

namespace RailShootGame
{
    public class GameplayTagsManager
    {
        public List<string> texts = new List<string>()
        {
            { "Damage"},
            { "Damage.Basic"},
            { "Damage.Type1"},
            { "Damage.Buffed.FireBuff"},
            { "Damage.Mitigated.Armor"},
        };
        public FGameplayTagNode GameplayRootTag;
        public Dictionary<FGameplayTag, FGameplayTagNode> GameplayTagNodeMap = new Dictionary<FGameplayTag, FGameplayTagNode>();

        public void ConstructGameplayTagTree()
        {
            GameplayRootTag = new FGameplayTagNode();

            for (int i = 0; i < texts.Count; i++)
            {
                AddTagTableRow(texts[i]);
            }

        }
        public void PopulateTreeFromDataTable()
        {

        }
        //处理单条数据
        public void AddTagTableRow(string TagRow)
        {
            FGameplayTagNode CurNode = GameplayRootTag;
            string FullTagString = TagRow;
            string[] SubTags = FullTagString.Split('.');
            int NumSubTags = SubTags.Length;
            for (int SubTagIdx = 0; SubTagIdx < NumSubTags; SubTagIdx++)
            {
                bool bIsExplicitTag = (SubTagIdx == (NumSubTags - 1));
                string ShortTagName = SubTags[SubTagIdx];
                string FullTagName;
                if (bIsExplicitTag)
                {
                    //我们已经知道了它的最终名字
                    FullTagName = FullTagString;
                }
                else if (SubTagIdx == 0)
                {
                    //完整标签等同于短标签，并开始构建完整标签字符串
                    FullTagName = ShortTagName;
                    FullTagString = SubTags[SubTagIdx];
                }
                else
                {
                    // 添加。tag并将其用作完整标签
                    FullTagString += (".");
                    FullTagString += SubTags[SubTagIdx];

                    FullTagName = FullTagString;
                }
                List<FGameplayTagNode> ChildTags = CurNode.GetChildTagNodes();
                int InsertionIdx = InsertTagIntoNodeArray(ShortTagName, FullTagName, CurNode, ChildTags);
                CurNode = ChildTags[InsertionIdx];
            }
        }
        public FGameplayTag RequestGameplayTag(string TagName)
        {
            FGameplayTag PossibleTag = new FGameplayTag(TagName);
            if (GameplayTagNodeMap.ContainsKey(PossibleTag))
            {
                return PossibleTag;
            }
            return new FGameplayTag();
        }
        public int InsertTagIntoNodeArray(string Tag, string FullTag, FGameplayTagNode ParentNode, List<FGameplayTagNode> NodeArray)
        {
            int FoundNodeIdx = -1;
            int WhereToInsert = -1;
            int LowerBoundIndex = 0;
            if (LowerBoundIndex < NodeArray.Count)
            {
                FGameplayTagNode CurrNode = NodeArray[LowerBoundIndex];
                if (CurrNode.GetSimpleTagName() == Tag)
                {
                    FoundNodeIdx = LowerBoundIndex;
                }
                else
                {
                    //在此之前插入新节点
                    WhereToInsert = LowerBoundIndex;
                }
            }
            if (FoundNodeIdx == -1)
            {
                if (WhereToInsert == -1)
                {
                    WhereToInsert = NodeArray.Count;
                }
                FGameplayTagNode TagNode = new FGameplayTagNode(Tag, FullTag, ParentNode);
                NodeArray.Insert(WhereToInsert, TagNode);
                FoundNodeIdx = WhereToInsert;
                FGameplayTag GameplayTag = TagNode.GetCompleteTag();
                GameplayTagNodeMap.Add(GameplayTag, TagNode);
            }
            return FoundNodeIdx;
        }
    }
}

