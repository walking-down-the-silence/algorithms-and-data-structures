using System;
using System.Collections.Generic;

namespace Silent.Collections
{
    /// <summary>
    /// The implementation of classic AVL Tree data structure
    /// </summary>
    /// <seealso cref="http://www.cise.ufl.edu/~nemo/cop3530/AVL-Tree-Rotations.pdf"/>
    /// <typeparam name="TValue"> The <see cref="TValue"/> type. </typeparam>
    public class AvlTree<TValue> where TValue : IComparable<TValue>
    {
        private readonly IComparer<TValue> _comparer;
        private TreeNode<TValue> _root;

        public AvlTree(IComparer<TValue> comparer)
        {
            _comparer = comparer;
        }

        public int Count { get; private set; }

        public int Height => _root == null ? 0 : _root.Height;

        public void Insert(TValue value)
        {
            _root = InternalInsert(_root, value);
            Count++;
        }

        private TreeNode<TValue> InternalInsert(TreeNode<TValue> parent, TValue value)
        {
            if (parent == null)
            {
                return new TreeNode<TValue>(value);
            }

            if (_comparer.Compare(value, parent.Value) < 0)
            {
                parent.LeftBranch = InternalInsert(parent.LeftBranch, value);
            }
            else
            {
                parent.RightBranch = InternalInsert(parent.RightBranch, value);
            }

            UpdateNodeHeight(parent);
            return BalanceTheTree(parent);
        }

        private void UpdateNodeHeight(TreeNode<TValue> node)
        {
            node.Height = Math.Max(GetNodeHeight(node.LeftBranch), GetNodeHeight(node.RightBranch)) + 1;
        }

        private int GetNodeHeight(TreeNode<TValue> node)
        {
            if (node == null)
            {
                return 0;
            }

            return node.Height;
        }

        private int GetNodeBalance(TreeNode<TValue> root)
        {
            return root == null ? 0 : GetNodeHeight(root.LeftBranch) - GetNodeHeight(root.RightBranch);
        }

        private bool IsRightHeavy(TreeNode<TValue> root) => GetNodeBalance(root) < -1;

        private bool IsLeftHeavy(TreeNode<TValue> root) => GetNodeBalance(root) > 1;

        private TreeNode<TValue> BalanceTheTree(TreeNode<TValue> parent)
        {
            TreeNode<TValue> newRoot = parent;

            if (IsRightHeavy(parent))
            {
                newRoot = IsLeftHeavy(parent.RightBranch)
                    ? LeftRightRotation(parent)
                    : LeftRotation(parent);
            }
            else if(IsLeftHeavy(parent))
            {
                newRoot = IsRightHeavy(parent.LeftBranch)
                    ? RightLeftRotation(parent)
                    : RightRotation(parent);
            }

            UpdateNodeHeight(newRoot);

            return newRoot;
        }

        private TreeNode<TValue> LeftRotation(TreeNode<TValue> root)
        {
            // Figure 1 - 1
            //   a          b      |    3            5
            //    \        / \     |     \          / \ 
            //     b      a   c    |      5        3   7
            //    / \      \       |     / \        \ 
            //   d   c      d      |    4   7        4
            // b becomes the new root.
            // a takes ownership of b's left child as its right child, or in this case - d.
            // b takes ownership of a as its left child.
            TreeNode<TValue> newRoot = root.RightBranch;
            root.RightBranch = newRoot.LeftBranch;
            newRoot.LeftBranch = root;

            UpdateNodeHeight(newRoot.LeftBranch);
            UpdateNodeHeight(newRoot.RightBranch);

            return newRoot;
        }

        private TreeNode<TValue> RightRotation(TreeNode<TValue> root)
        {
            // Figure 1 - 2
            //       c      b      |        7        5
            //      /      / \     |       /        / \ 
            //     b      a   c    |      5        3   7
            //    / \        /     |     / \          /
            //   a   d      d      |    3   6        6
            // b becomes the new root. 
            // c takes ownership of b's right child, as its left child or in this case - d.
            // b takes ownership of c, as it's right child. 
            TreeNode<TValue> newRoot = root.LeftBranch;
            root.LeftBranch = newRoot.RightBranch;
            newRoot.RightBranch = root;

            UpdateNodeHeight(newRoot.LeftBranch);
            UpdateNodeHeight(newRoot.RightBranch);

            return newRoot;
        }

        private TreeNode<TValue> LeftRightRotation(TreeNode<TValue> root)
        {
            // Figure 1 - 3
            //     c         c       b    
            //    /         /       / \   
            //   a         b       a   c  
            //    \       /               
            //     b     a                
            root.LeftBranch = LeftRotation(root.LeftBranch);
            return RightRotation(root);
        }

        private TreeNode<TValue> RightLeftRotation(TreeNode<TValue> root)
        {
            // Figure 1 - 3
            //   a       a           b    
            //    \       \         / \   
            //     с       b       a   c  
            //    /         \             
            //   b           c            
            root.RightBranch = RightRotation(root.RightBranch);
            return LeftRotation(root);
        }
    }
}
