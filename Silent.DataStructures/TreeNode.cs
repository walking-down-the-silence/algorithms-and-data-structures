namespace Common
{
    public class TreeNode<TValue>
    {
        public TreeNode(TValue value)
        {
            Value = value;
            Height = 1;
        }

        public TValue Value { get; }

        public int Height { get; set; }

        public TreeNode<TValue> LeftBranch { get; set; }

        public TreeNode<TValue> RightBranch { get; set; }

        public override string ToString() => $"Value: {Value}, Height: {Height}";
    }
}