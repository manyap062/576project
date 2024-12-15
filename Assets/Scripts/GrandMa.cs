using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class GrandMa : MonoBehaviour
{
    public NavMeshAgent granny;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = new Vector3();
        target = player.transform.position;
        granny.SetDestination(target);


    }
}
