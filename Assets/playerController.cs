using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    NavMeshAgent player;
    bool cookGameStarted = false;
    bool fishHeld = false;
    bool fishReturned = false;
    bool cheeseHeld = false;
    bool cheeseReturned = false;
    bool breadHeld = false;
    bool breadReturned = false;

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
            NavMeshHit meshHit;
            
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100) && NavMesh.SamplePosition(hit.point, out meshHit, .5f, NavMesh.AllAreas)){
                player.destination = meshHit.position;
            }
        }

        // Stop moving when destination has been reached
        if(player.remainingDistance < player.stoppingDistance){
            player.ResetPath();
        }

        // Enable or disable walking animation based on magnitude of velocity
        animator.SetBool("walking", player.velocity.magnitude > 0.2f);

        // Press E to interact
        if(Input.GetKeyDown(KeyCode.E)){
            // Begin cook minigame - this will probably be handled differently once interactions are implemented
            if(GameObject.FindGameObjectWithTag("cookMinigameText").GetComponent<Text>().enabled){
                cookGameStarted = true;

                GameObject[] fishObjects = GameObject.FindGameObjectsWithTag("fish");
                foreach(GameObject fish in fishObjects){
                    fish.GetComponent<MeshRenderer>().enabled = true;
                }

                GameObject.FindGameObjectWithTag("cheese").GetComponent<MeshRenderer>().enabled = true;

                GameObject.FindGameObjectWithTag("bread").GetComponent<MeshRenderer>().enabled = true;
                
                GameObject.FindGameObjectWithTag("cookMinigameText").GetComponent<Text>().enabled = false;
            }
            // Interact with ingredients
            else if(GameObject.FindGameObjectWithTag("fishText").GetComponent<Text>().enabled){
                fishHeld = true;

                GameObject[] fishObjects = GameObject.FindGameObjectsWithTag("fish");
                foreach(GameObject fish in fishObjects){
                    fish.GetComponent<MeshRenderer>().enabled = false;
                }

                GameObject[] heldFishObjects = GameObject.FindGameObjectsWithTag("fishHeld");
                foreach(GameObject fish in heldFishObjects){
                    fish.GetComponent<MeshRenderer>().enabled = true;
                }

                GameObject.FindGameObjectWithTag("fishText").GetComponent<Text>().enabled = false;
            }
            else if(GameObject.FindGameObjectWithTag("cheeseText").GetComponent<Text>().enabled){
                cheeseHeld = true;

                GameObject.FindGameObjectWithTag("cheese").GetComponent<MeshRenderer>().enabled = false;

                GameObject.FindGameObjectWithTag("cheeseHeld").GetComponent<MeshRenderer>().enabled = true;

                GameObject.FindGameObjectWithTag("cheeseText").GetComponent<Text>().enabled = false;
            }
            else if(GameObject.FindGameObjectWithTag("breadText").GetComponent<Text>().enabled){
                breadHeld = true;

                GameObject.FindGameObjectWithTag("bread").GetComponent<MeshRenderer>().enabled = false;

                GameObject.FindGameObjectWithTag("breadHeld").GetComponent<MeshRenderer>().enabled = true;

                GameObject.FindGameObjectWithTag("breadText").GetComponent<Text>().enabled = false;
            }
            else if(GameObject.FindGameObjectWithTag("ingredientText").GetComponent<Text>().enabled){
                if(fishHeld){
                    fishHeld = false;
                    fishReturned = true;

                    GameObject[] heldFishObjects = GameObject.FindGameObjectsWithTag("fishHeld");
                    foreach(GameObject fish in heldFishObjects){
                        fish.GetComponent<MeshRenderer>().enabled = false;
                    }

                    GameObject[] returnedFishObjects = GameObject.FindGameObjectsWithTag("fishReturned");
                    foreach(GameObject fish in returnedFishObjects){
                        fish.GetComponent<MeshRenderer>().enabled = true;
                    }
                }
                else if(cheeseHeld){
                    cheeseHeld = false;
                    cheeseReturned = true;

                    GameObject.FindGameObjectWithTag("cheeseHeld").GetComponent<MeshRenderer>().enabled = false;

                    GameObject.FindGameObjectWithTag("cheeseReturned").GetComponent<MeshRenderer>().enabled = true;
                }
                else if(breadHeld){
                    breadHeld = false;
                    breadReturned = true;

                    GameObject.FindGameObjectWithTag("breadHeld").GetComponent<MeshRenderer>().enabled = false;

                    GameObject.FindGameObjectWithTag("breadReturned").GetComponent<MeshRenderer>().enabled = true;
                }

                GameObject.FindGameObjectWithTag("ingredientText").GetComponent<Text>().enabled = false;

                if(fishReturned && cheeseReturned && breadReturned){
                    // Cook minigame is complete - handle interactions with chef here
                }
            }
        }
    }

    
    void OnTriggerEnter(Collider other){
        // Change camera position when lil swabbie moves to a different area of the ship
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

            if(!cookGameStarted){
                GameObject.FindGameObjectWithTag("cookMinigameText").GetComponent<Text>().enabled = true;
            }
            else if(fishHeld || cheeseHeld || breadHeld){
                GameObject.FindGameObjectWithTag("ingredientText").GetComponent<Text>().enabled = true;
            }
        }
        // If cook minigame has started, show text when lil swabbie moves near an ingredient
        else if(cookGameStarted){
            if(other.CompareTag("fish") && !fishReturned && !cheeseHeld && !breadHeld){
                GameObject.FindGameObjectWithTag("fishText").GetComponent<Text>().enabled = true;
            }
            else if(other.CompareTag("cheese") && !cheeseReturned && !fishHeld && !breadHeld){
                GameObject.FindGameObjectWithTag("cheeseText").GetComponent<Text>().enabled = true;
            }
            else if(other.CompareTag("bread") && !breadReturned && !cheeseHeld && !fishHeld){
                GameObject.FindGameObjectWithTag("breadText").GetComponent<Text>().enabled = true;
            }
        }
    }

    // Hide text when lil swabbie leaves certain areas
    void OnTriggerExit(Collider other){
        if(other.CompareTag("galley")){
            GameObject.FindGameObjectWithTag("cookMinigameText").GetComponent<Text>().enabled = false;
        }
        else if(other.CompareTag("fish")){
            GameObject.FindGameObjectWithTag("fishText").GetComponent<Text>().enabled = false;
        }
        else if(other.CompareTag("cheese")){
            GameObject.FindGameObjectWithTag("cheeseText").GetComponent<Text>().enabled = false;
        }
        else if(other.CompareTag("bread")){
            GameObject.FindGameObjectWithTag("breadText").GetComponent<Text>().enabled = false;
        }
    }
}