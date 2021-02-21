﻿using UnityEngine;

//Sources used:
//https://docs.unity3d.com/ScriptReference/GameObject.SetActive.html - SetActive function
//https://answers.unity.com/questions/785273/whats-the-difference-between-activatedeactivate-en.html - Difference between Activate/deactive and enable/disable

public class SwitchWeapon : MonoBehaviour
{
    public int currentWeapon = 0;
    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();  
    }

    // Update is called once per frame
    void Update()
    {
        int previousWeapon = currentWeapon;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (currentWeapon >= transform.childCount - 1)
            {
                currentWeapon = 0;
            }
            else
            {
                currentWeapon++;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (currentWeapon <= 0)
            {
                currentWeapon = transform.childCount - 1;
            }
            else
            {
                currentWeapon--;
            }
        }
        if (previousWeapon != currentWeapon)
        {
            SelectWeapon();
        }
    }
    private void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == currentWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
