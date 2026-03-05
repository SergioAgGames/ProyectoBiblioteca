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
