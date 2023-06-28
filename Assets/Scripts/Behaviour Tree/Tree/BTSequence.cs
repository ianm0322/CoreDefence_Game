namespace BT
{
    public abstract class BTSequence : BTNode
    {
        private BTNode _sequenceNode;

        public BTSequence()
        {
            _sequenceNode = GenerateSequence();
        }

        protected override BTState OnUpdate()
        {
            return _sequenceNode.Evaluate();
        }

        protected abstract BTNode GenerateSequence();
    }
}