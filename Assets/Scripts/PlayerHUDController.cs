using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerHUDController : MonoBehaviour
{
    private TMP_Text ammoCount;
    
    private void Start() 
    {
        ammoCount = GameObject.FindWithTag("AmmoCounter").GetComponent<TMP_Text>();
        ammoCount.text = "N/A";
    }
    public void UpdateAmmoCount(string text)
    {
        ammoCount.text = text;
    }
}
