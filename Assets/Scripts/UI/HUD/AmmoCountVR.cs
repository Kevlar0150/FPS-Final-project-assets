using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCountVR : MonoBehaviour
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
        ammo = GetComponentInChildren<Gun_raycastVR>().getBulletsRemaining();
        ammoMag = GetComponentInChildren<Gun_raycastVR>().getAmmoMag();
        ammoCount.text = ammo.ToString() + "/" + ammoMag.ToString();
    }
}
