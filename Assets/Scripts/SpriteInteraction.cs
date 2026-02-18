using UnityEngine;

public class SpriteInteraction : MonoBehaviour
{
    public GameObject objetoBrillo;

    void Awake()
    {
        
            objetoBrillo.SetActive(false);
    }

    void OnMouseEnter()
    {
       
        
            objetoBrillo.SetActive(true);
    }

    void OnMouseExit()
    {
        Debug.Log("HA salio EL rtoan");
        objetoBrillo.SetActive(false);
    }
}

