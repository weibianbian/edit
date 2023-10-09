using System.Collections.Generic;
using UnityEngine;

namespace UIToolkit.Runtime
{
	public class BinaryTreePacker : ITexture2DPacker
	{
        private Node root;
        private int maxWidth;
        private int maxHeight;

        public int Width => maxWidth;

        public int Height => maxHeight;

        public BinaryTreePacker() : this(1024, 1024)
        { 
        
        }

        public BinaryTreePacker(int maxWidth, int maxHeight)
        {
            this.maxWidth = maxWidth;
            this.maxHeight = maxHeight;
            root = new Node(0, 0, maxWidth, maxHeight);
        }

        public bool TryInsert(int width, int height, out RectInt position)
        {
            position = default(RectInt);

            Node node = FindNode(root, width, height);

            if (node != null)
            {
                SplitNode(node, width, height);
                position = new RectInt(node.x, node.y, width, height);
                return true;
            }

            return false;
        }

        private Node FindNode(Node currentNode, int width, int height)
        {
            if (currentNode.isOccupied)
            {
                Node foundNode = FindNode(currentNode.right, width, height);
                if (foundNode == null)
                    foundNode = FindNode(currentNode.down, width, height);
                return foundNode;
            }
            else if (currentNode.width >= width && currentNode.height >= height)
            {
                return currentNode;
            }
            else
            {
                return null;
            }
        }

        private void SplitNode(Node node, int width, int height)
        {
            node.isOccupied = true;
            node.down = new Node(node.x, node.y + height, node.width, node.height - height);
            node.right = new Node(node.x + width, node.y, node.width - width, height);
        }

        public void Remove(RectInt position)
        {
            RemoveNode(root, position);
        }

        private void RemoveNode(Node currentNode, RectInt position)
        {
            if (currentNode == null) return;

            if (currentNode.x == position.x && currentNode.y == position.y && currentNode.width == position.width && currentNode.height == position.height)
            {
                currentNode.isOccupied = false;
                return;
            }

            RemoveNode(currentNode.right, position);
            RemoveNode(currentNode.down, position);
        }

        public override string ToString()
        {
            int occupiedCount = CountOccupiedNodes(root);
            return "图集中的纹理 (BinaryTreePacker): " + occupiedCount;
        }

        private int CountOccupiedNodes(Node currentNode)
        {
            if (currentNode == null) return 0;

            int count = 0;

            if (currentNode.isOccupied)
                count++;

            count += CountOccupiedNodes(currentNode.right);
            count += CountOccupiedNodes(currentNode.down);

            return count;
        }

        private class Node
        {
            public int x;
            public int y;
            public int width;
            public int height;
            public bool isOccupied;
            public Node right;
            public Node down;

            public Node(int x, int y, int width, int height)
            {
                this.x = x;
                this.y = y;
                this.width = width;
                this.height = height;
                isOccupied = false;
                right = null;
                down = null;
            }
        }
    }
}