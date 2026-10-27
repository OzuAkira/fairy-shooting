using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Mathematics;

public class childEim : MonoBehaviour
{
    public bool finish = false;
    public GameObject player;
    big_eim big_Eim;
    [SerializeField] float addRotation , moveSpeed = 0.01f;

    public IEnumerator wait()
    {
        float socond = 1.5f;
        yield return new WaitForSeconds(socond);
    }

    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        player = GameObject.Find("player");

        StartCoroutine(move());
    }
    IEnumerator move()
    {

        yield return StartCoroutine(wait());


        Vector3 vect = transform.position - player.transform.position;
        float angle = Mathf.Atan2(vect.y, vect.x) * Mathf.Rad2Deg;//�x�N�g�������Ƃ�Rotation�ɓ���鐔�l���v�Z

        transform.rotation = Quaternion.Euler(0, 0, angle + addRotation);

        float count_i = 0 ,add_i = 0.01f, moveSpeed_2=0;
        while (true)
        {
            if (math.sin(count_i) < 0)
            {
                moveSpeed_2 = math.sin(count_i) * 0.05f;
                count_i += add_i;
            }
            else moveSpeed_2 = 0.05f;


            if (finish)
            {
                Vector3 movepos;
                if (player.activeSelf == false)
                {
                    movepos = Vector3.down * moveSpeed;
                }
                movepos = transform.position + transform.rotation * (Vector3.down * moveSpeed);//�e�̌����ɒ��i
                rb.MovePosition(movepos);
            }
            yield return null;
        }
    }
}
