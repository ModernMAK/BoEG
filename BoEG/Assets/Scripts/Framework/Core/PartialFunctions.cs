using System;

namespace MobaGame.Framework.Core
{
    public static class PartialFunctions
    {
        public static Func<T2, T3, TResult> Partial<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, T1 value)
        {
            TResult Wrapper(T2 a, T3 b)
            {
                return func(value, a, b);
            }

            return Wrapper;
        }

        public static Action<T2, T3> Partial<T1, T2, T3>(this Action<T1, T2, T3> func, T1 value)
        {
            void Wrapper(T2 a, T3 b)
            {
                func(value, a, b);
            }

            return Wrapper;
        }

        public static Func<T2, TResult> Partial<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 value)
        {
            TResult Wrapper(T2 param)
            {
                return func(value, param);
            }

            return Wrapper;
        }

        public static Action<T2> Partial<T1, T2>(this Action<T1, T2> func, T1 value)
        {
            void Wrapper(T2 param)
            {
                func(value, param);
            }

            return Wrapper;
        }

        public static Func<TResult> Partial<T1, TResult>(this Func<T1, TResult> func, T1 value)
        {
            TResult Wrapper()
            {
                return func(value);
            }

            return Wrapper;
        }


        public static Action Partial<T1>(this Action<T1> func, T1 value)
        {
            void Wrapper()
            {
                func(value);
            }

            return Wrapper;
        }
    }
}