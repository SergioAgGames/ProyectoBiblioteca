using UnityEngine;
using TMPro;
using System.Collections;
public class TextController : MonoBehaviour
{
    [SerializeField, TextArea (4,6)] private string[] dialogueLines;
    [SerializeField] private GameObject textPanel;
    [SerializeField] private GameObject namePanel;
    [SerializeField] private GameObject spritePanel;

    [SerializeField] private TMP_Text dialogueText;

    private bool dialogueStarted = false;
    private int lineIndex;
    float typingTime = 0.05f;
    void Update()
    {
        
    }

    public void StartDialogue()
    {
        dialogueStarted = true;
        textPanel.SetActive(true);
        lineIndex = 0;
        StartCoroutine(ShowLine());
        namePanel.SetActive(true);
        spritePanel.SetActive(true);
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

        if (lineIndex < dialogueLines.Length) 
        {

            StartCoroutine(ShowLine());
        }
        else
        {
            dialogueStarted=false;
            textPanel.SetActive(false);
            namePanel.SetActive(false);
            spritePanel.SetActive(false);

        }
    }
}
