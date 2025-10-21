using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class bom_animation : MonoBehaviour
{
     public float speed = 1.0f;         // 回転の速さ
    public float radiusGrowth = 0.5f;  // 半径の増加速度
    public float heightGrowth = 0f;  // 上方向(Y)への上昇速度（不要なら0に）
    public bool useXZPlane = false;     // XZ平面で回すか（falseならXY平面）

    private float angle = 0f;
    private float radius = 0f;
    float add;
    void Start()
    {
        add = transform.rotation.eulerAngles.z;
        //speed = UnityEngine.Random.Range(0.1f, 1);
    }
    void Update()
    {
        // 回転角度を増加
        angle += speed * Time.deltaTime;

        // 半径を徐々に拡大
        radius += radiusGrowth * Time.deltaTime;

        // 座標計算（内側から外側へ）
        float x = transform.position.x + Mathf.Cos(angle+add) * radius;
        float y = transform.position.y + Mathf.Sin(angle+add) * radius;

        if (useXZPlane)
        {
            transform.position = new Vector3(x, heightGrowth * angle, y);
        }
        else
        {
            transform.position = new Vector3(x, y, 0);
        }
    }
}
