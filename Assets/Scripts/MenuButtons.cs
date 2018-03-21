using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public Animator fadeOut;
    public float fadeTime;

    IEnumerator WaitForFade()
    {
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(2);
    }

    public void LoadScene(int buildIndex)
    {
        fadeOut.SetTrigger("FadeOut");
        StartCoroutine(WaitForFade());
        //SceneManager.LoadScene(buildIndex);
        //if(Time.timeScale == 0) Time.timeScale = 1;
    }

    public void LoadScene(string nameScene)
    {
        fadeOut.SetTrigger("FadeOut");
        //SceneManager.LoadScene(nameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}