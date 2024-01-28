using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beanRemover : MonoBehaviour
{
    //Lifespan
    public float lifespan = 10f;
    
    void FixedUpdate()
    {
        //Destroy after lifespan
        Destroy(gameObject, lifespan);
    }
}
