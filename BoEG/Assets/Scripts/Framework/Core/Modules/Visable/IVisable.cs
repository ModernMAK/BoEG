using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public interface IVisionSource
	{
        float Range { get; }
        Vector3 Position { get; }
	}
    public interface IVisable
    {
        /// <summary>
        /// A flag for whether the actor has invisibility.
        /// </summary>
        public bool IsInvisible { get; }
        /// <summary>
        /// A flag for whether the unit has any 'spotted' effects.
        /// </summary>
        /// <remarks>
        /// Spotted reveals invisibility.
        /// </remarks>
        public bool IsSpotted { get; }

        /// <summary>
        /// Is the unit visible to this team?
        /// </summary>
        public bool IsVisible(TeamData team);

        // What is this?
        // bool IsHidden { get; }
        //public bool IsHidden(TeamData team);


    }
}