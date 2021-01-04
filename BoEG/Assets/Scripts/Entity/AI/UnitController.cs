using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Commands;
using UnityEngine;

namespace MobaGame.Entity.AI
{
    public class UnitController : MonoBehaviour
    {
        private ICommandable _commandable;

        private void Awake()
        {
            _commandable = GetComponent<ICommandable>();
        }

        //V1
        //We want to attack move towards a position, until we die
        public void SetAttackTarget(Vector3 position)
        {
            //TODO
            var attackMoveCommand = new AttackMoveCommand(gameObject, position);
            _commandable.InterruptCommand(attackMoveCommand);
        }
    }
}