using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Quité Microsoft.Unity.VisualStudio.Editor que no es necesario aquí

public class TextController : MonoBehaviour
{
    [SerializeField, TextArea(4, 6)] private string[] dialogueLines;
    [SerializeField] private string[] characterName;
    [SerializeField] private Sprite[] characterSprite;

    [SerializeField] private GameObject textPanel;
    [SerializeField] private GameObject namePanel;
    [SerializeField] private GameObject spritePanel;

    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private UnityEngine.UI.Image characterImage;
    public Button continueButton;

    // NUEVO: Este es tu interruptor mágico. Si está en false, ignorará el AnswerController.
    [SerializeField] private bool tienePreguntas = false;

    [SerializeField] private Collider2D statueCollider;

    private int lineIndex;
    private int characternamesIndex;
    private int spriteIndex;
    float typingTime = 0.02f;

    private bool textStarted = false;
    private bool showLine = false;
    [SerializeField] private AnswerController answerController;

    public void StartDialogue()
    {
        textStarted = true;

        textPanel.SetActive(true);
        lineIndex = 0;
        characternamesIndex = 0;
        spriteIndex = 0;

        statueCollider.enabled = false;
        StartCoroutine(ShowLine());
    }

    public void nextLine()
    {
        // NUEVO: Apagamos el botón nada más pulsarlo para evitar el "doble clic"
        continueButton.interactable = false;

        lineIndex++;
        spriteIndex++;

        Debug.Log("Línea actual: " + lineIndex.ToString());

        if (lineIndex < dialogueLines.Length)
        {
            StartCoroutine(ShowLine());
            // Hemos quitado la llamada a ActivateAnswerPanel de aquí. 
            // Ahora se llamará cuando termine de escribir el texto.
        }
        else
        {
            textPanel.SetActive(false);
            statueCollider.enabled = true;

            // Diálogo terminado  marcar esta escena como completada
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            GameController.Instance?.CompleteScene(currentScene);
        }
    }

    private IEnumerator ShowLine()
    {
        // NUEVO: Aseguramos que el botón esté apagado mientras las letras aparecen
        continueButton.interactable = false;

        dialogueText.text = string.Empty;
        foreach (char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSeconds(typingTime);
        }

        // NUEVO: El texto ha terminado de escribirse.
        if (tienePreguntas && answerController != null)
        {
            // Le pasamos el control al AnswerController. 
            // Él decidirá si apaga el botón (si es pregunta) o lo enciende (si es diálogo normal).
            answerController.ActivateAnswerPanel(lineIndex);
        }
        else
        {
            // Si la estatua NO tiene preguntas, simplemente encendemos el botón para que lea la siguiente línea.
            continueButton.interactable = true;
        }
    }
    public void UpdateCharacterInfo()
    {
        if (characterName.Length > lineIndex)
        {
            nameText.text = characterName[lineIndex];
        }

        if (characterSprite.Length > lineIndex)
        {
            characterImage.sprite = characterSprite[lineIndex];
        }
    }
}