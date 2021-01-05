using UnityEngine;

namespace MobaGame.Framework.Core.Networking.Tracking
{
    public static class SingletonUtility
    {
        public const string ContainerName = "Singletons";

        public static Transform GetSingletonContainer()
        {
            var go = GameObject.Find(ContainerName);
            if (go == null)
                go = new GameObject(ContainerName);
            return go.transform;
        }

        public static Transform GetDesiredContainer(string name)
        {
            var container = GetSingletonContainer();
            var transform = container.Find(ContainerName);
            if (transform == null)
            {
                transform = new GameObject(ContainerName).transform;
                transform.SetParent(GetSingletonContainer());
            }

            return transform;
        }

        private static T CreateSingleton<T>(string name, Transform parent) where T : Component
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent);
            return go.AddComponent<T>();
        }

        private static T CreateSingleton<T>(string name) where T : Component =>
            CreateSingleton<T>(name, GetSingletonContainer());

        private static T CreateSingleton<T>(string name, string parent) where T : Component =>
            CreateSingleton<T>(name, GetDesiredContainer(parent));

        public static T GetInstance<T>(ref T instance, string name) where T : Component
        {
            //Located, return it
            if (instance != null) return instance;
            //Does it exist?
            instance = Object.FindObjectOfType<T>();
            if (instance != null) return instance;
            //Create it
            instance = CreateSingleton<T>(name);
            return instance;
        }

        public static T GetInstance<T>(ref T instance, string name, string parent) where T : Component
        {
            //Located, return it
            if (instance != null) return instance;
            //Does it exist?
            instance = Object.FindObjectOfType<T>();
            if (instance != null) return instance;
            //Create it
            instance = CreateSingleton<T>(name, parent);
            return instance;
        }
    }
}