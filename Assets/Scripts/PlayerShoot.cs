using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerShoot : MonoBehaviour {

    public ParticleSystem bullet;
    public GunData gun;
    public TMP_Text bulletText;

    Rigidbody rb;
    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        gun.currentAmmo = gun.magSize;
        UpdateGUI();
    }

     void Update()
    {
        Shoot();
        Reload();
    }


    private void Shoot()
    {
        var shoot = bullet.GetComponent<ParticleSystem>();
        if(Input.GetKeyDown(KeyCode.Mouse0) && gun.currentAmmo>0)
        {
            shoot.Emit(1);
            shoot.Play();
            gun.currentAmmo=gun.currentAmmo-1;
            UpdateGUI();
        }
        else shoot.Stop();
    }

    private void Reload()
    {
         if(Input.GetKeyDown(KeyCode.R))
         {
            Invoke("ReloadAction",gun.reloadTime);
         }
    }

    void ReloadAction()
    {
        gun.currentAmmo = gun.magSize;
        UpdateGUI();
    }

    void UpdateGUI()
    {
        bulletText.text = gun.currentAmmo.ToString();
    }
}