using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wing : MonoBehaviour
{
    void Update()
    {
        if (transform.parent.gameObject == null) Destroy(gameObject);
    }
}
