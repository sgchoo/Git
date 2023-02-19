using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    public GameObject bombEffect;

    void OnCollisionEnter(Collision collision)
    {
        GameObject bomb = Instantiate(bombEffect);
        bomb.transform.position = transform.position;

        Destroy(gameObject);  
    }
}
