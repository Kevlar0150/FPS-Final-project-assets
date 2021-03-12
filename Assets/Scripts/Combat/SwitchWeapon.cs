using UnityEngine;

//Sources used:
//https://docs.unity3d.com/ScriptReference/GameObject.SetActive.html - SetActive function
//https://answers.unity.com/questions/785273/whats-the-difference-between-activatedeactivate-en.html - Difference between Activate/deactive and enable/disable

public class SwitchWeapon : MonoBehaviour
{
    public int currentWeapon = 0;
    void Start()
    { SelectWeapon();}
    void Update()
    {
        int previousWeapon = currentWeapon; // Previous weapon = current weapon
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // Gets value of mouse scroll wheel above 0
        {
            if (currentWeapon >= transform.childCount - 1) // If current weapon is position 0 of the list of childs of the object this script is attached to then...
            {
                currentWeapon = 0;
            }
            else
            {
                currentWeapon++;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) // Gets value of mouse scroll wheel when below 0
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
