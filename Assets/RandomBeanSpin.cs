using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBeanSpin : MonoBehaviour
{
    Vector3 targetRotation;
    float speed = 0.05f;

    // Update is called once per frame
    void FixedUpdate()
    {
        // slowly rotate to a random rotation after every 0.5 seconds
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), speed);

        if (Time.time % 0.5f < 0.01f)
        {
            targetRotation = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        }
    }
}
