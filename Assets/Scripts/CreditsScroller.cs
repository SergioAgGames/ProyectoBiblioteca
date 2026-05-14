using UnityEngine;
using UnityEngine.SceneManagement;
public class CreditsScroller : MonoBehaviour
{
    [SerializeField] float speed = 50f; 
    [SerializeField] float exitPositionY = 1500f; 

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.localPosition.y > exitPositionY)
        {
            SceneManager.LoadScene("1.0.MenuScene");
        }
    }
}