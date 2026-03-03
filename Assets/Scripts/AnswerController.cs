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
    [SerializeField] private Text inputText;

    private string playerAnswer;



    public void CheckForAnswerPanel(int index)
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


}
