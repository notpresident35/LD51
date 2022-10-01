using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * For systems that require a lot of different objects, specifically particle systems, it's smart
 * to always draw from and push to an object pool rather than use constructors and garbage
 * collection.
 *
 * Drawing from an empty object pool is the same as creating the new object.
 *
 */
 
public class ObjectPool<ObjectType> : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
