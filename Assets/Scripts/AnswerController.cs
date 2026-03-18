using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AnswerController : MonoBehaviour
{
    [SerializeField] private GameObject AnswerPanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private int answerText;

    [SerializeField] private InputField inputText;

    [SerializeField] private string playerAnswer;

    public bool correctText = false;

    [SerializeField] private TextController textController;

    private void Update()
    {
        // Se ejecuta todo el rato para comprobar si escribes lo correcto
        TextValidate();
    }

    public void ActivateAnswerPanel(int index)
    {
        // Si llegamos a la línea de la pregunta...
        if (index == answerText)
        {
            AnswerPanel.SetActive(true);

            // Si no ha acertado, mantenemos el botón apagado
            if (correctText == false)
            {
                textController.continueButton.interactable = false;
                Debug.Log("Botón deshabilitado porque apareció la pregunta");
            }
            else
            {
                // Por si acaso ya la había acertado, lo encendemos
                textController.continueButton.interactable = true;
            }
        }
        else
        {
            AnswerPanel.SetActive(false);

            // NUEVO: Si es una línea de diálogo normal, VOLVEMOS A ENCENDER el botón 
            // para que el jugador pueda seguir leyendo.
            textController.continueButton.interactable = true;
        }
    }
    public void TextValidate()
    {
        // Si el texto del Input coincide con la respuesta correcta
        if (inputText.text == playerAnswer && correctText == false)
        {
            correctText = true;
            Debug.Log("Texto Correcto");
            textController.continueButton.interactable = true; // Volvemos a encender el botón
        }
    }
}