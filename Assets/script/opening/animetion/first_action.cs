using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class first_action : MonoBehaviour
{
    public Vector2 golePos = new Vector2(-130, 160) , menuPos = new Vector2(250,-80);
    public float titleMoveSpeed = 0.002f, menuMoveSpeed = 0.6f;
    public GameObject menuFolder , anykye_Obj , corsorObj;
    bool isPush , isCoroutine;
    RectTransform myRT,menuRT;
    int i = 0;
    AudioSource ads;
    public AudioClip ac;
    private void Start()
    {
        myRT = gameObject.GetComponent<RectTransform>();
        menuRT = menuFolder.GetComponent<RectTransform>();
        ads = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (Input.anyKey)isPush = true;
        if (isPush)
        {
            anykye_Obj.SetActive(false);
            
            if (isCoroutine == false) StartCoroutine(opening());
        }
        else
        {
            i++;
            if (i >= 90) anykye_Obj.SetActive(false);//1.5�b�o�������\��
            if (i >= 120)//0.5�b��ɕ\�����Ai�����Z�b�g
            {
                anykye_Obj.SetActive(true);
                i = 0;
            }
        }

    }
    
    IEnumerator opening()
    {
        bool loop = false;
        isCoroutine = true;
        ads.PlayOneShot(ac);

        while (loop == false)
        {
            myRT.anchoredPosition += golePos * titleMoveSpeed;//title�I�u�W�F�N�g�͌��_����n�܂邩��AgolePos�����̂܂܃x�N�g���ɂȂ�

            menuRT.anchoredPosition -= new Vector2(menuMoveSpeed , 0);//X���݈̂ړ��B����؂��̂ŁA����IF���ŋ�������K�v������

            if (myRT.anchoredPosition.y > golePos.y)
            {
                myRT.anchoredPosition = golePos;
                loop = true;
                corsorObj.SetActive(true);
            }
            yield return null;
        }
    }
}
