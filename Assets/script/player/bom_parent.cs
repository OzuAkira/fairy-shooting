using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bom_parent : MonoBehaviour
{
    public GameObject child_light_1 , child_light_2;
    List<GameObject> lights = new List<GameObject>();
    void Start()
    {
        StartCoroutine(bom_anime());
    }
    IEnumerator bom_anime()
    {
        for (int i = 0; i < 8; i++)
        {
            if(i%2 == 0)lights.Add( Instantiate(child_light_1, transform.position, Quaternion.Euler(0, 0, i * 45)));
            else lights.Add( Instantiate(child_light_2, transform.position, Quaternion.Euler(0, 0, i * 45)));           
        }
        yield return new WaitForSeconds(3);

        foreach (GameObject obj in lights)
        {
            Destroy(obj);
        }
        Destroy(gameObject);
    }
}
