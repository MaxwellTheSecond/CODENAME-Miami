using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastGun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public GameObject impact;
    Camera fpsCamera;
    PlayerHUDController pHUD;
    public GunData gun;


    private void Start()
    {
        pHUD = GameObject.FindWithTag("Player").GetComponent<PlayerHUDController>();
        fpsCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        gun.currentAmmo = gun.magSize;
        if(transform.parent != null)
        {
            gun.isEquipped=true;
            pHUD.UpdateAmmoCount(gun.currentAmmo.ToString());
        }
        else
        {
        gun.isEquipped=false;
        }
        
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && gun.currentAmmo>0 && gun.isEquipped == true && this.enabled==true)
        {
            Shoot();
        }
        if(Input.GetKeyDown(KeyCode.R) && gun.isEquipped == true && this.enabled==true)
        {
            Reload();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast (fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();

            if(target != null)
            {
            target.TakeDamage(damage);
            }

            gun.currentAmmo-=1;
            pHUD.UpdateAmmoCount(gun.currentAmmo.ToString());
            GameObject impactGO = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);
            
        }
        else
        {
            gun.currentAmmo-=1;
            pHUD.UpdateAmmoCount(gun.currentAmmo.ToString());
        }
    }
    void Reload()
    {
        Invoke("ReloadAction",gun.reloadTime);
    }
    private void ReloadAction()
    {
        gun.currentAmmo = gun.magSize;
        pHUD.UpdateAmmoCount(gun.currentAmmo.ToString());
    }

}
