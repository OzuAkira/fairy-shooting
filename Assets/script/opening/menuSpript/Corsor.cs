using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Corsor : MonoBehaviour
{
    public Abs_menuElement[] menus;
    int index = 0 , oldIndex = 0;
    bool up, down;
    AudioSource ads;
    public AudioClip ac;
    private void Start()
    {
        ads = GetComponent<AudioSource>();
        menuUpdate();
        gameObject.SetActive(false);
    }
    private void OnMove(InputValue value)
    {
        var axis = value.Get<Vector2>();
        if(axis.y == 1)up = true;
        else if(axis.y == -1)down = true;
    }
    void OnShot()
    {
        menus[index].select();
    }
    private void Update()
    {
        if (up)index--;
        else if(down)index++;

        if(index != oldIndex)menuUpdate();
    }
    void menuUpdate()
    {
        if(index < 0)index = menus.Length - 1;
        else if(index >= menus.Length)index = 0;

        ads.PlayOneShot(ac);
        oldIndex = index;
        up = false;
        down = false;

        for(int i=0;i<menus.Length;i++)
        {
            if (i == index) menus[i].On();
            else menus[i].Off();
        }
    }
}
