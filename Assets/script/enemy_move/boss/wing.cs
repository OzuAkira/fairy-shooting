using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wing : MonoBehaviour
{
    GameObject parentObj;
    // Start is called before the first frame update
    void Start()
    {
        parentObj = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (parentObj == null) Destroy(gameObject);
    }
}
