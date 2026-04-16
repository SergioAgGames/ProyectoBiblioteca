using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundSound : MonoBehaviour
{
    public static BackgroundSound Instance { get; private set; }

    [System.Serializable]
    public struct SceneLayer
    {
        public string sceneName;
        public AudioClip clip;
    }

    [Header("Capas de Audio por Escena")]
    public SceneLayer[] sceneLayers;

    private AudioSource[] audioSources;
    private bool[] hasStarted; // ¿Ya arrancó esta pista alguna vez?

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(transform.root.gameObject);

        // Crear AudioSources pero NO reproducir nada todavía
        audioSources = new AudioSource[sceneLayers.Length];
        hasStarted = new bool[sceneLayers.Length];

        for (int i = 0; i < sceneLayers.Length; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].clip = sceneLayers[i].clip;
            audioSources[i].loop = true;
            audioSources[i].playOnAwake = false;
            audioSources[i].volume = 0f;
            hasStarted[i] = false;
        }
    }

    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        for (int i = 0; i < sceneLayers.Length; i++)
        {
            if (sceneLayers[i].sceneName == scene.name)
            {
                ActivateLayer(i);
                break;
            }
        }
    }

    void ActivateLayer(int index)
    {
        // Activar la pista nueva sincronizada con la pista maestra (índice 0)
        if (!hasStarted[index])
        {
            hasStarted[index] = true;

            if (index == 0 || !audioSources[0].isPlaying)
            {
                // Primera pista: arrancar desde 0
                audioSources[index].Play();
            }
            else
            {
                // Arrancar en el mismo punto que la pista maestra
                audioSources[index].timeSamples = audioSources[0].timeSamples;
                audioSources[index].Play();
            }
        }

        // Subir volumen de todas las pistas que ya arrancaron (fade suave)
        for (int i = 0; i <= index; i++)
        {
            if (hasStarted[i])
                StartCoroutine(FadeVolume(audioSources[i], 1f, 1.5f));
        }
    }

    IEnumerator FadeVolume(AudioSource source, float targetVol, float duration)
    {
        float start = source.volume;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(start, targetVol, t / duration);
            yield return null;
        }
        source.volume = targetVol;
    }
}