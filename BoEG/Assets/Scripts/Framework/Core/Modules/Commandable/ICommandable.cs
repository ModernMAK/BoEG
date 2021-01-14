using System;
using System.Collections.Generic;
using MobaGame.Framework.Types;

namespace MobaGame.Framework.Core.Modules
{
    public interface ICommandable : IListener<IStepableEvent>
    {
        void AddCommand(ICommand command);
        void SetCommand(ICommand command);

        void InterruptCommand(ICommand command);
        void ClearCommands();
    }
}