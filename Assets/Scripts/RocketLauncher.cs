using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject muzzlePosition;
    public ParticleSystem muzzleFlash;
    public GameObject bulletPrefab;
    public int shootInterval = 69;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            shootInterval++; // Increases shoot interval so there requires time between each shot (Can't just hold it down like a machine gun)
            if (shootInterval >= 75)
            {
                muzzleFlash.Play();
                GameObject bulletObject = Instantiate(bulletPrefab); // Instantiate bullet mesh being passed in
                bulletObject.transform.position = muzzlePosition.transform.position + playerCamera.transform.forward; // Set position of the bullet
                bulletObject.transform.forward = playerCamera.transform.forward; // Set facing direction of the bullet
                Debug.Log("Fire!"); // Debug log to see if this If statement works.
                shootInterval = 0;
            }
        }
    }
}
