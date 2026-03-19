using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// El minimapa persiste entre escenas y controla su propia lógica.
/// Es un Singleton accesible desde cualquier script con MinimapScript.Instance
/// 
/// SETUP en Unity:
/// 1. Coloca este script en el GameObject del minimapa en la PRIMERA escena.
/// 2. En el Inspector, configura la lista "Zones" con una entrada por cada escena del juego.
/// 3. Cada entrada necesita: el nombre de la escena, el icono normal y el icono completado del menú.
/// </summary>
public class MinimapScript : MonoBehaviour
{
    // ── Singleton ────────────────────────────────────────────────
    public static MinimapScript Instance { get; private set; }

    // ── Inspector ────────────────────────────────────────────────
    [SerializeField] private GameObject minimapIcon;       // El icono del minimapa que abre el menú
    [SerializeField] private GameObject menuPanel;         // El panel/menú que se abre al pulsar el icono

    [System.Serializable]
    public class ZoneEntry
    {
        [Tooltip("Nombre exacto de la escena (igual que en File > Build Settings)")]
        public string sceneName;

        [Tooltip("Icono o botón del menú cuando la zona NO está completada")]
        public GameObject iconNormal;

        [Tooltip("Icono o botón del menú cuando la zona SÍ está completada")]
        public GameObject iconCompleted;
    }

    [SerializeField] private List<ZoneEntry> zones = new List<ZoneEntry>();

    // ── Estado interno ────────────────────────────────────────────
    private HashSet<string> completedScenes = new HashSet<string>();

    // ─────────────────────────────────────────────────────────────
    // UNITY CALLBACKS
    // ─────────────────────────────────────────────────────────────

    private void Awake()
    {
        // Patrón Singleton con persistencia entre escenas
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        RefreshAllIcons();
    }

    private void OnEnable()
    {
        // Cada vez que carga una escena nueva, refrescar los iconos del menú
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshAllIcons();
    }

    // ─────────────────────────────────────────────────────────────
    // API PÚBLICA — llamada desde AnswerController
    // ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Marca la escena ACTIVA como completada y actualiza su icono en el menú.
    /// Llamar desde AnswerController cuando la respuesta es correcta:
    ///     MinimapScript.Instance.CompleteCurrentScene();
    /// </summary>
    public void CompleteCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (completedScenes.Contains(currentScene))
        {
            Debug.Log($"[Minimap] '{currentScene}' ya estaba completada.");
            return;
        }

        completedScenes.Add(currentScene);
        Debug.Log($"[Minimap] Escena completada: {currentScene}");

        UpdateIconForScene(currentScene);
    }

    /// <summary>
    /// Devuelve true si la escena indicada ya fue completada.
    /// </summary>
    public bool IsSceneCompleted(string sceneName)
    {
        return completedScenes.Contains(sceneName);
    }

    // ─────────────────────────────────────────────────────────────
    // LÓGICA DEL MENÚ
    // ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Abre y cierra el menú del minimapa al pulsar el icono.
    /// </summary>
    public void ActivateCanvas()
    {
        bool newState = !menuPanel.activeSelf;
        menuPanel.SetActive(newState);
    }

    // ─────────────────────────────────────────────────────────────
    // MÉTODOS PRIVADOS
    // ─────────────────────────────────────────────────────────────

    private void RefreshAllIcons()
    {
        foreach (ZoneEntry zone in zones)
        {
            UpdateIconForScene(zone.sceneName);
        }
    }

    private void UpdateIconForScene(string sceneName)
    {
        bool completed = completedScenes.Contains(sceneName);

        foreach (ZoneEntry zone in zones)
        {
            if (zone.sceneName != sceneName) continue;

            if (zone.iconNormal != null)
                zone.iconNormal.SetActive(!completed);

            if (zone.iconCompleted != null)
                zone.iconCompleted.SetActive(completed);

            break;
        }
    }
}
