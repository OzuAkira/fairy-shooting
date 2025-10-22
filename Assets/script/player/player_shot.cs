using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class player_shot : MonoBehaviour
{
    private PlayerInput _playerInput;// 入力を受け取るPlayerInput
    [SerializeField] private string _slowActionName = "Shot";// アクション名
    private InputAction _shotAction;// アクションのコンポーネント

    bool isShot = false;
    int i;
    [SerializeField] int interval = 5;

    private float instant_Ypos = 0.5f;

    [SerializeField] GameObject player_bullet;

    public int my_power = 0 , power_level = 12;
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        // 攻撃アクションをPlayerInputから取得
        _shotAction = _playerInput.actions[_slowActionName];
    }


    
    
    private void Update()
    {
        //間隔でフラグを管理
        if (isShot)
        {
            if (i >= interval)
            {
                i = 0;
                isShot = false;
            }
            else i++;
        }

        //攻撃処理
        if (_shotAction != null && isShot == false)
        {
            // 攻撃ボタンの押下状態取得
            var isPressed = _shotAction.IsPressed();

            if (isPressed)
            {
                if(my_power > power_level*2)
                {
                    Instantiate(player_bullet, gameObject.transform.position + new Vector3(0, instant_Ypos), Quaternion.Euler(0, 0, 5));
                    Instantiate(player_bullet, gameObject.transform.position + new Vector3(0, instant_Ypos), Quaternion.identity);
                    Instantiate(player_bullet, gameObject.transform.position + new Vector3(0, instant_Ypos), Quaternion.Euler(0, 0, -5));
                }
                
                else if(my_power > power_level)
                {
                    Instantiate(player_bullet, gameObject.transform.position + new Vector3(0, instant_Ypos), Quaternion.Euler(0,0,2));
                    Instantiate(player_bullet, gameObject.transform.position + new Vector3(0, instant_Ypos), Quaternion.Euler(0, 0, -2));
                }

                else Instantiate(player_bullet, gameObject.transform.position + new Vector3(0, instant_Ypos), Quaternion.identity);//弾を生成

                isShot = true;


            }
        }
    }
    

}
