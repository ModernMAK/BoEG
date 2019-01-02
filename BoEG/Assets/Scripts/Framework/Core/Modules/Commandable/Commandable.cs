using System.Collections.Generic;
using Framework.Types;

namespace Framework.Core.Modules
{
    public class Commandable : Module, ICommandable
    {
        private LinkedList<ICommand> _commandQueue;
        private ICommand _activeCommand;

        private IStepable _baseStepable;
        
        

        protected override void Awake()
        {
            _commandQueue = new LinkedList<ICommand>();
            _activeCommand = null;
        }

//
//        protected override void PreStep(float deltaStep)
//        {
//            if (_activeCommand != null)
//            {
//                _activeCommand.PreStep(deltaStep);
//            }
//        }
//
//        protected override void Step(float deltaStep)
//        {
//            if (_activeCommand != null)
//            {
//                _activeCommand.Step(deltaStep);
//            }
//        }
//
//        protected override void PostStep(float deltaTick)
//        {
//            if (_activeCommand != null)
//            {
//                _activeCommand.PostStep(deltaTick);
//                if (_activeCommand.IsDone())
//                {
//                    _activeCommand.Stop();
//                    _activeCommand = null;
//                }
//            }
//
//            if (_activeCommand == null && _commandQueue.Count > 0)
//            {
//                _activeCommand = Dequeue();
//                _activeCommand.Start();
//            }
//        }
//        protected override void PhysicsStep(float deltaTick)
//        {
//            if (_activeCommand != null)
//            {
//                _activeCommand.PhysicsStep(deltaTick);
//            }
//        }
        
        private ICommand Dequeue()
        {
            var temp = _commandQueue.Last.Value;
            _commandQueue.RemoveLast();
            return temp;
        }

        private void InsertAtFront(ICommand command)
        {
            _commandQueue.AddLast(command);
        }

        private void Enqueue(ICommand command)
        {
            _commandQueue.AddFirst(command);
        }

        public void InterruptCommand(ICommand command)
        {
            StopActiveCommand();
            InsertAtFront(command);
        }

        private void StopActiveCommand()
        {
            if (_activeCommand != null)
            {
                _activeCommand.Stop();
                _activeCommand = null;
            }
        }

        public void ClearCommands()
        {
            StopActiveCommand();
            _commandQueue.Clear();
        }

        public void SetCommand(ICommand command)
        {
            ClearCommands();
            AddCommand(command);
        }

        public void AddCommand(ICommand command)
        {
            Enqueue(command);
        }
    }
}