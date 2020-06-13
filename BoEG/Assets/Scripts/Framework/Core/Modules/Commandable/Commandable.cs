using System.Collections.Generic;
using Framework.Types;
using UnityEngine;

namespace Framework.Core.Modules
{
    public class Commandable : MonoBehaviour, ICommandable
    {
        private ICommand _activeCommand;
        private readonly LinkedList<ICommand> _commandQueue;


        public Commandable()
        {
            _commandQueue = new LinkedList<ICommand>();
            _activeCommand = null;
        }

        public void InterruptCommand(ICommand command)
        {
            StopActiveCommand();
            InsertAtFront(command);
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

        public void PreStep(float deltaTime)
        {
            _activeCommand?.PreStep(deltaTime);
        }

        public void Step(float deltaStep)
        {
            _activeCommand?.Step(deltaStep);
        }

        public void PostStep(float deltaTick)
        {
            if (_activeCommand != null)
            {
                _activeCommand.PostStep(deltaTick);
                if (_activeCommand.IsDone())
                {
                    _activeCommand.Stop();
                    _activeCommand = null;
                }
            }

            if (_activeCommand == null && _commandQueue.Count > 0)
            {
                _activeCommand = Dequeue();
                _activeCommand.Start();
            }
        }

        public void PhysicsStep(float deltaTick)
        {
            _activeCommand?.PhysicsStep(deltaTick);
        }

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

        private void StopActiveCommand()
        {
            if (_activeCommand != null)
            {
                _activeCommand.Stop();
                _activeCommand = null;
            }
        }
    }
}