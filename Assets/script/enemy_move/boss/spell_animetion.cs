using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spell_animetion : MonoBehaviour
{
    Vector2 goalPos = new Vector2(113, -113), startPos = new Vector2(-118, -50);//�X�^�[�g�ƃS�[���̍��W

    [SerializeField]float moveSpeed = 0.1f , alphaSpeed = 0.001f;

    RectTransform rectTransform;
    Image image;
    UnityEngine.Color color;

    public bool finish = false;
    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();//UI���W���擾
        image = gameObject.GetComponent<Image>();
        color = image.color;//�F���擾

        color.a = 0;
        image.color = color;//�����ŐF�����f�����

        rectTransform.anchoredPosition = startPos;

        StartCoroutine(start_spell());
    }
    IEnumerator start_spell()
    {
        Vector2 Vector = goalPos - rectTransform.anchoredPosition;

        while (rectTransform.anchoredPosition != goalPos)
        {
            color.a +=alphaSpeed ;//1�b�ŕs�����x100��
            image.color = color;

            rectTransform.anchoredPosition += Vector * moveSpeed;//�ړ�
            if(rectTransform.anchoredPosition.x > goalPos.x)rectTransform.anchoredPosition = goalPos;//�ʂ�߂����狸��
            yield return null;
        }

        yield return new WaitForSeconds(1);



        Vector3 addScale = new Vector3(0.01f,0.01f,0);
        
        while (image.color.a >= 0)//�����ɂȂ�܂�
        {
            color.a -= alphaSpeed*2;//0.5�b�ŕs�����x0��
            image.color = color;

            rectTransform.localScale += addScale;
            yield return null;
            
        }

        finish = true;
    }
}
