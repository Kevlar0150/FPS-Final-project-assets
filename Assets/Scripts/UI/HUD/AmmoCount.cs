using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The entire code present in the script has been produced 100% by me.
public class AmmoCount : MonoBehaviour
{
    public int ammo;
    public int ammoMag;
    public Text ammoCount;

    //public GameObject pistolPrefab;

    // Start is called before the first frame update
    void Start()
    {
        ammoCount = GameObject.FindGameObjectWithTag("AmmoUI").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        ammoCount = GameObject.FindGameObjectWithTag("AmmoUI").GetComponent<Text>();
        if (GetComponentInChildren<Gun_raycast>())
        {
            ammo = GetComponentInChildren<Gun_raycast>().getBulletsRemaining();
            ammoMag = GetComponentInChildren<Gun_raycast>().getAmmoMag();
        }
        if (GetComponentInChildren<EnergyCannon>())
        {
            ammo = GetComponentInChildren<EnergyCannon>().getBulletsRemaining();
            ammoMag = GetComponentInChildren<EnergyCannon>().getAmmoMag();
        }
        ammoCount.text = ammo.ToString() + "/" + ammoMag.ToString();
    }
}
