using UnityEngine;

//Sources used:
//https://forum.unity.com/threads/solved-shotgun-shooting-c.464014/ - For shotgun creation
//https://forum.unity.com/threads/hitscan-raycasting-hits-vs-projectile-simulation.334463/ - Help deciding on what hit detection to use
//https://docs.unity3d.com/Manual/Layers.html - Help with LayerMask when shooting.

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
        // Shoot button
            if (Input.GetMouseButton(0) && Time.time >= shootInterval && !reloading && bulletsRemaining > 0)
            {
                shootInterval = Time.time + 1f / firingRate; // Sets shoot interval based on time and firing rate
                Shoot(); // Calls shoot function below
            }
        // Reload button
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
        bulletsRemaining--; // Decrease amount of bullets available to shoot

        // Bullet spread for Pistol, Assault Rifle, Revolver
        float spreadX = Random.Range(-bulletSpread, bulletSpread);
        float spreadY = Random.Range(-bulletSpread, bulletSpread);

        Vector3 spreadDirection = playerCamera.transform.forward + new Vector3(spreadX, spreadY, 0);
        var layerMask = 1 << 11; // Bit shifts the index of layer 11 (Player layer) to get bit mask
        
        layerMask = ~layerMask; // We invert it using the ~ sign so that we can collide with everything EXCEPT Layer 11 which is the player.
                                // This is so that we don't shoot ourselves.

        muzzleFlash.Play(); // Play the muzzle flash particle system  

        // Creates the ray at camera position, direction. Outputs whatever the ray touches into the hitInfo variable, 
        // length of ray is based on shoot range, Ray collides with everything except layer 11.
        if (!transform.tag.Equals("Shotgun") && Physics.Raycast(playerCamera.transform.position, spreadDirection, out hitInfo, shootRange, layerMask))
        {
            // Instantiate a gameobject with the particle effect at the point of hit. Make the effect normalized so that it is instantiated in the correct direction.
            GameObject impactVFXObject = Instantiate(impactVFX, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(impactVFXObject, 0.75f); // Destroy the impact VFX after 2 seconds.

            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        // Shotgun raycast with for loop to create multiple raycasts depending on bulletsPerShot on click.
        if (transform.tag.Equals("Shotgun"))
        {
            for (int i = 0; i <= bulletsPerShot; i++)
            {
                // Own bullet spread so that each bullet from 1 shot has a randomized spread
                float shotgunSpreadX = Random.Range(-bulletSpread, bulletSpread);
                float shotgunSpreadY = Random.Range(-bulletSpread, bulletSpread);
                Vector3 shotgunSpread = playerCamera.transform.forward + new Vector3(shotgunSpreadX, shotgunSpreadY, 0);

                if (Physics.Raycast(playerCamera.transform.position, shotgunSpread, out hitInfo, shootRange, layerMask))
                {
                    // Impact VFX
                    GameObject impactVFXObject = Instantiate(impactVFX, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    Destroy(impactVFXObject, 0.75f); // Destroy the impact VFX after 2 seconds.

                    // Damage enemy
                    Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damage);
                    }
                }
            }
        }

    }
}

