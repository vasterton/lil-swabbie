using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class playerController : MonoBehaviour
{
    NavMeshAgent player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set destination to wherever user clicks
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            RaycastHit hit;
            
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)){
                player.destination = hit.point;
                GetComponent<Animator>().SetBool("walking", true);
            }
        }

        // Stop moving when destination has been reached
        if(player.remainingDistance < player.stoppingDistance){
            player.ResetPath();
            GetComponent<Animator>().SetBool("walking", false);
        }

        // Follow player with camera (to be replaced with fixed cameras)
        Camera.main.transform.position = player.transform.position + new Vector3 (-3, 2.2f, -3);
        Camera.main.transform.LookAt(player.transform);
    }
}
