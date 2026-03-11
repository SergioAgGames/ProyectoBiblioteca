using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controla el minimapa. Se suscribe al GameController para actualizar
/// los iconos de zonas completadas automáticamente, incluso al cambiar de escena.
/// </summary>
public class MinimapScript : MonoBehaviour
{
    [SerializeField] private GameObject menuToToggle;
    [SerializeField] private GameObject interactableObject;

    [System.Serializable]
    public class MinimapZone
    {
        [Tooltip("Nombre exacto de la escena que representa este icono")]
        public string sceneName;

        [Tooltip("Icono normal (zona no completada)")]
        public GameObject iconNormal;

        [Tooltip("Icono completado (zona completada)")]
        public GameObject iconCompleted;
    }

    [SerializeField] private List<MinimapZone> zones = new List<MinimapZone>();

    private void OnEnable()
    {
       
        if (GameController.Instance != null)
        {
            GameController.Instance.OnSceneCompleted.AddListener(OnZoneCompleted);
            
            SyncAllZones();
        }
        else
        {
            Debug.LogWarning("[MinimapScript] No se encontró el GameController. Asegúrate de que existe en la escena inicial.");
        }
    }

    private void OnDisable()
    {
        // Desuscribirse para evitar memory leaks
        if (GameController.Instance != null)
        {
            GameController.Instance.OnSceneCompleted.RemoveListener(OnZoneCompleted);
        }
    }

    /// <summary>
    /// Callback que recibe el evento cuando una zona es completada.
    /// Actualiza el icono correspondiente en el minimapa.
    /// </summary>
    private void OnZoneCompleted(string completedSceneName)
    {
        foreach (MinimapZone zone in zones)
        {
            if (zone.sceneName == completedSceneName)
            {
                SetZoneIcon(zone, true);
                Debug.Log($"[MinimapScript] Icono actualizado para: {completedSceneName}");
                return;
            }
        }
    }

    /// <summary>
    /// Al cargar el minimapa, sincroniza todos los iconos con el estado guardado en GameController.
    /// </summary>
    private void SyncAllZones()
    {
        var allStates = GameController.Instance.GetAllSceneStates();

        foreach (MinimapZone zone in zones)
        {
            bool isCompleted = allStates.ContainsKey(zone.sceneName) && allStates[zone.sceneName];
            SetZoneIcon(zone, isCompleted);
        }
    }

    /// <summary>
    /// Activa el icono correcto (normal o completado) para una zona.
    /// </summary>
    private void SetZoneIcon(MinimapZone zone, bool completed)
    {
        if (zone.iconNormal != null)
            zone.iconNormal.SetActive(!completed);

        if (zone.iconCompleted != null)
            zone.iconCompleted.SetActive(completed);
    }

    public void ActivateCanvas()
    {
        bool newState = !menuToToggle.activeSelf;
        menuToToggle.SetActive(newState);

    }
    public void InteractableObjectsState()
    {

        bool statechange = !interactableObject.activeSelf;

        interactableObject.gameObject.SetActive(statechange);
        


    }


}
