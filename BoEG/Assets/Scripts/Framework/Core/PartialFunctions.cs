using System;

namespace MobaGame.Framework.Core
{
    public static class PartialFunctions
    {
        // Four Args

        public static Func<T2, T3, T4, TResult> Partial<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func, T1 value)
        {
            TResult Wrapper(T2 a, T3 b, T4 c)
            {
                return func(value, a, b, c);
            }

            return Wrapper;
        }

        public static Action<T3, T4> Partial<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> func, T1 a, T2 b)
        {
            void Wrapper(T3 c, T4 d)
            {
                func(a, b, c, d);
            }

            return Wrapper;
        }

        public static Func<T3, T4, TResult> Partial<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func, T1 a, T2 b)
        {
            TResult Wrapper(T3 c, T4 d)
            {
                return func(a, b, c, d);
            }

            return Wrapper;
        }

        public static Action<T2, T3, T4> Partial<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> func, T1 value)
        {
            void Wrapper(T2 a, T3 b, T4 c)
            {
                func(value, a, b, c);
            }

            return Wrapper;
        }

        public static Func<T4, TResult> Partial<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func, T1 a, T2 b, T3 c)
        {
            TResult Wrapper(T4 param)
            {
                return func(a, b,c, param);
            }

            return Wrapper;
        }

        public static Action<T4> Partial<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> func, T1 a, T2 b, T3 c)
        {
            void Wrapper(T4 param)
            {
                func(a, b, c, param);
            }

            return Wrapper;
        }

        public static Func<TResult> Partial<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func, T1 a, T2 b, T3 c, T4 d)
        {
            TResult Wrapper()
            {
                return func(a, b, c,d);
            }

            return Wrapper;
        }

        public static Action Partial<T1, T2, T3,T4>(this Action<T1, T2, T3, T4> func, T1 a, T2 b, T3 c, T4 d)
        {
            void Wrapper()
            {
                func(a, b, c,d);
            }

            return Wrapper;
        }


        // Three Args
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

        public static Func<T3,TResult> Partial<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, T1 a, T2 b)
        {
            TResult Wrapper(T3 param)
            {
                return func(a, b, param);
            }

            return Wrapper;
        }

        public static Action<T3> Partial<T1, T2, T3>(this Action<T1, T2, T3> func, T1 a, T2 b)
        {
            void Wrapper(T3 param)
            {
                func(a, b, param);
            }

            return Wrapper;
        }

        public static Func<TResult> Partial<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, T1 a, T2 b, T3 c)
        {
            TResult Wrapper()
            {
                return func(a, b, c);
            }

            return Wrapper;
        }

        public static Action Partial<T1, T2, T3>(this Action<T1, T2, T3> func, T1 a, T2 b, T3 c)
        {
            void Wrapper()
            {
                func(a, b, c);
            }

            return Wrapper;
        }

        // Two Args
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

        public static Func<TResult> Partial<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 a, T2 b)
        {
            TResult Wrapper()
            {
                return func(a, b);
            }

            return Wrapper;
        }

        public static Action Partial<T1, T2>(this Action<T1, T2> func, T1 a, T2 b)
        {
            void Wrapper()
            {
                func(a, b);
            }

            return Wrapper;
        }

        // One Arg
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