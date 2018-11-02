namespace Modules.Abilityable.Ability
{
    public interface IAbilityChannel
    {
        float MaxChannelDuration { get; }

        bool IsChanneling { get; }

        void StartChannel();

        void StopChannel();

        float ChannelProgress { get; }

        //0 -> the channel began (Or is not being channeled)
        //1 -> the channel completed
        float ChannelProgressNormalized { get; }
    }
}