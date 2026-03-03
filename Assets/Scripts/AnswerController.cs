using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.EventSystems;



public class AnswerController : MonoBehaviour
{
    [SerializeField] private GameObject AnswerPanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private int  answerText;
    

    [SerializeField] private InputField inputText;

    [SerializeField] private string playerAnswer;

    public bool correctText = false;

    [SerializeField] private TextController textController;
    
    private void Update()
    {
        TextValidate();
    }

    public void ActivateAnswerPanel(int index)
    {
     
        if (index == answerText)
        {
             
            AnswerPanel.SetActive(true);
        }
        else
        {
           
            AnswerPanel.SetActive(false);
        }
    }
    public void TextValidate()
    {
        if (inputText.text == playerAnswer)
        {
            correctText = true;
            Debug.Log("TExto Correcto");
            textController.continueButton.interactable = true;
        }
    }


}
