using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    [SerializeField] string sceneName;

    public void SceneChanger()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit(); 
    }
}
/*
TODO LIST:
-SCRIPT CONTROLADOR MUSICA 
*/
