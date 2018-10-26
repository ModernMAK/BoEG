using UnityEditor;
using UnityEngine;

namespace EditorSpace
{
    public static class PropertyDrawerUtil
    {
        private static string CreateTabs(int d)
        {
            var str = "";
            for (var i = 0; i < d; i++)
                str += "--";
            return str + ">";
        }

        private static string Scan(SerializedProperty prop)
        {
            var str = CreateTabs(prop.depth) + prop.displayName + "\n";
            if (prop.hasChildren)
            {
                var child = prop.Copy();
                child.Next(true);
                do
                {
                    str += Scan(child);
                } while (child.Next(false));
            }

            return str;
        }

        public static void PrintProperty(SerializedProperty prop)
        {
            Debug.Log(Scan(prop));
        }
    }
}