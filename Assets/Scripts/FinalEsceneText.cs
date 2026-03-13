using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI; 
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.EventSystems;

public class FinalEsceneText : MonoBehaviour
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


 

    private int lineIndex;
    private int characternamesIndex;
    private int spriteIndex;
    float typingTime = 0.05f;

    private bool textStarted = false;


    [SerializeField] private FinalAnswerController answerController;
    public void StartDialogue()
    {
        textStarted = true;
   
        textPanel.SetActive(true);
        lineIndex = 0;
        characternamesIndex = 0;
        spriteIndex = 0;

       
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
          
        }
        else
        {
           
            textPanel.SetActive(false);

           

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
