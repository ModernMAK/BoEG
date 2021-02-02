using MobaGame.Framework.Types;

namespace MobaGame.Framework.Core.Modules
{
    public class CritEffect : IListener<IAttackerable>
    {
        private readonly RandomProc _proc;
        private readonly float _critMultiplier;
        public CritEffect(RandomProc proc, float critMultiplier)
        {
            _proc = proc;
            _critMultiplier = critMultiplier;
        }
        private void CritProc(object sender, AttackCritEventArgs e)
        {
            if (e.CriticalMultiplier > _critMultiplier)
                return;

            if (_proc.Proc())
            {
                e.CriticalMultiplier = _critMultiplier;
            }
        }
        public void Register(IAttackerable source)
        {
            source.CritModifiers += CritProc;
        }

        public void Unregister(IAttackerable source)
        {
            source.CritModifiers -= CritProc;
        }
    }
}