using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class SwordHolster : MonoBehaviour
{
    public float dropTimer = 1f;
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
            dropTimer -= Time.deltaTime;
        }

    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.transform.name);

      
        if (other.GetComponent<SwordCombatVR>())
        {
            Debug.Log(gameObject.transform.name);
            inHolsterZone = true;
            other.GetComponent<OutlineHolster>().enabled = true;
            other.GetComponent<OutlineHolster>().OutlineColor = Color.yellow;
            
            if (transform.childCount <= 0)
            {
                if (dropTimer < 1)
                {
                    other.GetComponent<OutlineHolster>().OutlineColor = Color.cyan;
                    if (!other.GetComponent<XRGrabInteractable>().GetIsHeld())
                    {
                        other.GetComponent<Rigidbody>().isKinematic = true;
                        other.transform.SetParent(this.gameObject.transform);
                        other.transform.localPosition = Vector3.zero;
            
                    }
                }
            }
            else
            {
                other.GetComponent<OutlineHolster>().OutlineColor = Color.red;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        inHolsterZone = false;
        other.GetComponent<OutlineHolster>().enabled = false;
        dropTimer = 1f;
    }
}
