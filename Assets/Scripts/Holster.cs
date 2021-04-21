using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Holster : MonoBehaviour
{
    public float dropTimer = 3f;
    private bool inHolsterZone = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (inHolsterZone)
        {
            dropTimer-= Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        if (other.name == "PistolVR")
        {
            Debug.Log("Touching");
            inHolsterZone = true;
            other.GetComponent<OutlineHolster>().enabled = true;
            other.GetComponent<OutlineHolster>().OutlineColor = Color.cyan;
            if (!other.GetComponent<XRGrabInteractable>().GetIsHeld())
            {
                other.transform.SetParent(gameObject.transform);
                other.transform.localPosition = Vector3.zero;
                other.GetComponent<Rigidbody>().isKinematic = true;
                Debug.Log("ATTACH");
            }
            else
            {
                other.transform.SetParent(null);
                other.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        inHolsterZone = false;
        other.GetComponent<OutlineHolster>().enabled = false;
        
    }
}
