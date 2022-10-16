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
    public GunData gunData;
    private float currentAmmo;
    public float CurrentAmmo{ get {return currentAmmo;} set{currentAmmo = value;}}
    private bool isEquipped = false;
    public bool IsEquipped{ get {return isEquipped;} set{isEquipped=value;}}
    public Animation anim;

    private void Start()
    {
        pHUD = GameObject.FindWithTag("Player").GetComponent<PlayerHUDController>();
        fpsCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        //anim = GetComponent<Animation>();
        currentAmmo = gunData.magSize;
        if(transform.parent != null)
        {
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            Collider coll = transform.GetComponent<Collider>();
            rb.isKinematic=true;
            coll.enabled=false;
            isEquipped=true;
            pHUD.UpdateAmmoCount(currentAmmo.ToString());
        }
        else
        {
        isEquipped=false;
        }
        
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && currentAmmo>0 && isEquipped == true && this.enabled==true)
        {
            Shoot();
        }
        if(Input.GetKeyDown(KeyCode.R) && isEquipped == true && this.enabled==true && currentAmmo < gunData.magSize)
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

            currentAmmo-=1;
            pHUD.UpdateAmmoCount(currentAmmo.ToString());
            GameObject impactGO = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);

            
        }
        else
        {
            currentAmmo-=1;
            pHUD.UpdateAmmoCount(currentAmmo.ToString());
        }
    }
    void Reload()
    {
        //Invoke("ReloadAction", gunData.reloadTime);
        ReloadAction();
    }

    void ReloadOneBullet()
    {
        currentAmmo++;
        pHUD.UpdateAmmoCount(currentAmmo.ToString());
    }
    private void ReloadAction()
    {
        StartCoroutine("WaitForReload");
    }

    IEnumerator WaitForReload()
    {
        while(currentAmmo < gunData.magSize)
        {
        anim.Play("PistolReload");
        yield return new WaitForSeconds(0.3f);
        }
    }

    


}
