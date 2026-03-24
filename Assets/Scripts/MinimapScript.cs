using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MinimapScript : MonoBehaviour
{
    // ── Singleton ─────────────────────────────────────────────────
    public static MinimapScript Instance { get; private set; }

    // ── Inspector ─────────────────────────────────────────────────
    [SerializeField] private GameObject menuPanel;

    [System.Serializable]
    public class ZoneEntry
    {
        [Tooltip("Nombre exacto de la escena (igual que en File > Build Settings)")]
        public string sceneName;

        [Tooltip("Icono del menú cuando la zona NO está completada")]
        public GameObject iconNormal;

        [Tooltip("Icono del menú cuando la zona SÍ está completada")]
        public GameObject iconCompleted;
    }

    [SerializeField] private List<ZoneEntry> zones = new List<ZoneEntry>();

    [Header("Escena Final")]
    [Tooltip("Todos los iconos que se ocultarán cuando todas las escenas estén completadas")]
    [SerializeField] private GameObject[] allMinimapIcons;

    [Tooltip("Icono de la escena final, oculto hasta que se completen todas las zonas")]
    [SerializeField] private GameObject finalSceneIcon;

    // ── Estado interno ────────────────────────────────────────────
    private HashSet<string> completedScenes = new HashSet<string>();

    // ─────────────────────────────────────────────────────────────
    // UNITY CALLBACKS
    // ─────────────────────────────────────────────────────────────

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Asegurarse de que el icono final empieza oculto
        if (finalSceneIcon != null)
            finalSceneIcon.SetActive(false);

        RefreshAllIcons();
    }

    private void OnEnable()
    {
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
    // API PÚBLICA
    // ─────────────────────────────────────────────────────────────

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

        // Comprobar si todas las zonas están completas
        CheckAllZonesCompleted();
    }

    public bool IsSceneCompleted(string sceneName)
    {
        return completedScenes.Contains(sceneName);
    }

    // ─────────────────────────────────────────────────────────────
    // LÓGICA DEL MENÚ
    // ─────────────────────────────────────────────────────────────

    public void ActivateCanvas()
    {
        bool newState = !menuPanel.activeSelf;
        menuPanel.SetActive(newState);
    }

    [SerializeField] private GameObject[] interactableObjects;

    public void InteractableObjectsState()
    {
        bool stateChange = !interactableObjects[0].activeSelf;
        foreach (GameObject obj in interactableObjects)
        {
            obj.SetActive(stateChange);
        }
    }

    // ─────────────────────────────────────────────────────────────
    // MÉTODOS PRIVADOS
    // ─────────────────────────────────────────────────────────────

    private void RefreshAllIcons()
    {
        // Si ya están todas completadas, mantener el estado final
        if (AreAllZonesCompleted())
        {
            ShowFinalSceneIcon();
            return;
        }

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

    private void CheckAllZonesCompleted()
    {
        if (!AreAllZonesCompleted()) return;

        Debug.Log("[Minimap] ¡Todas las zonas completadas!");
        ShowFinalSceneIcon();
    }

    private bool AreAllZonesCompleted()
    {
        // Comprueba que cada zona registrada está en el HashSet de completadas
        foreach (ZoneEntry zone in zones)
        {
            if (!completedScenes.Contains(zone.sceneName))
                return false;
        }
        return zones.Count > 0;
    }

    private void ShowFinalSceneIcon()
    {
        // Ocultar todos los iconos normales del minimapa
        foreach (GameObject icon in allMinimapIcons)
        {
            if (icon != null)
                icon.SetActive(false);
        }

        // Mostrar el icono de la escena final
        if (finalSceneIcon != null)
            finalSceneIcon.SetActive(true);
    }
}