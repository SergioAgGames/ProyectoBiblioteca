using UnityEngine;


public class MinimapScript : MonoBehaviour
{
    [SerializeField] private GameObject menuToToggle;
    [SerializeField] private GameObject interactableObject;
    public void ActivateCanvas()
    {
        bool newState = !menuToToggle.activeSelf;
        menuToToggle.SetActive(newState);

    }
    public void InteractableObjectsState()
    {

        bool statechange = !interactableObject.activeSelf;

        interactableObject.gameObject.SetActive(statechange);
        


    }


}
