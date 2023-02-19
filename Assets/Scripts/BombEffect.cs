using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffect : MonoBehaviour
{
    void Update()
    {
        StartCoroutine(DestoryEffect());       
    }

    IEnumerator DestoryEffect()
    {
        yield return new WaitForSeconds(1.6f);

        Destroy(gameObject);
    }
}
