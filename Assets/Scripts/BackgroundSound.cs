
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundSound : MonoBehaviour
{
    public static BackgroundSound Instance { get; private set; }

    [System.Serializable]
    public struct SceneLayer
    {
        public string sceneName;   // Nombre exacto de la escena
        public AudioClip clip;     // La pista de audio
    }

    [Header("Capas de Audio por Escena")]
    public SceneLayer[] sceneLayers;

    private AudioSource[] audioSources;

    void Awake()
    {
        // Singleton persistente
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeAudioSources();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void InitializeAudioSources()
    {
        audioSources = new AudioSource[sceneLayers.Length];

        for (int i = 0; i < sceneLayers.Length; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].clip = sceneLayers[i].clip;
            audioSources[i].loop = true;
            audioSources[i].playOnAwake = false;
            audioSources[i].mute = true; // Todas muteadas al inicio
        }

        // Arrancar TODAS juntas en el mismo frame
        foreach (var source in audioSources)
            source.Play();

        // Desmutear solo la primera (escena inicial)
        UpdateLayers(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateLayers(scene.name);
    }

    void UpdateLayers(string sceneName)
    {
        for (int i = 0; i < sceneLayers.Length; i++)
        {
            // Desmutea acumulativamente: todas las anteriores + la actual
            bool shouldPlay = false;
            for (int j = 0; j <= i; j++)
            {
                if (sceneLayers[j].sceneName == sceneName)
                {
                    shouldPlay = true;
                    break;
                }
                // Si la escena actual vino después de la capa j, desmutear
                if (IsSceneAfterOrCurrent(sceneName, j))
                    shouldPlay = true;
            }

            audioSources[i].mute = !shouldPlay;
        }
    }

    // Devuelve true si la escena actual es igual o posterior a la capa [layerIndex]
    bool IsSceneAfterOrCurrent(string currentScene, int layerIndex)
    {
        // El orden de las capas define el orden de las escenas
        for (int i = layerIndex; i < sceneLayers.Length; i++)
        {
            if (sceneLayers[i].sceneName == currentScene)
                return true;
        }
        return false;
    }

    // Llamar esto para ir a una nueva escena de forma segura
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
