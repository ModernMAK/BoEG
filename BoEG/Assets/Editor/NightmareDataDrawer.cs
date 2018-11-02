//using Entity.Abilities.DarkHeart;
//using UnityEditor;
//
//namespace EditorSpace
//{
//    [CustomPropertyDrawer(typeof(Nightmare.NightmareData))]
//    public class NightmareDataDrawer : BetterAbilityDataDrawer
//    {
//        private static readonly string[] PropertyPaths =
//            {"_manaCost", "_castRange", "_tickInfo", "_totalDamage"};
//
//
//        protected override string[] SubPropertyPaths
//        {
//            get { return PropertyPaths; }
//        }
//    }
//    [CustomPropertyDrawer(typeof(NetherCurse.NetherCurseData))]
//    public class NetherCurseDataDrawer : BetterAbilityDataDrawer
//    {
//        private static readonly string[] PropertyPaths =
//            {"_manaCost", "_castRange", "_tickInfo", "_totalDamage", "_areaOfEffect"};
//
//
//        protected override string[] SubPropertyPaths
//        {
//            get { return PropertyPaths; }
//        }
//    }
//}