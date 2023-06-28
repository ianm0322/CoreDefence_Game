namespace BT
{
    public class WaitForSecondNode : ExecutionNode
    {
        private float _duration;
        private float _timer;

        public WaitForSecondNode(float duration)   
        {
            _duration = duration;
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            _timer = 0;
        }

        protected override BTState OnUpdate()
        {
            if(_timer >= _duration)
            {
                return BTState.Success;
            }
            else
            {
                return BTState.Running;
            }
        }
    }
}