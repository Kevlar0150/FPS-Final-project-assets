using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class SwordCombatVR : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(gameObject.GetComponent<XRGrabInteractable>().GetIsHeld());

    }
    public void EnableOutline()
    {
        if (transform.parent == null)
        {
            GetComponent<Outline>().OutlineColor = Color.green;
            GetComponent<Outline>().enabled = true;
        }
    }
    public void DisableOutline()
    {
        GetComponent<Outline>().enabled = false;
    }
}
