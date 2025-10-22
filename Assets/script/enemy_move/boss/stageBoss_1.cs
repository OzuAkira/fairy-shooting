using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class stageBoss_1 : MonoBehaviour

//ボスは全ての処理をこのスクリプトで実行する
//ただし、UIは除く
{
    [SerializeField] float start_y_Pos = 3 , moveSpeed;
    [SerializeField] GameObject HP_slider , spell_bg;

    [SerializeField] GameObject[] phase_1_bullet;


    GameObject playerObj , bom , canvas , score , resObj ,bg;

    Text _text;
    public int hit_point = 1, kill_point = 10, now_score;

    Rigidbody2D rb;
    BoxCollider2D bc;
    Slider slider;
    public float HP = 200;

    bool isSpell = false , isPhase_1 = false , isPhase_2 = false;

    System.Random rnd;

    AudioSource audio;
    public AudioClip audioClip;

    void Start()
    {
        audio = GetComponent<AudioSource>();

        bc = GetComponent<BoxCollider2D>();
        bc.enabled = false;//最初は無敵

        rb = GetComponent<Rigidbody2D>();

        rnd = new System.Random();      // Randomオブジェクトを作成

        playerObj = GameObject.Find("player");
        
        player_shot player_shot = playerObj.GetComponent<player_shot>();


        if (player_shot.my_power > player_shot.power_level * 2) HP *= 3;
        else if (player_shot.my_power > player_shot.power_level ) HP *= 2;

        bom = Instantiate(bom_obj);
        bom.SetActive(false);

        //score関係
        canvas = GameObject.Find("Canvas_2");
        score = canvas.transform.Find("score").gameObject;

        

        StartCoroutine(buttleStart());//登場＆無敵解除
    }

    bool stopd = false;

    
    private void Update()
    {
        if(slider == null)return;
        if(slider.value <= (slider.maxValue / 10) * 3)//現HPが残り3割になったら
        {
            if (stopd == false　&& isPhase_1)
            { 
                StopAllCoroutines();
                stopd = true;
                StartCoroutine(spell_1());
            }
            else if (stopd == false && isPhase_2)
            {
                StopAllCoroutines();
                stopd = true;
                StartCoroutine(spell_2());
            }

        }
        if (audio.isPlaying == false)
        {
            audio.clip = audioClip;
            audio.Play();
            audio.loop = true;
        }
    }

    //被弾処理

    //ここの数値は要検討
    float spell_damage = 0.5f;
    public List<GameObject> nextHP = new List<GameObject>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player_bullet"))
        {
            
            Destroy(collision.gameObject);
            //score の増か
            _text = score.GetComponent<Text>();
            now_score = int.Parse(_text.text);

            now_score += hit_point;
            _text.text = now_score.ToString();

            if (isSpell) HP -= spell_damage;
            else HP--;//ここは変えない



            if (isPhase_1) slider.value = HP - slider.maxValue;
            else if (isPhase_2) slider.value = HP;
            if (slider.value <= 0 && isPhase_1)
            {
                bom.SetActive(true);//全敵弾を消す
                Destroy(bg);
                StopAllCoroutines();//コルーチンを全て停止

                isPhase_1 = false;//連続呼びを防止
                
                slider.value = slider.maxValue;

                stopd = false;
                isSpell = false;

                Image HP_0_image = nextHP[0].GetComponent<Image>();
                UnityEngine.Color c = HP_0_image.color;
                c = new Color(0.1509f, 0.1309f, 0.1217f);//色を指定
                HP_0_image.color = c;//控えのHPバーを1つ黒にする

                bc.enabled = false;//当たり判定を消した

                StartCoroutine(phase_2());
            }
            //ここにボスが撃破された時の処理を書く
            else if(slider.value <= 0 && isPhase_2)
            {
                bom.SetActive(true);//全敵弾を消す
                Destroy (bg);
                player_bom pb = playerObj.GetComponent<player_bom>();
                pb.Invincible = true;

                StopAllCoroutines();//コルーチンを全て停止

                isPhase_2 = false;//連続呼びを防止
                //stopd = false;
                resObj = GameObject.Find("GameMaster");
                StartCoroutine(resObj.GetComponent<Resurrection>().end(true));

                Destroy(_hp);
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                
            }
        }
    }

    GameObject _hp;
    IEnumerator buttleStart()
    {
        //下に降りてくるやつ
        while (gameObject.transform.position.y > start_y_Pos)
        {
            Vector2 pos = gameObject.transform.position - new Vector3(0,moveSpeed,0);
            rb.MovePosition(pos);

            yield return null;
        }

        yield return new WaitForSeconds(2);


        _hp = Instantiate(HP_slider);//HPバーを表示

        int nextHP_count = 2;
        for (int i = 1; i <= nextHP_count; i++)//次のHPバーのオブジェクトを取得
        {
            nextHP.Add(_hp.transform.GetChild(i).gameObject);
        }
        //↓あとで消す
        //nextHP[0].SetActive(false);

        slider = _hp.transform.Find("Slider").GetComponent<Slider>();//sliderを取得
        slider.maxValue = HP/2;
        slider.value = slider.maxValue;//HPを初期化

        bc.enabled = true;//当たり判定をture


        isPhase_1 = true;
        StartCoroutine(phase_1());
    }

    int frame_1 = 0 , shotFrame = 158;

    IEnumerator phase_1()
    {
        float xPos , i_1=0 , add_i =0.01f ;

        
        StartCoroutine(circleShot());

        yield return new WaitForSeconds(2);

        StartCoroutine(eimShot_1());

        while (true)
        {
            xPos = math.sin(i_1);
            i_1 += add_i;

            Vector2 pos = new Vector3(xPos*2, gameObject.transform.position.y, 0);
            rb.MovePosition (pos);

            frame_1++;//移動後に撃つ処理
            if (frame_1 == shotFrame)yield return StartCoroutine(circleShot());
            if(frame_1 == shotFrame*2) StartCoroutine(circleShot());
            if(frame_1 == shotFrame * 3)yield return StartCoroutine(circleShot());
            if (frame_1 == shotFrame * 4)
            {
                StartCoroutine(circleShot());
                frame_1 = 0;//元に戻す（x=0）
            }

            yield return null;  
        }
    }
    public List<Vector2> Raw_eimPos;
    IEnumerator eimShot_1()
    {
        while (true)
        {
            List<Vector2> clonePos = new List<Vector2>(Raw_eimPos);//コピー

            int rand_n = rnd.Next(clonePos.Count/2, clonePos.Count);
            for (int i = 0; i < rand_n; i++)
            {
                int rand_index = rnd.Next(0, clonePos.Count);//インデックスをランダムに生成
                Instantiate(phase_1_bullet[1], clonePos[rand_index], Quaternion.identity);//ランダムな座標に自機狙い弾を生成
                clonePos.RemoveAt(rand_index);//使用したインデックスを削除
                yield return new WaitForSeconds(1);
            }

            yield return new WaitForSeconds(2);
        }
    }
    
    IEnumerator circleShot()
    {

        int circleNum = 45;
        float addAngle = 4;

        float ms_1 = 0.05f, ms_2 = 0.1f;

        float _ = rnd.Next(0, 2);//第一引数以上、第二引数未満

        if (_ == 0)
        {
            for (int i = 0; i < circleNum; i++)
            {
                Straight_move straight_Move = Instantiate(phase_1_bullet[0], gameObject.transform.position, Quaternion.Euler(0, 0, i * 8))
                    .GetComponent<Straight_move>();
                straight_Move.moveSpeed = ms_1;
            }
            yield return new WaitForSeconds(0.25f);

            for (int i = 0; i < circleNum; i++)
            {
                Straight_move straight_Move = Instantiate(phase_1_bullet[0], gameObject.transform.position, Quaternion.Euler(0, 0, i * 8 + addAngle))
                    .GetComponent<Straight_move>();
                straight_Move.moveSpeed = ms_2;
            }
            yield return new WaitForSeconds(3);
        }
        else
        {
            for (int i = 0; i < circleNum; i++)
            {
                Straight_move straight_Move = Instantiate(phase_1_bullet[0], gameObject.transform.position, Quaternion.Euler(0, 0, i * 8 + addAngle))
                    .GetComponent<Straight_move>();
                straight_Move.moveSpeed = ms_1;
            }
            yield return new WaitForSeconds(0.25f);

            for (int i = 0; i < circleNum; i++)
            {
                Straight_move straight_Move = Instantiate(phase_1_bullet[0], gameObject.transform.position, Quaternion.Euler(0, 0, i * 8))
                    .GetComponent<Straight_move>();
                straight_Move.moveSpeed = ms_2;
            }
            yield return new WaitForSeconds(3);
        }
    }

    Vector3 spell_myPos = new Vector3(0, 3, 0);
    float spell_1_moveSpeed = 0.01f;

    public GameObject eim_bullet ,eim_bullet_2, wave_bullet;

    IEnumerator spell_1()
    {
        yield return StartCoroutine(spell_Anime(1));//メソッドの名前に（spell_1）の1にあわせる
        //本来は0の方が好ましい
        
        //ここからスペルカードの弾幕を書く
        while (true)
        {
            Instantiate(wave_bullet, transform.position, quaternion.identity);
            Instantiate(wave_bullet, transform.position, quaternion.identity).GetComponent<waveBullet>().isMinus = true;

            Instantiate(eim_bullet, transform.position, quaternion.identity);
            Instantiate(eim_bullet, transform.position, quaternion.identity).GetComponent<big_eim>().minus = true;

            yield return new WaitForSeconds(4);

            Instantiate(eim_bullet_2, transform.position, quaternion.identity);
            Instantiate(eim_bullet_2, transform.position, quaternion.identity).GetComponent<big_eim>().minus = true;
            yield return new WaitForSeconds(6);
        }

    }
    public GameObject[] enemy_image;
    public GameObject bom_obj ;
    IEnumerator spell_Anime(int image_num)
    {
        bc.enabled = false;//当たり判定を消す
        isSpell = true;

        bg = Instantiate(spell_bg, new Vector3(0, 0, 0), Quaternion.identity);
        
        yield return null;
        bom.SetActive(true);

        Vector3 velocity = spell_myPos - gameObject.transform.position;//ベクトルを計算
        bool right = false;
        if (velocity.x > 0) right = true;//進行方向が右（X方向のベクトルが正の値）の場合はtrue

        GameObject Animetion_obj = Instantiate(enemy_image[image_num]);//UIアニメーションを再生（スペルカード名）

        //定位置まで移動
        while (gameObject.transform.position != spell_myPos)
        {


            Vector3 movePos = gameObject.transform.position + velocity * spell_1_moveSpeed;

            if (right && movePos.x > 0) movePos = spell_myPos;//矯正
            if (right == false && movePos.x < 0) movePos = spell_myPos;//矯正

            rb.MovePosition(movePos);

            yield return null;
        }

        spell_animetion sa = Animetion_obj.transform.Find("enemy_image").gameObject.GetComponent<spell_animetion>();
        //UIアニメーションが再生されたら脱出
        while (sa.finish == false)
        {
            sa = Animetion_obj.transform.GetChild(0).gameObject.GetComponent<spell_animetion>();
            yield return null;
        }

        Destroy(Animetion_obj);
        bom.SetActive(false);

        bc.enabled = true;//当たり判定を復活

    }
    public GameObject[] phase2_bullets;
    IEnumerator phase_2()
    {
        yield return new WaitForSeconds(3);

        bc.enabled = true;//当たり判定を回復

        bom.SetActive(false);
        isPhase_2 = true;

        float add_i=0.0001f;
        float sum_vect = 0;
        Vector3 goale_pos = new Vector3(-4, -1.5f, 0);
        //Vector3 goale_vect = transform.position - goale_pos;
        while (transform.position != new Vector3(-4, 1.5f, 0))
        {
            
            rb.MovePosition(gameObject.transform.position + goale_pos * sum_vect);

            if (transform.position.x < -4) gameObject.transform.position = new Vector3(-4 , 1.5f,0);
            else sum_vect += add_i;
            yield return null;

        }

        goale_pos = new Vector3(4, 0.75f, 0);
        sum_vect = 0;

        add_i = 0.0001f;
        int i = 0;
        yield return new WaitForSeconds(1);

        System.Random random = new System.Random();
        int loop_count = 3;

        while (transform.position != new Vector3(4, 3f, 0))
        {

            rb.MovePosition(gameObject.transform.position + goale_pos * sum_vect);

            if (transform.position.x > 4) gameObject.transform.position = new Vector3(4, 3, 0);
            else sum_vect += add_i;

            int frame = 10;

            i++;
            if (i % frame == 0)
            {
                for (int j = 0; j < loop_count; j++)
                {
                    int ran_int = random.Next(-25, 25);
                    int ran_speed = random.Next(3, 10);

                    GameObject g_bullet = Instantiate(phase2_bullets[1], transform.position, Quaternion.Euler(0, 0, (i / frame) + ran_int));
                    g_bullet.GetComponent<Straight_move>().moveSpeed = ran_speed * 0.01f;
                    SpriteRenderer sr = g_bullet.GetComponent<SpriteRenderer>();
                    switch (UnityEngine.Random.Range(0, 7))
                    {
                        case 0:
                            sr.color = Color.white
                                ; break;
                        case 1:
                            sr.color = Color.red
                                ; break;
                        case 2:
                            //sr.color = Color.blue
                                ; break;
                        case 3:
                            sr.color = Color.green
                                ; break;
                        case 4:
                            sr.color = Color.cyan
                                ; break;
                        case 5:
                            sr.color = Color.magenta
                                ; break;
                        case 6:
                            sr.color = Color.yellow
                                ; break;
                    }//色を変える
                }
            }
            yield return null;

        }
        yield return new WaitForSeconds(1);

        goale_pos = new Vector3(-1, 0, 0);
        sum_vect = 0;

        add_i = 0.0001f;

        bool shoted = false;
        while (transform.position != new Vector3(0, 3f, 0))
        {

            rb.MovePosition(gameObject.transform.position + goale_pos * sum_vect);

            if (transform.position.x <= 2 && shoted == false)
            {
                shoted = true;
                Instantiate(phase2_bullets[2], gameObject.transform.position, Quaternion.identity);//自機狙いの拡散弾
                                                                                                   //紅魔郷EXの中ボス倒した直後に妖精が撃ってくるアレ を一発
            }
            if (transform.position.x < 0) gameObject.transform.position = new Vector3(0, 3, 0);
            else sum_vect += add_i;

            yield return null;

        }
        int gravity_int = 50;

        for(int ii = 0; ii < gravity_int; ii++)
        {
           SpriteRenderer sr = Instantiate(phase2_bullets[3] , transform.position ,Quaternion.identity).GetComponent<SpriteRenderer>();
            switch(UnityEngine.Random.Range(0, 7))
            {
                case 0:
                    sr.color = Color.white
                        ;break;
                case 1:
                    sr.color = Color.red
                        ; break;
                case 2:
                    //sr.color = Color.blue
                        ; break;
                case 3:
                    sr.color = Color.green
                        ; break;
                case 4:
                    sr.color = Color.cyan
                        ; break;
                case 5:
                    sr.color = Color.magenta
                        ; break;
                case 6:
                    sr.color = Color.yellow
                        ; break;
            }//色を変える

            yield return null; yield return null;//2frame待つ
        }
        StartCoroutine(phase_2());//繰り返し
    }
    [SerializeField] Vector2 wall_bullet_Pos1, wall_bullet_Pos2;
    IEnumerator spell_2()
    {
        yield return StartCoroutine(spell_Anime(2));
        StartCoroutine(wallShot());

        float i = 0, add_i = 0.01f;
        int shotCount = 0 , interval = 12;
        while (true)
        {
            if (shotCount <= interval)
            {
                float angle = math.sin(i);
                i += add_i;
                Instantiate(phase2_bullets[5], transform.position, Quaternion.Euler(0, 0, angle));

                for (int t = 0; t < 10; t++)//10フレーム待機
                    yield return null;
                shotCount++;
            }
            else
            {
                shotCount = 0;
                yield return new WaitForSeconds(0.5f);
            }
        }
        
    }
    IEnumerator wallShot()
    {
        int wall_n = 4;
        while (true) 
        {
            for (int i = 0; i < wall_n; i++)
            {
                Instantiate(phase2_bullets[4], wall_bullet_Pos1, Quaternion.identity);
                Instantiate(phase2_bullets[4], wall_bullet_Pos2, Quaternion.Euler(0, 0, 180));
                //マイナス座標に出現させる
                Instantiate(phase2_bullets[4], -wall_bullet_Pos2, Quaternion.identity);
                Instantiate(phase2_bullets[4], -wall_bullet_Pos1, Quaternion.Euler(0, 0, 180));

            }
            yield return new WaitForSeconds(1f);
        }
    }
    




}

