using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float duration = 1.5f; // Tiempo en segundos
    FinalEsceneText finalescenetext;

    void Start()
    {
        // Aseguramos que empiece invisible
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
            // Interpolación lineal entre 0 y 1
            canvasGroup.alpha = Mathf.Lerp(0, 1, counter / duration);
            yield return null;
        }

        canvasGroup.alpha = 1; // Asegurar que quede al máximo

        finalescenetext.StartDialogue();
    }
}