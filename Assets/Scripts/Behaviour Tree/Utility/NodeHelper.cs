using System;
namespace BT
{
    public static class NodeHelper
    {
        #region Execution Task
        public static BTNode Call(Action action) => new CallNode(action);
        public static BTNode Run(Func<BTState> func) => new ActionNode(func);
        public static BTNode Return(BTState state) => new ResultNode(state);
        public static BTNode Wait(float duration) => new WaitForSecondNode(duration);
        #endregion Execution Task

        #region Composite Task
        public static BTNode Select(params BTNode[] nodes) => new SelectorNode(nodes);
        public static BTNode Parallel(params BTNode[] nodes) => new ParallelNode(nodes);
        public static BTNode Or(params BranchNode[] nodes) => new ParallelNode(nodes);
        public static BTNode Sequence(params BTNode[] nodes) => new SequenceNode(nodes);
        public static BTNode And(params BranchNode[] nodes) => new SequenceNode(nodes);
        #endregion Composite Task

        #region Decorator Task
        public static ConditionalNode Condition(Func<bool> cond) => new ConditionalNode(cond);
        public static ConditionalNode Boolean(bool boolean) => new ConditionalNode(boolean);
        public static BranchNode If(Func<bool> cond, BTNode node) => new BranchNode(cond, node);
        public static BranchNode IfElse(Func<bool> cond, BTNode successNode, BTNode failureNode) => new BranchNode(cond, successNode, failureNode);
        public static BranchNode Branch(Func<bool> cond, BTNode successNode, BTNode failureNode) => new BranchNode(cond, successNode, failureNode);
        public static TryNode Try(Func<bool> cond, BTNode node) => new TryNode(cond, node);
        public static WhileNode While(Func<bool> cond, BTNode node) => new WhileNode(cond, node);
        public static RepeatNode For(int count, BTNode node) => new RepeatNode(count, node);
        public static RepeatForSecondNode ForSecond(float duration, BTNode node) => new RepeatForSecondNode(duration, node, stopOnFail: true);
        public static InverseNode Not(BTNode node) => new InverseNode(node);
        public static UntilSuccessNode UntilSuccess(BTNode node) => new UntilSuccessNode(node);
        public static UntilFailureNode UntilFail(BTNode node) => new UntilFailureNode(node);
        public static FailNode Fail(BTNode node) => new FailNode(node);
        public static SuccessNode Success(BTNode node) => new SuccessNode(node);
        #endregion Decorator Task

        #region Others
        public static RootNode Root(BTNode node) => new RootNode(node);

        public static BTState TRUE => BTState.Success;
        public static BTState FALSE => BTState.Failure;
        public static BTState CONTINUE => BTState.Running;
        #endregion Others
    }
}