using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponThrow : MonoBehaviour
{
    Transform player, gunContainer, fpsCam;
    PlayerHUDController pHUD;

    public float pickUpRange = 100f;
    public float dropForwardForce, dropUpwardForce;
    private bool canPickup = true;
    private IEnumerator coroutine;
    public Animation anim;

    private void Start() {
        pHUD = GameObject.FindWithTag("Player").GetComponent<PlayerHUDController>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        gunContainer = GameObject.FindWithTag("WeaponHolster").GetComponent<Transform>();
        fpsCam = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1) && gunContainer.transform.childCount>0 && anim.isPlaying == false)
        {
            coroutine = ThrowAction();
            StartCoroutine(coroutine);
        }
        else if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            RaycastHit hit;
            bool isHit = (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, pickUpRange));
            if(isHit && hit.collider.tag == "Gun" && gunContainer.transform.childCount==0)
            {
            PickUp(hit.transform.gameObject);
            }
            else
            {
                 isHit = (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, 5*pickUpRange));
                 if(isHit && hit.collider.tag == "Gun")
                 PullTowards(hit.transform.gameObject);
            }
        }
    }
    IEnumerator ThrowAction()
    {
        anim.Play("PistolThrow");
        yield return new WaitForSeconds(0.69f);
        Throw(gunContainer.GetChild(0).transform.gameObject);
    }

    private void OnCollisionEnter(Collision other) {
        if(gunContainer.transform.childCount==0 && other.gameObject.tag=="Player" && canPickup)
        PickUp(transform.gameObject);
    }

    IEnumerator PickupTimer(float waitTime)
    {
        canPickup = false;
        yield return new WaitForSeconds(waitTime);
        canPickup = true;
    }

    private void PullTowards(GameObject thisGunObject)
    {
        Rigidbody rb = thisGunObject.GetComponent<Rigidbody>();
        Transform playerOrientation = GameObject.FindWithTag("Player").GetComponent<Transform>();
        Vector3 pullDirection = new Vector3(0,0.7f,0) + -(thisGunObject.transform.position - playerOrientation.position).normalized;
        rb.AddForce(pullDirection * Vector3.Distance(thisGunObject.transform.position,playerOrientation.position), ForceMode.Impulse);
    }

    private void Throw(GameObject thisGunObject)
    {
        coroutine = PickupTimer(2f);
        StartCoroutine(coroutine);
        thisGunObject.GetComponent<RaycastGun>().IsEquipped = false;
        pHUD.UpdateAmmoCount("");

        //Set parent to null
        thisGunObject.transform.SetParent(null);
        Rigidbody rb = thisGunObject.GetComponent<Rigidbody>();
        Collider coll = thisGunObject.GetComponent<Collider>();
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

    private void PickUp(GameObject thisGunObject)
    {
        RaycastGun thisGunScript = thisGunObject.GetComponent<RaycastGun>();
        thisGunScript.IsEquipped = true;
        pHUD.UpdateAmmoCount(thisGunScript.CurrentAmmo.ToString());

        //Make weapon a child of the camera and move it to default position
        thisGunObject.transform.SetParent(gunContainer);
        thisGunObject.transform.localPosition = Vector3.zero;
        thisGunObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        thisGunObject.transform.localScale = Vector3.one;

        //Make Rigidbody kinematic and BoxCollider a trigger
        thisGunObject.GetComponent<Rigidbody>().isKinematic = true;
        thisGunObject.GetComponent<Collider>().enabled = false;
    }

}