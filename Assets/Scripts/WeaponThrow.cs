using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponThrow : MonoBehaviour
{
    Rigidbody rb;
    Collider coll;
    Transform player, gunContainer, fpsCam;
    PlayerHUDController pHUD;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;
    public GunData gun;
    public bool equipped;
    public static bool slotFull;
    

    private void Start() {
        pHUD = GameObject.FindWithTag("Player").GetComponent<PlayerHUDController>();
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        if(gun.isEquipped)
        {
        rb.isKinematic=true;
        coll.enabled=false;
        }
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        gunContainer = GameObject.FindWithTag("WeaponHolster").GetComponent<Transform>();
        fpsCam = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        Debug.Log(player);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && gun.isEquipped)
        {
            Throw();
        }
        Vector3 distanceToPlayer = player.position - transform.position;
        if(Input.GetKeyDown(KeyCode.E) && !slotFull && distanceToPlayer.magnitude <= pickUpRange)
        {
            PickUp();
        }
    }

    private void Throw()
    {
        gun.isEquipped = false;
        equipped = false;
        slotFull = false;
        pHUD.UpdateAmmoCount("N/A");

        //Set parent to null
        transform.SetParent(null);

        //Make Rigidbody not kinematic and BoxCollider normal
        rb.isKinematic = false;
        coll.enabled = true;

        //Gun carries momentum of player
        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        //AddForce
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
        //Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);
    }

    private void PickUp()
    {
        gun.isEquipped = true;
        equipped = true;
        slotFull = true;
        pHUD.UpdateAmmoCount(gun.currentAmmo.ToString());

        //Make weapon a child of the camera and move it to default position
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        //Make Rigidbody kinematic and BoxCollider a trigger
        rb.isKinematic = true;
        coll.enabled = false;
    }
}