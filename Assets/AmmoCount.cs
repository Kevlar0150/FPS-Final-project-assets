using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCount : MonoBehaviour
{
    public int ammo = 30;
    public int ammoMag = 90;
    public Text ammoCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ammoCount.text = ammo.ToString() + "/" + ammoMag.ToString();
    }
}
