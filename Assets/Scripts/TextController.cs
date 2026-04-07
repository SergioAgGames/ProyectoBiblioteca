using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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
    [SerializeField] private Image blackFadeImage;
    [SerializeField] private float fadeSpeed = 1f;


    private bool yaInteractuado = false;

    public void StartDialogue()
    {
        if (yaInteractuado) return;
        yaInteractuado = true;

        textStarted = true;
        textPanel.SetActive(true);
        lineIndex = 0;
        characternamesIndex = 0;
        spriteIndex = 0;
        foreach (Collider2D col in objectCollider)
        {

            if (col != null)
            {
                col.enabled = false;
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

            string currentScene = SceneManager.GetActiveScene().name;
            GameController.Instance?.CompleteScene(currentScene);
            if (cambiarEscenaAlTerminar)
            {

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
        continueButton.interactable = false;
        dialogueText.text = string.Empty;
        foreach (char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSeconds(typingTime);
        }

        if (tienePreguntas && answerController != null)
        {

            answerController.ActivateAnswerPanel(lineIndex);
        }
        else
        {

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
    private IEnumerator FadeAndChangeScene()
    {

        blackFadeImage.gameObject.SetActive(true);

        Color fadeColor = blackFadeImage.color;
        float alpha = 0f;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeColor.a = alpha;
            blackFadeImage.color = fadeColor;
            yield return null;
        }
        SceneManager.LoadScene(nombreSiguienteEscena);
    }
}