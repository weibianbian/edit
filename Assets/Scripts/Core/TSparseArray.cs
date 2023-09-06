using System;

namespace Core
{
    public class FFreeListLink
    {
        /** 如果元素没有分配,这是一个链接到前一个元素的数组的空闲列表. */
        public int PrevFreeIndex;

        /** 如果元素没有被分配，这是一个到数组未分配列表中的下一个元素的链接。. */
        public int NextFreeIndex;
    };

    public class FElementOrFreeListLink<T>
    {
        public T ElementData;
        public FFreeListLink FreeListLink;
    }

    public class TSparseArray<T>
    {
        public FElementOrFreeListLink<T>[] Data;
        public int FirstFreeIndex = 0;
        public int NumFreeIndices = 0;

        FFreeListLink FreeListLink;
        public TSparseArray()
        {
            Data = new FElementOrFreeListLink<T>[0];
            FirstFreeIndex = -1;
            NumFreeIndices = 0;
            FreeListLink = new FFreeListLink();
            FreeListLink.NextFreeIndex = FirstFreeIndex;
        }
        public int Num()
        {
            return Data.Length - NumFreeIndices;
        }
        public int GetMaxIndex()
        {
            return Data.Length;
        }
        public void RemoveAt(int Index)
        {
            if (NumFreeIndices > 0)
            {
                Data[FirstFreeIndex].FreeListLink.PrevFreeIndex = Index;
            }
            Data[Index].ElementData = default(T);
            Data[Index].FreeListLink.PrevFreeIndex = -1;
            Data[Index].FreeListLink.NextFreeIndex = NumFreeIndices > 0 ? FirstFreeIndex : -1;
            FirstFreeIndex = Index;
            ++NumFreeIndices;
        }
        public int Add(T Element)
        {
            int Index = 0;
            if (NumFreeIndices > 0)
            {
                Index = FirstFreeIndex;
                FirstFreeIndex = GetFreeListLink(FirstFreeIndex).NextFreeIndex;
                --NumFreeIndices;
            }
            else
            {
                int OldNum = Data.Length;
                ResizeTo(Data.Length + 1);
                Index= OldNum;
            }
            if (Data[Index] == null)
            {
                Data[Index] = new FElementOrFreeListLink<T>();
            }
            Data[Index].ElementData = Element;
            if (NumFreeIndices > 0)
            {
                Data[FirstFreeIndex].FreeListLink.PrevFreeIndex = -1;
            }
            return Index;
        }
        public T GetData(int Index)
        {
            return Data[Index].ElementData;
        }
        public void ResizeTo(int NewNum)
        {
            int OldNum = Data.Length;
            FElementOrFreeListLink<T>[] OldData = Data;
            FElementOrFreeListLink<T>[] NewData = new FElementOrFreeListLink<T>[NewNum];
            Array.Copy(OldData, 0, NewData, 0, OldNum);

            Data = NewData;

        }
        public FFreeListLink GetFreeListLink(int Index)
        {
            FElementOrFreeListLink<T> Element = Data[Index];
            return Element.FreeListLink;
        }
        public T this[int Index]
        {
            get
            {
                return GetData(Index);
            }
        }
    }
}
