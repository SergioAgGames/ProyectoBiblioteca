using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AnswerController : MonoBehaviour
{
    [SerializeField] private GameObject AnswerPanel;
   
    [SerializeField] private int answerLine;

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


        if (index == answerLine)
        {
            AnswerPanel.SetActive(true);
            textController.continueButton.interactable = false;
        }
        else
        {
            AnswerPanel.SetActive(false);

            textController.continueButton.interactable = true;
        }
    }

    public void TextValidate()
    {
        if (correctText) return;

        if (!AnswerPanel.activeSelf) return;

        if (inputText.text == playerAnswer)
        {
            correctText = true;
            Debug.Log("[AnswerController] Respuesta correcta");

            textController.continueButton.interactable = true;
            
            if (MinimapScript.Instance != null)
            {
                MinimapScript.Instance.CompleteCurrentScene();
            }
           
        }
    }
}