using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    NavMeshAgent player;
    bool gameover = false;
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
        //fix this when merged
        if (!gameover){
        if(player.remainingDistance < player.stoppingDistance){
                player.ResetPath();
        }
        }

        // Enable or disable walking animation based on magnitude of velocity
        animator.SetBool("walking", player.velocity.magnitude > 0.2f);

        //good ending - if enough points
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(loadWin());
        }
        //bad ending - if lacking points
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(loadLoose());
        }
        //do we need a normal ending?
        if(gameover && Input.GetKeyDown(KeyCode.Return))
        {
            //go to title screen
        }

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

    private IEnumerator loadWin()
    {
        //show animate out animation - uncomment these once merged with shipGame
        //GameObject.FindWithTag("loading").GetComponent<Animator>().SetBool("animateOut", true);
        yield return new WaitForSeconds(1f);
        //GameObject.FindWithTag("loading").GetComponent<Animator>().SetBool("animateOut", false);
        
        //this = lil-swabbie
        this.GetComponent<NavMeshAgent>().enabled = false;
        this.transform.SetPositionAndRotation(new Vector3(-38.2f,3.83f,-12.78f), Quaternion.Euler(new Vector3(0, -115, 0)));
        this.GetComponent<Animator>().SetBool("win",true);
        GameObject pirate = GameObject.FindGameObjectWithTag("forced");
        pirate.transform.SetPositionAndRotation(new Vector3(-37.9f ,3.8f,-10.3f), Quaternion.Euler(new Vector3(0, 210.5f, 0)));
        pirate.GetComponent<Animator>().SetBool("win", true);
        pirate = GameObject.FindGameObjectWithTag("osilly");
        pirate.transform.SetPositionAndRotation(new Vector3(-36,3.2f,-10.4f), Quaternion.Euler(new Vector3(0,-130.6f,0)));
        pirate.GetComponent<Animator>().SetBool("win", true);
        pirate = GameObject.FindGameObjectWithTag("katarina");
        pirate.transform.SetPositionAndRotation(new Vector3(-36.74f,5.57f,-9.8f), Quaternion.Euler(new Vector3(20.5f,205,-2)));
        pirate.GetComponent<Animator>().SetBool("win", true);
        pirate = GameObject.FindGameObjectWithTag("kaspar");
        pirate.transform.SetPositionAndRotation(new Vector3(-33.3f,3.5f,-10.8f), Quaternion.Euler(new Vector3(0, -123.8f, 0)));
        pirate.GetComponent<Animator>().SetBool("win", true);
        pirate = GameObject.FindGameObjectWithTag("chongus");
        pirate.transform.SetPositionAndRotation(new Vector3(-33.7f,3.5f,-12.8f), Quaternion.Euler(new Vector3(0, -101, 0)));
        pirate.GetComponent<Animator>().SetBool("win", true);
        pirate = GameObject.FindGameObjectWithTag("mario");
        pirate.transform.SetPositionAndRotation(new Vector3(-35.3f,3.5f,-14), Quaternion.Euler(new Vector3(0, 268.5f, 0)));
        pirate.GetComponent<Animator>().SetBool("win", true);

        Camera.main.transform.SetPositionAndRotation(new Vector3(-40.8f, 6.6f, -13.9f), Quaternion.Euler(new Vector3(17.1f, 67, 0)));
        gameover = true;
    }

    private IEnumerator loadLoose()
    {
        //show animate out animation - uncomment these once merged with shipGame
        //GameObject.FindWithTag("loading").GetComponent<Animator>().SetBool("animateOut", true);
        yield return new WaitForSeconds(1f);
        //GameObject.FindWithTag("loading").GetComponent<Animator>().SetBool("animateOut", false);

        //this = lil-swabbie
        this.GetComponent<NavMeshAgent>().enabled = false;
        this.transform.SetPositionAndRotation(new Vector3(-49.6f,4.9f,-12), Quaternion.Euler(new Vector3(0, -168.4f, 0)));
        this.GetComponent<Animator>().SetBool("lose", true);
        GameObject pirate = GameObject.FindGameObjectWithTag("forced");
        pirate.transform.SetPositionAndRotation(new Vector3(-36.1f,3.8f,-9.9f), Quaternion.Euler(new Vector3(0, 210.5f, 0)));
        pirate = GameObject.FindGameObjectWithTag("osilly");
        pirate.transform.SetPositionAndRotation(new Vector3(-46.6f,5,-12), Quaternion.Euler(new Vector3(0, -101.6f, 0)));
        //set some sort of animation
        pirate = GameObject.FindGameObjectWithTag("katarina");
        pirate.transform.SetPositionAndRotation(new Vector3(-47.2f,7.2f,-11.3f), Quaternion.Euler(new Vector3(-5.4f, 233, 0.7f)));
        pirate = GameObject.FindGameObjectWithTag("kaspar");
        pirate.transform.SetPositionAndRotation(new Vector3(-32.2f, 2.8f, -13.5f), Quaternion.Euler(new Vector3(0, -102.5f, 0)));
        pirate = GameObject.FindGameObjectWithTag("chongus");
        pirate.transform.SetPositionAndRotation(new Vector3(-43.7f,4.6f,-12.4f), Quaternion.Euler(new Vector3(0, -104, 0)));
        //set his "no" animation?
        pirate = GameObject.FindGameObjectWithTag("mario");
        pirate.transform.SetPositionAndRotation(new Vector3(-40.4f,4.2f,-12.8f), Quaternion.Euler(new Vector3(0, 268.5f, 0)));

        Camera.main.transform.SetPositionAndRotation(new Vector3(-52.3f,7.9f,-13.9f), Quaternion.Euler(new Vector3(6.7f, 79, 0)));
        gameover = true;
    }
}

