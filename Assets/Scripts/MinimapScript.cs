using UnityEngine;


public class MinimapScript : MonoBehaviour
{
    [SerializeField] private GameObject menuToToggle;

    public void ActivateCanvas()
    {
        bool newState = !menuToToggle.activeSelf;
        menuToToggle.SetActive(newState);




    }


}
