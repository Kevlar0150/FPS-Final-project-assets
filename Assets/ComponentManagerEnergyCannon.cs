using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class ComponentManagerEnergyCannon : MonoBehaviour
{
    XRGrabInteractable grabScript;
    LineRenderer lineRenderer;
    EnergyCannonVR gunScript;
    // Start is called before the first frame update
    void Start()
    {
        gunScript = GetComponent<EnergyCannonVR>();
        grabScript = GetComponent<XRGrabInteractable>();
        lineRenderer = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (grabScript.GetIsHeld())
        {
            gunScript.enabled = true;
            lineRenderer.enabled = true;
        }
        else
        {
            gunScript.enabled = false;
            lineRenderer.enabled = false;
        }
    }
}