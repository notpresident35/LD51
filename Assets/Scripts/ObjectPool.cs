using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * For systems that require a lot of different objects, specifically particle systems, it's smart
 * to always draw from and push to an object pool rather than use constructors and garbage
 * collection.
 *
 * Drawing from an object pool returns an object in an indeterminate state.
 * Drawing from an empty object pool calls its constructor with no parameters.
 *
 */
 
public class ObjectPool<ObjectType> where ObjectType : class, new()
{
    private static Queue<ObjectType> pool = new Queue<ObjectType>();
    
    public static void Push(ObjectType thing)
    {
        pool.Enqueue(thing);
    }
    
    public static bool IsEmpty()
    {
        return pool.Count == 0;
    }

    public static ObjectType Pop()
    {
        if (pool.Count == 0)
        {
            return new ObjectType();
        }
        return pool.Dequeue();
    }
}
