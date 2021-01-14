using System.Collections.Generic;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public class Commandable : ActorModule, ICommandable, IRespawnable
    {
        private readonly LinkedList<ICommand> _commandQueue;
        private ICommand _activeCommand;


        public Commandable(Actor actor) : base(actor)
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

        private void PreStep(float deltaTime)
        {
            _activeCommand?.PreStep(deltaTime);
        }

        private void Step(float deltaStep)
        {
            _activeCommand?.Step(deltaStep);
        }

        private void PostStep(float deltaTick)
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

        private void PhysicsStep(float deltaTick)
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

        public void Respawn()
        {
            ClearCommands();
        }

        public void Register(IStepableEvent source)
        {
            source.PreStep += PreStep;
            source.Step += Step;
            source.PhysicsStep += PhysicsStep;
            source.PostStep += PostStep;
        }

        public void Unregister(IStepableEvent source)
        {
            source.PreStep -= PreStep;
            source.Step -= Step;
            source.PhysicsStep -= PhysicsStep;
            source.PostStep -= PostStep;
        }
    }
}