using UnityEngine;

public class Gun_raycast : MonoBehaviour
{
    // Gun properties
    public float damage;
    public float firingRate;
    public float shootRange;
    public float bulletSpread;
    public float reloadTime;
    private float shootInterval = 0f;
    public int bulletsPerShot;
    public int magazineSize;

    int bulletsRemaining;
    bool reloading;

    // References
    public Camera playerCamera;
    public ParticleSystem muzzleFlash;
    public GameObject impactVFX;
    public RaycastHit hitInfo; // Store's whatever the raycast hits into the variable hitInfo
    
    private void Start()
    {
        bulletsRemaining = magazineSize;
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetMouseButton(0) && Time.time >= shootInterval && !reloading && bulletsRemaining > 0)
            {
                shootInterval = Time.time + 1f / firingRate; // Sets shoot interval based on time and firing rate
                Shoot(); // Calls shoot function below
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletsRemaining < magazineSize && !reloading)
            { Reload();}
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsRemaining = magazineSize;
        reloading = false;
    }

    void Shoot()
    {
        bulletsRemaining--;

        float spreadX = Random.Range(-bulletSpread, bulletSpread);
        float spreadY = Random.Range(-bulletSpread, bulletSpread);

        Vector3 spreadDirection = playerCamera.transform.forward + new Vector3(spreadX, spreadY, 0);
        var layerMask = 1 << 11; // Bit shifts the index of layer 11 (Player layer) to get bit mask
        
        layerMask = ~layerMask; // We invert it using the ~ sign so that we can collide with everything EXCEPT Layer 11 which is the player.
                                // This is so that we don't shoot ourselves.

        muzzleFlash.Play(); // Play the muzzle flash particle system

        // Creates the ray at camera position, direction. Outputs whatever the ray touches into the hitInfo variable, 
        // length of ray is based on shoot range, Ray collides with everything except layer 11.
        if (Physics.Raycast(playerCamera.transform.position, spreadDirection, out hitInfo, shootRange, layerMask))
        {
            //Debug.Log(hitInfo.transform.name);

            // Instantiate a gameobject with the particle effect at the point of hit. Make the effect normalized so that it is instantiated in the correct direction.
            GameObject impactVFXObject = Instantiate(impactVFX, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

            Destroy(impactVFXObject, 2f); // Destroy the impact VFX after 2 seconds.
        }
        
    }
}

