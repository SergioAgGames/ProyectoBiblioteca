using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public UnityEvent<string> OnSceneCompleted = new UnityEvent<string>();

  
    private Dictionary<string, bool> completedScenes = new Dictionary<string, bool>();

    [SerializeField] private List<string> registeredSceneNames = new List<string>();

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeScenes();
    }

    private void InitializeScenes()
    {
        foreach (string sceneName in registeredSceneNames)
        {
            if (!completedScenes.ContainsKey(sceneName))
            {
                completedScenes[sceneName] = false;
            }
        }
    }

    public void CompleteScene(string sceneName)
    {
        if (!completedScenes.ContainsKey(sceneName))
        {

            completedScenes[sceneName] = false;
            Debug.LogWarning($"[GameController] La escena '{sceneName}' no estaba registrada. Se agregó automáticamente.");
        }

        if (completedScenes[sceneName])
        {
            Debug.Log($"[GameController] La escena '{sceneName}' ya estaba completada.");
            return;
        }

        completedScenes[sceneName] = true;
        Debug.Log($"[GameController] Escena completada: {sceneName}");

     
        OnSceneCompleted.Invoke(sceneName);

      
        CheckAllScenesCompleted();
    }
    public bool IsSceneCompleted(string sceneName)
    {
        return completedScenes.ContainsKey(sceneName) && completedScenes[sceneName];
    }
    public Dictionary<string, bool> GetAllSceneStates()
    {
        return completedScenes;
    }
    private void CheckAllScenesCompleted()
    {
        foreach (var entry in completedScenes)
        {
            if (!entry.Value) return;
        }

        Debug.Log("[GameController] ¡Todas las escenas completadas! El juego puede finalizar.");

        // UnityEngine.SceneManagement.SceneManager.LoadScene("EscenaFinal");
    }
    public void ResetAllProgress()
    {
        List<string> keys = new List<string>(completedScenes.Keys);
        foreach (string key in keys)
        {
            completedScenes[key] = false;
        }
        Debug.Log("[GameController] Progreso reiniciado.");
    }
}
