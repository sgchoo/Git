using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform camFollow;
    
    void Update()
    {
        transform.position = camFollow.position;
    }
}
