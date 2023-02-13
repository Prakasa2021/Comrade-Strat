using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickNDropBox : MonoBehaviour
{
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, boxContainer, tppCam;
    public GameObject[] dropArea;

    public float pickUpRange, dropRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    Collider colliderBox;

    public GameObject combineBox;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerBody").GetComponent<Transform>();
        boxContainer = GameObject.FindGameObjectWithTag("BoxContainer").GetComponent<Transform>();
        tppCam = GameObject.FindGameObjectWithTag("BoxHandler").GetComponent<Transform>();
        dropArea = GameObject.FindGameObjectsWithTag("DropArea");
        combineBox = GameObject.FindGameObjectWithTag("DropAreaParent");
        colliderBox = GetComponent<Collider>();

        if(!equipped)
        {
            rb.isKinematic = false;
            coll.isTrigger = false;
        }

        if(equipped)
        {
            rb.isKinematic = true;
            coll.isTrigger = true;
        }
    }

    private void Update()
    {       
        Vector3 distanceToPlayer = player.position - transform.position;
        if(!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull)
        {
            PickUp();
        }

        if(equipped && Input.GetKeyDown(KeyCode.F))
        {
            Drop();
        }
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        transform.SetParent(boxContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        rb.isKinematic = true;
        coll.isTrigger = true;
        colliderBox.enabled = false;
    
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Weapon"))
        {
            go.GetComponent<PickNDropWeapon>().enabled = false;
        }
    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        coll.isTrigger = false;
        colliderBox.enabled = true;

        foreach(GameObject dropAr in dropArea)
        {
            if(dropAr.GetComponent<DropArea>().isTriggerArea == true)
            {
                var rayOrigin = player.transform.position;
                var rayDirection = dropAr.transform.position - player.transform.position;
                RaycastHit hitInfo;
                
                if(Physics.Raycast(rayOrigin, rayDirection, out hitInfo))
                {
                    coll.isTrigger = true;
                    rb.isKinematic = true;
                    dropAr.GetComponent<DropAreaP>().isFill = true;
                    combineBox.GetComponent<CombineBox>().Boxs.Add(dropAr.GetComponent<DropAreaP>().idBox);

                    transform.SetParent(dropAr.transform);
                    transform.position = dropAr.transform.position;
                    transform.localRotation = dropAr.transform.localRotation;
                }
                transform.GetComponent<Collider>().enabled = true;
            }
            else
            {
                rb.AddForce(tppCam.forward * dropForwardForce, ForceMode.Impulse);
                rb.AddForce(tppCam.up * dropUpwardForce, ForceMode.Impulse);

                float random = Random.Range(-1f, 1f);
                rb.AddTorque(new Vector3(random, random, random) * 10);
            }
        }
    
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Weapon"))
        {
            go.GetComponent<PickNDropWeapon>().enabled = true;
        }
    }
}