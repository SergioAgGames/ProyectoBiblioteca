using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float duration = 1.5f; 
    [SerializeField] private FinalEsceneText finalescenetext;

    void Start()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade()
    {
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            
            canvasGroup.alpha = Mathf.Lerp(0, 1, counter / duration);
            yield return null;
        }

        canvasGroup.alpha = 1; 

        finalescenetext.StartDialogue();
    }
}