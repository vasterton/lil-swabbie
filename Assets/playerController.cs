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
        Animator animator = this.GetComponent<Animator>();

        // Set destination to wherever user clicks
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            RaycastHit hit;
            
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100) && hit.transform.CompareTag("deck")){
                player.destination = hit.point;
            }
        }

        // Stop moving when destination has been reached
        if(player.remainingDistance < player.stoppingDistance){
            player.ResetPath();
        }

        // Enable or disable walking animation based on magnitude of velocity
        animator.SetBool("walking", player.velocity.magnitude > 0.2f);
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("upperDeck")){
            Camera.main.transform.SetPositionAndRotation(new Vector3(-8.17f, 11.41f, -21.76f), Quaternion.Euler(new Vector3(20, 0, 0)));
        }
        else if(other.CompareTag("captainsQuarters")){
            Camera.main.transform.SetPositionAndRotation(new Vector3(-12.51f, 5.42f, -14.33f), Quaternion.Euler(new Vector3(10, 20, 0)));
        }
        else if(other.CompareTag("mainDeck")){
            Camera.main.transform.SetPositionAndRotation(new Vector3(-24, 10.31f, -23.8f), Quaternion.Euler(new Vector3(20, 15, 0)));
        }
        else if(other.CompareTag("mainDeckFront")){
            Camera.main.transform.SetPositionAndRotation(new Vector3(-41.36f, 10.31f, -22.53f), Quaternion.Euler(new Vector3(20, 20, 0)));
        }
        else if(other.CompareTag("lowerDeck")){
            Camera.main.transform.SetPositionAndRotation(new Vector3(-25.11f, 2.63f, -16.06f), Quaternion.Euler(new Vector3(10, 25, 0)));
        }
        else if(other.CompareTag("lowerDeckFront")){
            Camera.main.transform.SetPositionAndRotation(new Vector3(-26.3f, 2.65f, -16.03f), Quaternion.Euler(new Vector3(10, -25, 0)));
        }
        else if(other.CompareTag("galley")){
            Camera.main.transform.SetPositionAndRotation(new Vector3(-12, 2.5f, -15.71f), Quaternion.Euler(new Vector3(10, 20, 0)));
        }
    }
}