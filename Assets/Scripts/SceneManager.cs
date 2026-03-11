using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    [SerializeField] string sceneName;

    public void SceneChanger()
    {
        SceneManager.LoadScene(sceneName);
    }


}
/*
TODO LIST:
-MUSICA MAS ANIMADA
-RETOCAR FOTOS INFANTIL
-SCRIPT CONTROLADOR DE ESCENAS COMPLETADAS
-SCRIPT CONTROLADOR MUSICA 
*/
