using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourHorizontal : MonoBehaviour
{
    public float combineBoxRange;
    private Transform player;
    int ID;

    void Start() 
    {
        ID = GetInstanceID();
        player = GameObject.FindGameObjectWithTag("PlayerBody").GetComponent<Transform>();
    }

    void Update() 
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        if(distanceToPlayer.magnitude <= combineBoxRange && Input.GetKeyDown(KeyCode.G))
        {
            CombineBoxs();
        }
    }

    void CombineBoxs()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        Instantiate(Resources.Load("WeaponPrototype"), transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.CompareTag("BoxD"))
        {
            if(ID < other.gameObject.GetComponent<FourHorizontal>().ID)
            {
                return;
            }
            transform.position = new Vector3(transform.position.x, 1.25f, transform.position.z);

            Instantiate(Resources.Load("BoxCombination/BoxFull"), transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }    
}
