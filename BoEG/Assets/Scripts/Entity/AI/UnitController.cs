using System;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Commands;
using UnityEngine;

namespace MobaGame.Entity.AI
{
    // TODO abstract this into a 'Controller' which can be used for AI or Players.
    [RequireComponent(typeof(Actor))]
    public class UnitController : ActorBehaviour
    {
        private ICommandable _commandable;

        protected void Start()
        {
            if (_commandable == null && !Self.TryGetModule(out _commandable))
                throw new NullReferenceException("Actor does not have commandable!");
        }

        //V1
        //We want to attack move towards a position, until we die
        public void SetAttackTarget(Vector3 position)
        {
            if (_commandable == null && !Self.TryGetModule(out _commandable))
                throw new NullReferenceException("Actor does not have commandable!");
            //TODO
            var attackMoveCommand = new AttackMoveCommand(gameObject, position);
            _commandable.InterruptCommand(attackMoveCommand);
        }
    }
}