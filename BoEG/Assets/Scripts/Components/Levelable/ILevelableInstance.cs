using Core;

namespace Components.Levelable
{
    public interface ILevelableInstance
    {
        float Experience { get; }
        int Level { get; }
        
    }
}