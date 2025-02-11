using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    Image image;

    Coroutine coroutine;

    private void Start()
    {
        image = GameObject.FindGameObjectWithTag("White").GetComponent<Image>();
        StartCoroutine(EnterScene());
    }

    public void ResetScene()
    {
        if (coroutine == null) coroutine = StartCoroutine(TranstionScene("Game"));
    }

    public void LoadScene(string scene)
    {
        if (coroutine == null) coroutine = StartCoroutine(TranstionScene(scene));
    }

    IEnumerator TranstionScene(string sceneName)
    {
        float t = 0;
        Color color = image.color;

        while (t < 0.2f)
        {
            color = image.color;
            color.a = t / 0.2f;
            image.color = color;

            t += Time.deltaTime;
            yield return null;
        }

        color = image.color;
        color.a = 1;
        image.color = color;

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

        coroutine = null;
    }

    IEnumerator EnterScene()
    {
        float t = 0;
        Color color = image.color;

        while (t < 1.5f)
        {
            color = image.color;
            color.a = 1 - t / 1.5f;
            image.color = color;

            t += Time.deltaTime;

            yield return null;
        }

        color = image.color;
        color.a = 0f;
        image.color = color;

        coroutine = null;
    }
}
