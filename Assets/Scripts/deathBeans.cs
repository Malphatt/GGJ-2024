using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathBeans : MonoBehaviour
{
    //Prefab of bean
    public GameObject beanPrefab;
    //How many beans to spawn
    public int numBeans = 300;

    //Called when gameobject is enabled
    void OnEnable()
    {
        //Spawn 300 beans
        for (int i = 0; i < numBeans; i++)
        {
            //Random position
            Vector3 pos = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));

            pos = this.transform.position + pos;
                
            //Random rotation
            Quaternion rot = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            //Spawn bean
            GameObject bean = Instantiate(beanPrefab, pos, rot);
            //Add an explosive large force upwards on the bean
            bean.GetComponent<Rigidbody>().AddExplosionForce(10000, pos, 10, 100);
        }
    }
}
