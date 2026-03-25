using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Quité Microsoft.Unity.VisualStudio.Editor que no es necesario aquí
using UnityEngine.SceneManagement;


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

    [SerializeField] private Collider2D[] objectCollider;

    private int lineIndex;
    private int characternamesIndex;
    private int spriteIndex;
    float typingTime = 0.02f;

    private bool textStarted = false;
    private bool showLine = false;
    [SerializeField] private AnswerController answerController;




    [SerializeField] private bool cambiarEscenaAlTerminar = false;
    [SerializeField] private string nombreSiguienteEscena;
    [SerializeField] private Image blackFadeImage; // Arrastra aquí tu FadeImage
    [SerializeField] private float fadeSpeed = 1f;

    public void StartDialogue()
    {
        textStarted = true;

        textPanel.SetActive(true);
        lineIndex = 0;
        characternamesIndex = 0;
        spriteIndex = 0;

        foreach (Collider2D col in objectCollider)
        {
            // Verificamos que no esté vacío (por si olvidaste asignar alguno en el Inspector)
            if (col != null)
            {
                col.enabled = false; // Desactivamos este collider específico
            }
        }

        StartCoroutine(ShowLine());
    }

    public void nextLine()
    {
        continueButton.interactable = false;

        lineIndex++;
        spriteIndex++;

        Debug.Log("Línea actual: " + lineIndex.ToString());

        if (lineIndex < dialogueLines.Length)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            textPanel.SetActive(false);

            // Diálogo terminado marcar esta escena como completada
            string currentScene = SceneManager.GetActiveScene().name;
            GameController.Instance?.CompleteScene(currentScene);

            // NUEVO: Comprobamos si esta interacción debe cambiar la escena
            if (cambiarEscenaAlTerminar)
            {
                // Iniciamos el fundido en negro y el cambio de escena
                StartCoroutine(FadeAndChangeScene());
            }
            else
            {

                foreach (Collider2D col in objectCollider)
                {
                    if (col != null)
                    {
                        col.enabled = true; 
                    }
                }
            }
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

    // NUEVO: Corrutina para el fundido a negro
    private IEnumerator FadeAndChangeScene()
    {
        // 1. Activamos la imagen negra (que ahora mismo es transparente)
        blackFadeImage.gameObject.SetActive(true);

        // Obtenemos su color actual
        Color fadeColor = blackFadeImage.color;
        float alpha = 0f;

        // 2. Bucle que va aumentando la opacidad poco a poco
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed; // Aumenta el alpha según la velocidad
            fadeColor.a = alpha;                 // Aplicamos el nuevo alpha
            blackFadeImage.color = fadeColor;    // Guardamos el color en la imagen

            yield return null; // Esperamos al siguiente frame para seguir oscureciendo
        }

        // 3. Cuando la pantalla está totalmente negra, cargamos la nueva escena
        SceneManager.LoadScene(nombreSiguienteEscena);
    }
}