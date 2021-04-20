using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Holster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        if (other.name == "PistolVR")
        {
            other.GetComponent<OutlineHolster>().enabled = true;
            other.GetComponent<OutlineHolster>().OutlineColor = Color.cyan;
            if (!other.GetComponent<XRGrabInteractable>().GetIsHeld())
            {
                Debug.Log("ATTACH");
            } 
        }
    }
    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<OutlineHolster>().enabled = false;
    }
}
