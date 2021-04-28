using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The code to rotate the object has been produced following a tutorial by My.Pineapple Studio, 2020, https://www.youtube.com/watch?v=FFM2oyLUysk&t=5s
// The rest of the code such as OnTriggerStay(),OnTriggerExit() has been produced by my.
public class ReloadVR : MonoBehaviour
{
    float reloadTimer = 2f;
    bool inReloadBox = false;

    [SerializeField] private Camera playerCamera;
    private float rotationSpeed = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        //Position reloader at chest of player
        transform.position = new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y-0.4f, playerCamera.transform.position.z);
    }
    // Update is called once per frame
    void Update()
    {
        

        var rotationDifference = Mathf.Abs(playerCamera.transform.eulerAngles.y - transform.eulerAngles.y);
        var finalRotationSpeed = rotationSpeed;

        if (rotationDifference > 60)
        {
            finalRotationSpeed = rotationSpeed * 2;
        }
        else if (rotationDifference > 40 && rotationDifference < 60)
        {
            finalRotationSpeed = rotationSpeed;
        }
        else if (rotationDifference < 40 && rotationDifference > 20)
        {
            finalRotationSpeed = rotationSpeed / 2;
        }
        else if (rotationDifference < 20 && rotationDifference > 0)
        {
            finalRotationSpeed = rotationSpeed / 4;
        }

        var step = finalRotationSpeed * Time.deltaTime;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0), step);

        if (inReloadBox)
        {
            reloadTimer -= Time.deltaTime;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Gun_raycastVR>() || other.GetComponent<EnergyCannonVR>())
        {
            Debug.Log("Reloading");
            inReloadBox = true;
           
            other.GetComponent<Outline>().enabled = true;
            other.GetComponent<Outline>().OutlineColor = Color.magenta;

            if (reloadTimer <= 0)
            {
                if (other.GetComponent<Gun_raycastVR>())
                {
                    other.GetComponent<Gun_raycastVR>().Reload();
                }
                if (other.GetComponent<EnergyCannonVR>())
                {
                    other.GetComponent<EnergyCannonVR>().Reload();
                }

                other.GetComponent<Outline>().enabled = false;

                reloadTimer = 2f;
            }
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<Outline>().enabled = false;
    }
}
