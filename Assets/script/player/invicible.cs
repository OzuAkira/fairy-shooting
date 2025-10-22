using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invicible : MonoBehaviour
{
    SpriteRenderer sr;
    player_bom pb;
    private void Start()
    {
        pb = transform.parent.gameObject.GetComponent <player_bom>();
        sr = transform.gameObject.GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if( pb.Invincible)sr.color = Color.blue;
        else sr.color = Color.white;
    }
}
