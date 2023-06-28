namespace BT
{
    public class ResultNode : ExecutionNode
    {
        public BTState _state;

        public ResultNode(BTState state)   
        {
            this._state = state;
        }

        protected override BTState OnUpdate()
        {
            return _state;
        }
    }
}