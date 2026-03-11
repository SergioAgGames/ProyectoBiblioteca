using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI; 
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.EventSystems;

public class TextController : MonoBehaviour
{
    [SerializeField, TextArea (4,6)] private string[] dialogueLines;
    [SerializeField] private string[] characterName;
    [SerializeField] private Sprite[] characterSprite;
     

    [SerializeField] private GameObject textPanel;
    [SerializeField] private GameObject namePanel;
    [SerializeField] private GameObject spritePanel;

    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private UnityEngine.UI.Image characterImage;
    public Button continueButton;


    [SerializeField] private Collider2D statueCollider;

    private int lineIndex;
    private int characternamesIndex;
    private int spriteIndex;
    float typingTime = 0.05f;

    private bool textStarted = false;


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
    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;
        foreach(char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSeconds(typingTime);
        }
    }

    public void nextLine()
    {
        lineIndex++;
      
        spriteIndex++;

       
        Debug.Log(lineIndex.ToString());

        if (lineIndex < dialogueLines.Length)
        {

            StartCoroutine(ShowLine());
            answerController.ActivateAnswerPanel(lineIndex);

            if (answerController.correctText == false)
            {

                continueButton.interactable = false;
                Debug.Log("Boton deshabilitado");

            }
            /*To do list
            -minimap diferentes ubicaciones,
            -textos no repetibles enteros
            -menu inicial
            -escena creditos
            -sistema que guarde avance entre escenas: podria ser ago como un bool por zona que se activa cuando la completas, cuando teines todos completados el minimapa te falsea y te lleva a una escena inicial v2 para finalizar el juego.
            */
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
