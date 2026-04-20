using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AnswerController : MonoBehaviour
{
    [SerializeField] private GameObject AnswerPanel;
    // [SerializeField] private TMP_Text dialogueText; // Esta variable no parece usarse en este script, se puede comentar o borrar.
    [SerializeField] private int answerLine;

    [SerializeField] private InputField inputText;
    [SerializeField] private string playerAnswer;

    public bool correctText = false;

    [SerializeField] private TextController textController;

    private void Update()
    {
        TextValidate();
    }

    /// <summary>
    /// Activa el panel de respuesta o habilita el botón de continuar dependiendo de la línea actual.
    /// </summary>
    /// <param name="index">Índice de la línea de diálogo actual.</param>
    public void ActivateAnswerPanel(int index)
    {
        // === SOLUCIÓN AQUÍ ===

        if (index == answerLine)
        {
            // Es la línea de la pregunta. Mostramos el panel de respuesta.
            AnswerPanel.SetActive(true);

            // El botón de "Siguiente" debe permanecer desactivado hasta que la respuesta sea correcta.
            // (Ya fue desactivado por TextController, pero aseguramos por claridad).
            textController.continueButton.interactable = false;
        }
        else
        {
            // NO es la línea de la pregunta. Es un diálogo normal.
            AnswerPanel.SetActive(false);

            // IMPORTANTE: Habilitamos el botón para que el jugador pueda continuar leyendo.
            textController.continueButton.interactable = true;
        }
    }

    public void TextValidate()
    {
        // Solo procesar si aún no se ha respondido correctamente
        if (correctText) return;

        // NUEVO: Comprobamos si el panel de respuesta está visible antes de validar el texto.
        // Esto evita validaciones innecesarias en cada frame si no estamos en la pregunta.
        if (!AnswerPanel.activeSelf) return;

        if (inputText.text == playerAnswer)
        {
            correctText = true;
            Debug.Log("[AnswerController] Respuesta correcta");

            textController.continueButton.interactable = true;
            
            // ── Notificar al minimapa que esta escena está completada ──
            if (MinimapScript.Instance != null)
            {
                MinimapScript.Instance.CompleteCurrentScene();
            }
            else
            {
                Debug.LogWarning("[AnswerController] No se encontró el MinimapScript. ¿Está en la escena?");
            }
        }
    }
}