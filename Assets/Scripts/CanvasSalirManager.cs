using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CanvasSalirManager : MonoBehaviour
{
    
    public GameObject pantallaCanvas;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            pantallaCanvas.SetActive(pantallaCanvas);

        }
    }
    public void CerrarJuego()
    {

        Application.Quit();

    }
    public void OcultarCanvas()
    {
        if (pantallaCanvas != null)
            pantallaCanvas.SetActive(false);
    }
}