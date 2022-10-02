using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * For systems that require a lot of different objects, specifically particle systems, it's smart
 * to always draw from and push to an object pool rather than use constructors and garbage
 * collection.
 *
 * Every object pool has a poster child reference of the same object type. If drawing from an
 * empty pool, it returns an Object.Instantiate with the poster child as an argument.
 *
 */
 
public class ObjectPool<ObjectType> where ObjectType : MonoBehaviour
{
    public ObjectType PosterChild;

    private Queue<ObjectType> pool;

    public ObjectPool(ObjectType posterChild)
    {
        this.PosterChild = posterChild;
        this.pool = new Queue<ObjectType>();
    }

    public void Push(ObjectType thing)
    {
        pool.Enqueue(thing);
    }
    
    public bool IsEmpty()
    {
        return pool.Count == 0;
    }

    public ObjectType Pop()
    {
        if (pool.Count == 0)
        {
            return Object.Instantiate(PosterChild);
        }
        return pool.Dequeue();
    }
}
