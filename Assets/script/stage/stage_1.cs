using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class stage_1 : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image image;

    public GameObject enemy_1, enemy_2, enemy_3, enemy_4 , enemy_5 , enemy_6 , enemy_7 ,boss;
    public Vector2 enemy1_2_pos, enemy3_pos, enemy4_pos , enemy7_pos , boss_Pos;

    int count = 10;

    int next_count = 8;
    [SerializeField]float wait_s = 1f;
    [SerializeField] GameObject bomBody;
    List<GameObject> keyEnemies = new List<GameObject>();//倒さないと先に進めない

    public AudioClip clip;
    AudioSource audio_s;

    void Start()
    {
        audio_s = GetComponent<AudioSource>();
        
        StartCoroutine(game());
    }
    void Update()
    {
        if (audio_s.isPlaying == false)
        {
            audio_s.Stop();
            audio_s.clip = clip;
            audio_s.Play();
            audio_s.loop = true;
        }
        
    }
    IEnumerator game()
    {
        yield return new WaitForSeconds(1);
        
        yield return gameStart();
        yield return new WaitForSeconds(2); 
        yield return StartCoroutine(phase_1());


    }
    IEnumerator gameStart()
    {
        UnityEngine.Color c = image.color;
        bool loop = false, isMax = false;
        float i = 0 , add_i = 0.01f;

        //フェードイン＆フェードアウト
        while (loop == false)
        {
            //Debug.Log("i= " + i);
            if (i < 1 && isMax == false) i += add_i;//Maxまで0.01づつ上げる

            else if (isMax == false)//Maxまで行ったら、１秒待ってフラグを立てる
            {
                yield return new WaitForSeconds(1);
                isMax = true;
            }

            if (isMax)i -= add_i;//Minまで0.0１づつ減らす

            c.a = i;
            image.color = c;
            yield return null;

            if (i <= 0 && isMax) loop = true;//iが0になったらWhileから脱出
        }
    }

    IEnumerator phase_1()
    {
        //右端に弾消しObｊを配置
        GameObject bom = Instantiate(bomBody, new Vector3(8, 0), Quaternion.identity);

        //左端からCount匹の敵が出現
        for (int i = 0; i < count; i++)
        {
            Instantiate(enemy_1, enemy1_2_pos, Quaternion.identity);
            yield return new WaitForSeconds(wait_s);

            if (i == next_count) StartCoroutine(phase_2());//  非同期で実行

        }
        Destroy(bom);
    }
    IEnumerator phase_2()
    {
        GameObject bom = Instantiate(bomBody, new Vector3(-8, 0), Quaternion.identity);
        //右端からCount匹の敵が出現
        for (int i = 0; i < count; i++)
        {
            Instantiate(enemy_2, new Vector2(enemy1_2_pos.x * -1, enemy1_2_pos.y), Quaternion.identity);

            yield return new WaitForSeconds(wait_s);

            if (i == count - 1)//ちょっと硬い２体の敵が出現
            {
                GameObject wallShot_enemy_1 = Instantiate(enemy_3, enemy3_pos, Quaternion.identity);
                wallShot_enemy_1.GetComponent<enemy_3>().interval_flag = true;//interval_flagをTrueにする

                keyEnemies.Add(wallShot_enemy_1);//こいつは倒さないとボスが出現しない

                GameObject wallShot_enemy_2 = Instantiate(enemy_3, new Vector2(enemy3_pos.x * -1, enemy3_pos.y), Quaternion.identity);//x座標に-1を乗算して出現させる

                keyEnemies.Add (wallShot_enemy_2);//こいつは倒さないとボスが出現しない
            }
        }
        Destroy(bom);
        yield return new WaitForSeconds(1);
        StartCoroutine(phase_3());

    }
    IEnumerator phase_3()
    {
        GameObject middle_enemy1 = Instantiate(enemy_4, enemy4_pos, Quaternion.identity);//テンション高いやつ

        yield return new WaitForSeconds(2);

        GameObject bom = Instantiate(bomBody, new Vector3(-8, 0), Quaternion.identity);
        while (middle_enemy1 != null)
        {
            Instantiate(enemy_5, new Vector2(enemy1_2_pos.x * -1, enemy1_2_pos.y), Quaternion.identity);//右端からCount匹の敵が出現
            yield return new WaitForSeconds(4);
        }
        Destroy (bom);

        GameObject middle_enemy2 = Instantiate(enemy_4, new Vector2( -enemy4_pos.x , enemy4_pos.y), Quaternion.identity);//テンション高いやつ2号。反対側から出現

        GameObject circlShot_enemy = Instantiate(enemy_7 , enemy7_pos , Quaternion.identity);//360度撃つ奴
        enemy_death death = circlShot_enemy.GetComponent<enemy_death>();

        keyEnemies.Add(circlShot_enemy);//こいつは倒さないとボスが出現しない

        yield return new WaitForSeconds(wait_s);

        bom = Instantiate(bomBody, new Vector3(8, 0), Quaternion.identity);
        while (middle_enemy2 != null)
        {
            death.hpStoper = true;
            Instantiate(enemy_6, enemy1_2_pos, Quaternion.identity);//左端からCount匹の敵が出現
            yield return new WaitForSeconds(4);
        }
        Destroy(bom);

        death.hpStoper = false;
        yield return new WaitForSeconds(wait_s);

        while (true)
        {
            if (keyEnemies.All(enemy => enemy == null))//keyEnemies内全てのオブジェクトがnullだったら
                break;

            yield return null;
        }
        yield return new WaitForSeconds(3);
        while(audio_s.volume != 0)
        {
            audio_s.volume -= 0.001f;
            yield return null;
        }
        audio_s.Stop();
        Instantiate(boss, boss_Pos, Quaternion.identity);

    }
    



}
