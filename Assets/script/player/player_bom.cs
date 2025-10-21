using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class player_bom : MonoBehaviour
{
    public GameObject bom , GM , bomUI;
    SpriteRenderer sr;
    Resurrection res;
    [SerializeField] Text bomText;
    bool _do = false;

    public bool Invincible = false;
    private void Start()
    {
        res = GM.GetComponent<Resurrection>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }
    void OnBom()
    {
        if (_do == false && res._bom > 0) StartCoroutine(bom_animetion());
    }
    IEnumerator bom_animetion()
    {
        _do = true;//�A�˖h�~�p

        res._bom--;
        res.textChange(" Bom   : ", res._bom, bomText);//UI�̍X�V��resurection�̊֐����g�p

        res = GM.GetComponent<Resurrection>();//resurection�̍X�V

        bom.SetActive(true);
        Invincible = true;

        Instantiate(bomUI, gameObject.transform.position,Quaternion.identity);
        yield return new WaitForSeconds(3);//�{����������

        bom.SetActive(false);
        Invincible = false;

        _do = false;
    }
    private void Update()
    {
        if(Invincible) sr.color = Color.red;
        else sr.color = Color.white;
    }

}
