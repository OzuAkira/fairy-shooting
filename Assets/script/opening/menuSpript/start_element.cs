using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class start_element : Abs_menuElement
{
    AudioSource ads;
    public AudioClip ac;
    void Start()
    {
        ads = GetComponent<AudioSource>();
    }
    public override void select()
    {
        StartCoroutine("LoadScene");
    }

    IEnumerator LoadScene()
    {
        ads.PlayOneShot(ac);

        yield return new WaitForSeconds(1);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("StageScene");

        // ���[�h���܂��Ȃ玟�̃t���[����
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
