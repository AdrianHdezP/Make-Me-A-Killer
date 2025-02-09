using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagment : MonoBehaviour
{
    public void ResetScene()
    {
        SceneManager.LoadScene("Game");  
    }
}
