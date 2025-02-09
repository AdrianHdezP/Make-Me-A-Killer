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
        while (t < 0.2f)
        {
            Color color = image.color;
            color.a = t / 0.2f;
            image.color = color;

            t += Time.deltaTime;
            yield return null;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

        coroutine = null;
    }

    IEnumerator EnterScene()
    {
        float t = 0;
        while (t < 1.5f)
        {
            Color color = image.color;
            color.a = 1 - t / 1.5f;
            image.color = color;

            t += Time.deltaTime;

            yield return null;
        }

        coroutine = null;
    }
}
