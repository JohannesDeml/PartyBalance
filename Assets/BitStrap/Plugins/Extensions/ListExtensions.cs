﻿using System.Collections.Generic;
using System.Text;

namespace BitStrap
{
    /// <summary>
    /// Bunch of utility extension methods to the generic List class.
    /// These methods are intended to be System.Ling substitutes as they do not generate garbage.
    /// </summary>
    public static class ListExtensions
    {
        public struct GCFreeEnumerator<T>
        {
            private List<T>.Enumerator enumerator;

            public T Current
            {
                get { return enumerator.Current; }
            }

            public GCFreeEnumerator( List<T> collection )
            {
                enumerator = collection.GetEnumerator();
            }

            public GCFreeEnumerator<T> GetEnumerator()
            {
                return this;
            }

            public bool MoveNext()
            {
                return enumerator.MoveNext();
            }
        }

        /// <summary>
        /// Use this method to iterate a List in a foreach loop but with no garbage
        /// </summary>
        /// <example>
        /// foreach( var element in myList.Each() )
        /// {
        ///     // code goes here...
        /// }
        /// </example>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static GCFreeEnumerator<T> Each<T>( this List<T> collection )
        {
            return new GCFreeEnumerator<T>( collection );
        }

        /// <summary>
        /// Behaves like System.Linq.Count however it does not generate garbage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static int Count<T>( this List<T> collection, System.Predicate<T> predicate )
        {
            if( predicate == null )
                return 0;

            int count = 0;
            for( int i = 0; i < collection.Count; i++ )
            {
                if( predicate( collection[i] ) )
                    count++;
            }

            return count;
        }

        /// <summary>
        /// Behaves like System.Linq.All however it does not generate garbage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool All<T>( this List<T> collection, System.Predicate<T> predicate )
        {
            if( predicate == null )
                return false;

            for( int i = 0; i < collection.Count; i++ )
            {
                if( !predicate( collection[i] ) )
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Behaves like System.Linq.Any however it does not generate garbage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool Any<T>( this List<T> collection, System.Predicate<T> predicate )
        {
            if( predicate == null )
                return false;

            for( int i = 0; i < collection.Count; i++ )
            {
                if( predicate( collection[i] ) )
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Behaves like System.Linq.FirstOrDefault however it does not generate garbage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T FirstOrDefault<T>( this List<T> collection )
        {
            return collection.Count > 0 ? collection[0] : default( T );
        }

        /// <summary>
        /// Behaves like System.Linq.FirstOrDefault however it does not generate garbage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T FirstOrDefault<T>( this List<T> collection, System.Predicate<T> predicate )
        {
            for( var enumerator = collection.GetEnumerator(); enumerator.MoveNext(); )
            {
                if( predicate( enumerator.Current ) )
                    return enumerator.Current;
            }

            return default( T );
        }

        /// <summary>
        /// Pretty format a list as "[ e1, e2, e3, ..., en ]".
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static string ToStringFull<T>( this List<T> predicate )
        {
            if( predicate == null )
                return "null";
            if( predicate.Count <= 0 )
                return "[]";

            StringBuilder sb = new StringBuilder();

            sb.Append( "[ " );

            for( int i = 0; i < predicate.Count - 1; i++ )
            {
                sb.Append( predicate[i].ToString() );
                sb.Append( ", " );
            }

            sb.Append( predicate[predicate.Count - 1].ToString() );
            sb.Append( " ]" );

            return sb.ToString();
        }
    }
}
