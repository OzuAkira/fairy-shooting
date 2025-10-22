using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class item_catch : MonoBehaviour
{
    player_shot p_shot;

    GameObject score, power, canvas;
    Text score_text, powre_text;
    public int now_score;

    Resurrection res;
    AudioSource ads;
    [SerializeField] AudioClip[] ac;
    [SerializeField]GameObject GM;
    [SerializeField] Text bomText;
    void Start()
    {
        GameObject player = transform.parent.gameObject;
        p_shot = player.GetComponent<player_shot>();

        canvas = GameObject.Find("Canvas_2");
        score = canvas.transform.Find("score").gameObject;
        power = canvas.transform.Find("power").gameObject;

        res = GM.GetComponent<Resurrection>();
        ads = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("item"))
        {
            item catch_item = collision.gameObject.GetComponent<item>();

            ads.PlayOneShot(ac[0]);

            if (catch_item.isPoint)
            {
                score_text = score.GetComponent<Text>();
                now_score = int.Parse(score_text.text);

                now_score += catch_item.have_point;
                score_text.text = now_score.ToString();
            }
            else
            {
                powre_text = power.GetComponent<Text>();
                p_shot.my_power += catch_item.have_power;

                powre_text.text = "Power : " + p_shot.my_power.ToString();

                //score���グ��
                score_text = score.GetComponent<Text>();
                now_score = int.Parse(score_text.text);

                now_score += catch_item.have_point;
                score_text.text = now_score.ToString();
            }
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("bom"))
        {
            res = GM.GetComponent<Resurrection>();//resurection�̍X�V
            ads.PlayOneShot(ac[1]);


            res._bom++;
            res.textChange(" Bom   : ", res._bom, bomText);//UI�̍X�V��resurection�̊֐����g�p


            Destroy(collision.gameObject);
        }
    }
}
