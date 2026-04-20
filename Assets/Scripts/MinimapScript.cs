using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MinimapScript : MonoBehaviour
{
  
    public static MinimapScript Instance { get; private set; }

    
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Canvas minimapCanvas;

    [System.Serializable]
    public class ZoneEntry
    {
        
        public string sceneName;

        public GameObject iconNormal;

        public GameObject iconCompleted;
    }

    [SerializeField] private List<ZoneEntry> zones = new List<ZoneEntry>();

    [SerializeField] private GameObject[] allMinimapIcons;

    [SerializeField] private GameObject finalSceneIcon;

    private HashSet<string> completedScenes = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);


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
        Debug.Log($" Escena cargada: {scene.name}");

        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }

        interactableObjects = GameObject.FindGameObjectsWithTag("MapInteractable");
        if (interactableObjects != null)
        {
            foreach (GameObject obj in interactableObjects)
            {
                if (obj != null) obj.SetActive(true);
            }
        }

        if (minimapCanvas != null)
        {
            Camera newCam = Camera.main;

            if (newCam == null)
            {
                newCam = UnityEngine.Object.FindFirstObjectByType<Camera>();
            }

            if (newCam != null)
            {
                minimapCanvas.worldCamera = newCam;
                Debug.Log($"<color=cyan>[Minimap] Éxito: Cámara '{newCam.name}' asignada al Canvas en la escena {scene.name}</color>");
            }
            else
            {
                Debug.LogError($"[Minimap] ERROR: No se encontró NINGUNA cámara en la escena '{scene.name}'.");
            }
        }
        else
        {
            Debug.LogError("[Minimap] ERROR: La variable 'minimapCanvas' está vacía. ¡Arrastra el Canvas desde el Inspector!");
        }
    }
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

        CheckAllZonesCompleted();
    }

    public bool IsSceneCompleted(string sceneName)
    {
        return completedScenes.Contains(sceneName);
    }

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

    
    private void RefreshAllIcons()
    {
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
        foreach (ZoneEntry zone in zones)
        {
            if (!completedScenes.Contains(zone.sceneName))
                return false;
        }
        return zones.Count > 0;
    }

    private void ShowFinalSceneIcon()
    {
        foreach (GameObject icon in allMinimapIcons)
        {
            if (icon != null)
                icon.SetActive(false);
        }

        if (finalSceneIcon != null)
            finalSceneIcon.SetActive(true);
    }


}