using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    NavMeshAgent player;
    GameObject arrow;
    public Transform cameraPosition = null;
    bool gameover = false;
    bool cookGameStarted = false;
    bool fishHeld = false;
    bool fishReturned = false;
    bool cheeseHeld = false;
    bool cheeseReturned = false;
    bool breadHeld = false;
    bool breadReturned = false;
    bool playingShipGame = false;
    bool endScreen = false;
    int cannonballs = 0;
    bool attacking = false;
    Text pointCounter;
    int points = 0;
    System.Random rand = new System.Random();
    Text timer;
    TimeSpan t;
    double currCountdownValue;
    bool captainInteraction = false;
    bool captainInteracted = false;
    bool kasparInteraction = false;
    bool kasparInteracted = false;
    bool marioInteraction = false;
    bool marioInteracted = false;
    bool cookInteraction = false;
    bool cookInteracted = false;
    bool fishInteraction = false;
    bool cheeseInteraction = false;
    bool breadInteraction = false;
    bool returnInteraction = false;
    bool forcedInteraction = false;
    bool cameraMoving = false;
    string option = "";
    Text dialogue;
    public GameObject s0;
    public GameObject s1;
    public GameObject s2;
    public GameObject s3;
    public GameObject s4;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<NavMeshAgent>();
        arrow = GameObject.FindGameObjectWithTag("arrow");

        pointCounter = GameObject.FindGameObjectWithTag("points").GetComponent<Text>();
        GameObject.FindGameObjectWithTag("points").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("time").GetComponent<Text>().enabled = true;

        StartCoroutine(StartCountdown());
    }

    // Update is called once per frame
    void Update()
    {
        Animator animator = this.GetComponent<Animator>();
        pointCounter.text = points.ToString() + " pt(s)";

        // Set destination to wherever user clicks
        if(Input.GetKeyDown(KeyCode.Mouse0) && !playingShipGame){
            RaycastHit hit;
            NavMeshHit meshHit;
            
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100) && NavMesh.SamplePosition(hit.point, out meshHit, .5f, NavMesh.AllAreas)){
                player.destination = meshHit.position;
            }
        }

        // Stop moving when destination has been reached
        if(!gameover && !playingShipGame && player.remainingDistance < player.stoppingDistance){
            player.ResetPath();
        }

        // Enable or disable walking animation based on magnitude of velocity
        animator.SetBool("walking", player.velocity.magnitude > 0.2f);
        if (cameraMoving)
        {
            if (Camera.main.transform.position != cameraPosition.position && Camera.main.transform.rotation != cameraPosition.rotation)
            {
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraPosition.position, Time.deltaTime * 2);
                Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, cameraPosition.rotation, Time.deltaTime * 2);
            }
            else
            {
                cameraMoving = false;
            }
        }
    }

    void OnTriggerEnter(Collider other){
        // Change camera position when lil swabbie moves to a different area of the ship
        if(other.CompareTag("upperDeck")){
            moveCamera(-8.17f, 11.41f, -21.76f, 20, 0, 0);
            GameObject.FindGameObjectWithTag("leftDir").GetComponent<Text>().enabled = true;
            GameObject.FindGameObjectWithTag("rightDir").GetComponent<Text>().enabled = false;
        }
        else if(other.CompareTag("kaspar")){
            if(!kasparInteraction && !kasparInteracted){
                GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = true;
                dialogue = GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>();
                dialogue.text = "Interact";
                GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = true;
                kasparInteraction = true;
            }
            GameObject.FindGameObjectWithTag("leftDir").GetComponent<Text>().enabled = true;
            GameObject.FindGameObjectWithTag("rightDir").GetComponent<Text>().enabled = false;
        }
        else if(other.CompareTag("forced")){
            if(!forcedInteraction){
                GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = true;
                dialogue = GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>();
                dialogue.text = "Interact";
                GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = true;
                forcedInteraction = true;
            }
            GameObject.FindGameObjectWithTag("leftDir").GetComponent<Text>().enabled = false;
            GameObject.FindGameObjectWithTag("rightDir").GetComponent<Text>().enabled = true;
        }
        else if(other.CompareTag("mario")){
            if(!marioInteraction && !marioInteracted && !cookGameStarted){
                GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = true;
                dialogue = GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>();
                dialogue.text = "Interact";
                GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = true;
                marioInteraction = true;
            }
        }
        else if(other.CompareTag("captainsQuarters")){
            moveCamera(-12.51f, 5.42f, -14.33f,10, 20, 0);

            if(!captainInteraction && !captainInteracted){
                GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = true;
                dialogue = GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>();
                dialogue.text = "Interact";
                GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = true;
                captainInteraction = true;
            }
            GameObject.FindGameObjectWithTag("leftDir").GetComponent<Text>().enabled = true;
            GameObject.FindGameObjectWithTag("rightDir").GetComponent<Text>().enabled = false;
        }
        else if(other.CompareTag("mainDeck")){
            moveCamera(-24, 10.31f, -23.8f,20, 15, 0);
            GameObject.FindGameObjectWithTag("leftDir").GetComponent<Text>().enabled = true;
            GameObject.FindGameObjectWithTag("rightDir").GetComponent<Text>().enabled = true;
        }
        else if(!gameover && other.CompareTag("mainDeckFront")){
            moveCamera(-41.36f, 10.31f, -22.53f,20, 20, 0);
            GameObject.FindGameObjectWithTag("leftDir").GetComponent<Text>().enabled = false;
            GameObject.FindGameObjectWithTag("rightDir").GetComponent<Text>().enabled = true;
        }
        else if(other.CompareTag("lowerDeck")){
            moveCamera(-25.11f, 2.63f, -16.06f,10, 25, 0);
            GameObject.FindGameObjectWithTag("leftDir").GetComponent<Text>().enabled = true;
            GameObject.FindGameObjectWithTag("rightDir").GetComponent<Text>().enabled = true;
        }
        else if(other.CompareTag("lowerDeckFront")){
            moveCamera(-26.3f, 2.65f, -16.03f,10, -25, 0);
            GameObject.FindGameObjectWithTag("leftDir").GetComponent<Text>().enabled = false;
            GameObject.FindGameObjectWithTag("rightDir").GetComponent<Text>().enabled = true;
        }
        else if(other.CompareTag("galley")){
            moveCamera(-12, 2.5f, -15.71f,10, 20, 0);
            GameObject.FindGameObjectWithTag("leftDir").GetComponent<Text>().enabled = true;
            GameObject.FindGameObjectWithTag("rightDir").GetComponent<Text>().enabled = false;

            if(!cookInteraction && !cookInteracted){
                GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = true;
                dialogue = GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>();
                dialogue.text = "Interact";
                GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = true;
                cookInteraction = true;
            }
            else if(fishHeld || cheeseHeld || breadHeld){
                GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = true;
                dialogue = GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>();
                dialogue.text = "Return food";
                GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = true;
                returnInteraction = true;
            }
        }
        // If cook minigame has started, show text when lil swabbie moves near an ingredient
        else if(cookGameStarted && !(fishHeld || cheeseHeld || breadHeld)){
            if(other.CompareTag("fish") && !fishReturned && !cheeseHeld && !breadHeld){
                GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = true;
                dialogue = GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>();
                dialogue.text = "Pick Up Fish";
                GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = true;
                fishInteraction = true;
            }
            else if(other.CompareTag("cheese") && !cheeseReturned && !fishHeld && !breadHeld){
                GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = true;
                dialogue = GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>();
                dialogue.text = "Pick Up Cheese";
                GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = true;
                cheeseInteraction = true;
            }
            else if(other.CompareTag("bread") && !breadReturned && !cheeseHeld && !fishHeld){
                GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = true;
                dialogue = GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>();
                dialogue.text = "Pick Up Bread";
                GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = true;
                breadInteraction = true;
            }
        }
    }

    private void moveCamera(float v1, float v2, float v3, float v4, float v5, float v6)
    {
        cameraPosition.position = new Vector3(v1, v2, v3);
		cameraPosition.rotation = Quaternion.Euler(new Vector3(v4, v5, v6));
		cameraMoving = true;
    }

    // Hide text when lil swabbie leaves certain areas
    void OnTriggerExit(Collider other){
        if(other.CompareTag("galley")){
            GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = false;
            GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = false;
        }
        else if(other.CompareTag("fish")){
            GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = false;
            GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = false;
            fishInteraction = false;
        }
        else if(other.CompareTag("cheese")){
            GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = false;
            GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = false;
            cheeseInteraction = false;
        }
        else if(other.CompareTag("bread")){
            GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = false;
            GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = false;
            breadInteraction = false;
        }
        else if(other.CompareTag("captainsQuarters")){
            if(captainInteraction){
                GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = false;
                GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = false;
                captainInteraction = false;
            }
        }
        else if(other.CompareTag("kaspar")){
            if(kasparInteraction){
                GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = false;
                GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = false;
                kasparInteraction = false;
            }
        }
        else if(other.CompareTag("forced")){
            if(forcedInteraction){
                GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = false;
                GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = false;
                forcedInteraction = false;
            }
        }
        else if(other.CompareTag("mario")){
            if(marioInteraction){
                GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = false;
                GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = false;
                marioInteraction = false;
            }
        }
    }

    private IEnumerator loadWin()
    {
        gameover = true;
        //show animate out animation - uncomment these once merged with shipGame
        GameObject.FindWithTag("loading").GetComponent<Animator>().SetBool("animateOut", true);
        yield return new WaitForSeconds(1f);
        GameObject.FindWithTag("loading").GetComponent<Animator>().SetBool("animateOut", false);

        //this = lil-swabbie
        this.GetComponent<NavMeshAgent>().enabled = false;
        this.transform.SetPositionAndRotation(new Vector3(-38.2f, 3.83f, -12.78f), Quaternion.Euler(new Vector3(0, -115, 0)));
        this.GetComponent<Animator>().SetBool("win", true);
        GameObject pirate = GameObject.FindGameObjectWithTag("forced");
        pirate.transform.SetPositionAndRotation(new Vector3(-37.9f, 3.8f, -10.3f), Quaternion.Euler(new Vector3(0, 210.5f, 0)));
        pirate.GetComponent<Animator>().SetBool("win", true);
        pirate = GameObject.FindGameObjectWithTag("osilly");
        pirate.transform.SetPositionAndRotation(new Vector3(-36, 3.2f, -10.4f), Quaternion.Euler(new Vector3(0, -130.6f, 0)));
        pirate.GetComponent<Animator>().SetBool("win", true);
        pirate = GameObject.FindGameObjectWithTag("katarina");
        pirate.transform.SetPositionAndRotation(new Vector3(-36.74f, 5.57f, -9.8f), Quaternion.Euler(new Vector3(20.5f, 205, -2)));
        pirate.GetComponent<Animator>().SetBool("win", true);
        pirate = GameObject.FindGameObjectWithTag("kaspar");
        pirate.transform.SetPositionAndRotation(new Vector3(-33.3f, 3.5f, -10.8f), Quaternion.Euler(new Vector3(0, -123.8f, 0)));
        pirate.GetComponent<Animator>().SetBool("win", true);
        pirate = GameObject.FindGameObjectWithTag("chongus");
        pirate.transform.SetPositionAndRotation(new Vector3(-33.7f, 3.5f, -12.8f), Quaternion.Euler(new Vector3(0, -101, 0)));
        pirate.GetComponent<Animator>().SetBool("win", true);
        pirate = GameObject.FindGameObjectWithTag("mario");
        pirate.transform.SetPositionAndRotation(new Vector3(-35.3f, 3.5f, -14), Quaternion.Euler(new Vector3(0, 268.5f, 0)));
        pirate.GetComponent<Animator>().SetBool("win", true);

        Camera.main.transform.SetPositionAndRotation(new Vector3(-40.8f, 6.6f, -13.9f), Quaternion.Euler(new Vector3(17.1f, 67, 0)));
        
        yield return new WaitForSeconds(8f);
        endScreen = false;
    }

    private IEnumerator loadLoose()
    {
        gameover = true;
        //show animate out animation - uncomment these once merged with shipGame
        GameObject.FindWithTag("loading").GetComponent<Animator>().SetBool("animateOut", true);
        yield return new WaitForSeconds(1f);
        GameObject.FindWithTag("loading").GetComponent<Animator>().SetBool("animateOut", false);

        //this = lil-swabbie
        this.GetComponent<NavMeshAgent>().enabled = false;
        this.transform.SetPositionAndRotation(new Vector3(-49.6f, 4.9f, -12), Quaternion.Euler(new Vector3(0, -168.4f, 0)));
        this.GetComponent<Animator>().SetBool("lose", true);
        GameObject pirate = GameObject.FindGameObjectWithTag("forced");
        pirate.transform.SetPositionAndRotation(new Vector3(-36.1f, 3.8f, -9.9f), Quaternion.Euler(new Vector3(0, 210.5f, 0)));
        pirate = GameObject.FindGameObjectWithTag("osilly");
        pirate.transform.SetPositionAndRotation(new Vector3(-46.6f, 5, -12), Quaternion.Euler(new Vector3(0, -101.6f, 0)));
        //set some sort of animation
        pirate = GameObject.FindGameObjectWithTag("katarina");
        pirate.transform.SetPositionAndRotation(new Vector3(-47.2f, 7.2f, -11.3f), Quaternion.Euler(new Vector3(-5.4f, 233, 0.7f)));
        pirate = GameObject.FindGameObjectWithTag("kaspar");
        pirate.transform.SetPositionAndRotation(new Vector3(-32.2f, 2.8f, -13.5f), Quaternion.Euler(new Vector3(0, -102.5f, 0)));
        pirate = GameObject.FindGameObjectWithTag("chongus");
        pirate.transform.SetPositionAndRotation(new Vector3(-43.7f, 4.6f, -12.4f), Quaternion.Euler(new Vector3(0, -104, 0)));
        pirate.GetComponent<Animator>().SetBool("lose", true);
        //set his "no" animation?
        pirate = GameObject.FindGameObjectWithTag("mario");
        pirate.transform.SetPositionAndRotation(new Vector3(-40.4f, 4.2f, -12.8f), Quaternion.Euler(new Vector3(0, 268.5f, 0)));

        Camera.main.transform.SetPositionAndRotation(new Vector3(-52.3f, 7.9f, -13.9f), Quaternion.Euler(new Vector3(6.7f, 79, 0)));

        yield return new WaitForSeconds(8f);
        endScreen = false;
    }

    private IEnumerator loadCookGame()
    {
        cookGameStarted = true;
        
        GameObject[] fishObjects = GameObject.FindGameObjectsWithTag("fish");
        foreach(GameObject fish in fishObjects){
            fish.GetComponent<MeshRenderer>().enabled = true;
        }

        GameObject.FindGameObjectWithTag("cheese").GetComponent<MeshRenderer>().enabled = true;
        GameObject.FindGameObjectWithTag("bread").GetComponent<MeshRenderer>().enabled = true;

        while(cookGameStarted){
            if(fishReturned && cheeseReturned && breadReturned){
                cookGameStarted = false;
            }
            yield return null;
        }
        
        yield return null;
    }

    private IEnumerator loadShipGame()
    {
        //show animate out animation
        GameObject.FindWithTag("loading").GetComponent<Animator>().SetBool("animateOut", true);
        yield return new WaitForSeconds(1.0f);
        GameObject.FindWithTag("loading").GetComponent<Animator>().SetBool("animateOut", false);
        this.GetComponent<NavMeshAgent>().enabled = false;
        this.GetComponent<Transform>().position = new Vector3(-20.4f, 30.3f, -12f);
        this.GetComponent<Transform>().rotation = Quaternion.Euler(new Vector3(0, -90, 0));
        Camera.main.transform.SetPositionAndRotation(new Vector3(-17.2f, 34f, -12f), Quaternion.Euler(new Vector3(26.5f, -90, 0)));
        
        while (playingShipGame && cannonballs >= 0)
        {
            if (Input.GetKeyDown(KeyCode.Return) && !attacking)
            {
                StartCoroutine(fireCannon());
            }
            if (Input.GetKey(KeyCode.RightArrow) && !attacking)
            {
                //move camera and crosshair
                Camera.main.transform.RotateAround(this.transform.position, Vector3.up, 0.5f);
                arrow.transform.Rotate(0,0,-0.5f);
            }
            if (Input.GetKey(KeyCode.LeftArrow) && !attacking)
            {
                //move camera and crosshair
                Camera.main.transform.RotateAround(this.transform.position, -Vector3.up, 0.5f);
                arrow.transform.Rotate(0, 0, 0.5f);

            }
            if (Input.GetKey(KeyCode.UpArrow) && !attacking)
            {
                //move crosshair
                if (arrow.transform.localScale.x < 200)
                {
                    arrow.GetComponent<Transform>().localScale += new Vector3(0.8f, 0, 0);
                    GameObject.FindGameObjectWithTag("FxTemporaire").GetComponent<Transform>().localScale += new Vector3(-0.000686f, 0, 0);
                    GameObject.FindGameObjectWithTag("splash").GetComponent<Transform>().localScale += new Vector3(-0.000686f, 0, 0);
                }
            }
            if (Input.GetKey(KeyCode.DownArrow) && !attacking)
            {
                //move crosshair
                if(arrow.transform.localScale.x > 25)
                {
                    arrow.transform.localScale += new Vector3(-0.8f, 0, 0);
                    GameObject.FindGameObjectWithTag("FxTemporaire").GetComponent<Transform>().localScale += new Vector3(0.000686f, 0, 0);
                    GameObject.FindGameObjectWithTag("splash").GetComponent<Transform>().localScale += new Vector3(0.000686f, 0, 0);
                }
            }
            if (cannonballs == 0 && !attacking)
            {
                StartCoroutine(loadMainGame());
                GetComponent<Animator>().SetBool("playingShipGame", false);
                arrow.SetActive(false);
            }
            yield return null;
        }
        yield return null;
    }

    private IEnumerator loadMainGame()
    {
        //show animate out animation
        GameObject.FindWithTag("loading").GetComponent<Animator>().SetBool("animateOut", true);
        yield return new WaitForSeconds(1f);
        playingShipGame = false;
        GameObject.FindGameObjectWithTag("mario").GetComponent<Animator>().SetBool("cookwin", true);
        GameObject.FindWithTag("loading").GetComponent<Animator>().SetBool("animateOut", false);
        this.GetComponent<Transform>().position = new Vector3(-29.52219f, 3.624516f, -14.07293f);
        this.GetComponent<Transform>().rotation = Quaternion.Euler(new Vector3(0, -238.542f, 0));
        this.GetComponent<NavMeshAgent>().enabled = true;
        Camera.main.transform.SetPositionAndRotation(new Vector3(-24, 10.31f, -23.8f), Quaternion.Euler(new Vector3(20, 15, 0)));
    }

    private IEnumerator fireCannon()
    {
        GameObject hit = GameObject.FindGameObjectWithTag("FxTemporaire");

        attacking = true;
        GetComponent<Animator>().SetBool("attack", true);
        yield return new WaitForSeconds(3f);
        GetComponent<Animator>().SetBool("attack", false);

        if (s0.GetComponent<Animator>().GetBool("targeted"))
        {
            s0.GetComponent<Animator>().SetBool("shot", true);
            points += 1;
            hit.GetComponent<ParticleSystem>().Play();
            //earn points here
        }
        else if (s1.GetComponent<Animator>().GetBool("targeted"))
        {
            s1.GetComponent<Animator>().SetBool("shot", true);
            points += 1;
            hit.GetComponent<ParticleSystem>().Play();
            //earn points here
        }
        else if (s2.GetComponent<Animator>().GetBool("targeted"))
        {
            s2.GetComponent<Animator>().SetBool("shot", true);
            points += 1;
            hit.GetComponent<ParticleSystem>().Play();
            //earn points here
        }
        else if (s3.GetComponent<Animator>().GetBool("targeted"))
        {
            s3.GetComponent<Animator>().SetBool("shot", true);
            points += 1;
            hit.GetComponent<ParticleSystem>().Play();
            //earn points here
        }
        else if (s4.GetComponent<Animator>().GetBool("targeted"))
        {
            s4.GetComponent<Animator>().SetBool("shot", true);
            points += 1;
            hit.GetComponent<ParticleSystem>().Play();
            //earn points here
        }
        else { GameObject.FindGameObjectWithTag("splash").GetComponent<ParticleSystem>().Play(); }
        cannonballs--;

        yield return new WaitForSeconds(2f);
        attacking = false;
    }

    public void interact(){
        if(fishInteraction){
            fishHeld = true;

            GameObject[] fishObjects = GameObject.FindGameObjectsWithTag("fish");
            foreach(GameObject fish in fishObjects){
                fish.GetComponent<MeshRenderer>().enabled = false;
            }

            GameObject[] heldFishObjects = GameObject.FindGameObjectsWithTag("fishHeld");
            foreach(GameObject fish in heldFishObjects){
                fish.GetComponent<MeshRenderer>().enabled = true;
            }

            GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = false;
            GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = false;
            fishInteraction = false;
        }
        else if(cheeseInteraction){
            cheeseHeld = true;

            GameObject.FindGameObjectWithTag("cheese").GetComponent<MeshRenderer>().enabled = false;
            GameObject.FindGameObjectWithTag("cheeseHeld").GetComponent<MeshRenderer>().enabled = true;
            GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = false;
            GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = false;
            cheeseInteraction = false;
        }
        else if(breadInteraction){
            breadHeld = true;

            GameObject.FindGameObjectWithTag("bread").GetComponent<MeshRenderer>().enabled = false;
            GameObject.FindGameObjectWithTag("breadHeld").GetComponent<MeshRenderer>().enabled = true;
            GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = false;
            GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = false;
            breadInteraction = false;
        }
        else if(returnInteraction){
            points += 2;
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

                GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = false;
                GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = false;
            }
            else if(cheeseHeld){
                cheeseHeld = false;
                cheeseReturned = true;

                GameObject.FindGameObjectWithTag("cheeseHeld").GetComponent<MeshRenderer>().enabled = false;
                GameObject.FindGameObjectWithTag("cheeseReturned").GetComponent<MeshRenderer>().enabled = true;

                GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = false;
                GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = false;
            }
            else if(breadHeld){
                breadHeld = false;
                breadReturned = true;

                GameObject.FindGameObjectWithTag("breadHeld").GetComponent<MeshRenderer>().enabled = false;
                GameObject.FindGameObjectWithTag("breadReturned").GetComponent<MeshRenderer>().enabled = true;

                GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = false;
                GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = false;
            }
            returnInteraction = false;
        }
        else if(captainInteraction){
            StartCoroutine(captainConvo());
        }
        else if(kasparInteraction){
            StartCoroutine(kasparConvo());
        }
        else if(forcedInteraction){
            StartCoroutine(forcedConvo());
        }
        else if(marioInteraction){
            StartCoroutine(marioConvo());
        }
        else if(cookInteraction){
            StartCoroutine(cookConvo());
        }
    }

    private IEnumerator captainConvo(){
        GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = false;
        GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = false;
        
        GameObject.FindGameObjectWithTag("captainPane").GetComponent<Image>().enabled = true;
        GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>().enabled = true;

        dialogue = GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>();

        dialogue.text = "Captain: Welcome to my ship, The Intrepid!\n"
                        + "[A] - Glad to be aboard!\n"
                        + "[B] - ...\n"
                        + "[C] - Whatever.";

        Input.ResetInputAxes();
        yield return StartCoroutine(WaitForKeyDown(new KeyCode[] { KeyCode.A, KeyCode.B, KeyCode.C }));

        switch (option) {
            case ("A"):
                dialogue.text = "Captain: We need enthusiasm like that!\n";
                points += 2;
                break;
            case ("B"):
                dialogue.text = "Captain: ...\n";
                break;
            case ("C"):
                dialogue.text = "Captain: It is an honor to serve on my ship!\n";
                points -= 2;
                break;
        }
        dialogue.text += "You will be assisting the crew members with some tasks.\n"
                        + "[A] - Will do!\n"
                        + "[B] - I'll think about it.";
        option = "";

        Input.ResetInputAxes();
        yield return StartCoroutine(WaitForKeyDown(new KeyCode[] { KeyCode.A, KeyCode.B }));

        switch (option) {
            case ("A"):
                dialogue.text = "Captain: Good swabbie.\n";
                points += 2;
                break;
            case ("B"):
                dialogue.text = "Captain: That’s an order, swabbie...\n";
                points -= 2;
                break;
        }
        dialogue.text += "Now go! Help out your superiors with tasks.\n"
                        + "[A] - (Exit)";

        Input.ResetInputAxes();
        yield return StartCoroutine(WaitForKeyDown(new KeyCode[] { KeyCode.A }));

        GameObject.FindGameObjectWithTag("captainPane").GetComponent<Image>().enabled = false;
        GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>().enabled = false;
        captainInteracted = true;
    }

    private IEnumerator kasparConvo(){
        GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = false;
        GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = false;
        
        GameObject.FindGameObjectWithTag("captainPane").GetComponent<Image>().enabled = true;
        GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>().enabled = true;

        dialogue = GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>();

        dialogue.text = "Kaspar: Hello swabbie...\n"
                        + "[A] - Hi!\n"
                        + "[B] - Who are you?";

        Input.ResetInputAxes();
        yield return StartCoroutine(WaitForKeyDown(new KeyCode[] { KeyCode.A, KeyCode.B }));

        switch (option) {
            case ("A"):
                dialogue.text = "Kaspar: Welcome aboard.\n";
                points += 2;
                break;
            case ("B"):
                dialogue.text = "Kaspar: Don't you know? I'm first mate, Kaspar.\n";
                points -= 2;
                break;
        }
        dialogue.text += "You should probably go to meet the captain.\n"
                        + "[A] - The captain?\n"
                        + "[B] - Why?";
        option = "";

        Input.ResetInputAxes();
        yield return StartCoroutine(WaitForKeyDown(new KeyCode[] { KeyCode.A, KeyCode.B }));

        switch (option) {
            case ("A"):
                dialogue.text = "Kaspar: Yes... of this fine vessel, The Intrepid\n";
                points += 1;
                break;
            case ("B"):
                dialogue.text = "Kaspar: Careful. With an attitude like that, you won't make many friends...\n";
                points -= 1;
                break;
        }
        dialogue.text += "Captain is in his quarters, below this upper deck. Go meet him. Bye swabbie.\n"
                        + "[A] - (Exit)";

        Input.ResetInputAxes();
        yield return StartCoroutine(WaitForKeyDown(new KeyCode[] { KeyCode.A }));

        GameObject.FindGameObjectWithTag("captainPane").GetComponent<Image>().enabled = false;
        GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>().enabled = false;
        kasparInteracted = true;
    }

    private IEnumerator forcedConvo(){
        GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = false;
        GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = false;
        
        GameObject.FindGameObjectWithTag("captainPane").GetComponent<Image>().enabled = true;
        GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>().enabled = true;

        dialogue = GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>();

        dialogue.text = "???: *Unintelligible noises*\n"
                        + "[A] - Uhhhhh...\n"
                        + "[B] - *Unintelligible noises*";

        Input.ResetInputAxes();
        yield return StartCoroutine(WaitForKeyDown(new KeyCode[] { KeyCode.A, KeyCode.B }));

        switch (option) {
            case ("A"):
                dialogue.text = "???: *Unintelligible noises*\n";
                break;
            case ("B"):
                dialogue.text = "???: *Unintelligible noises*\n";
                if(rand.Next(10) % 2 == 0){
                    points += 3;
                }
                else{
                    points -= 2;
                }
                break;
        }
        dialogue.text += "???: *Unintelligible noises*\n"
                        + "[A] - (Exit)";

        Input.ResetInputAxes();
        yield return StartCoroutine(WaitForKeyDown(new KeyCode[] { KeyCode.A }));

        GameObject.FindGameObjectWithTag("captainPane").GetComponent<Image>().enabled = false;
        GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>().enabled = false;
    }

    private IEnumerator marioConvo(){
        GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = false;
        GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = false;
        
        GameObject.FindGameObjectWithTag("captainPane").GetComponent<Image>().enabled = true;
        GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>().enabled = true;

        dialogue = GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>();

        dialogue.text = "Mario: Hey there. You must be the new swabbie.\n"
                        + "[A] - That's me!\n"
                        + "[B] - ...";

        Input.ResetInputAxes();
        yield return StartCoroutine(WaitForKeyDown(new KeyCode[] { KeyCode.A, KeyCode.B }));

        switch (option) {
            case ("A"):
                dialogue.text = "Mario: Good.\n";
                points += 2;
                break;
            case ("B"):
                dialogue.text = "Mario: I'm speaking to you...\n";
                points -= 2;
                break;
        }
        dialogue.text += "Anyways, it's about time you learn to shoot the cannons, huh?\nUse the arrow keys to aim and press enter to tell me to fire. Be warned, there will be a delay.\n"
                        + "[A] - (Start Minigame)";
        option = "";

        Input.ResetInputAxes();
        yield return StartCoroutine(WaitForKeyDown(new KeyCode[] { KeyCode.A }));

        if (!playingShipGame)
        {
            playingShipGame = true;
            cannonballs = 5;
            player.ResetPath();
            StartCoroutine(loadShipGame());
            GetComponent<Animator>().SetBool("playingShipGame", true);

            GameObject.FindGameObjectWithTag("captainPane").GetComponent<Image>().enabled = false;
            GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>().enabled = false;
            //playingShipGame = false;
        }

        while(playingShipGame){
            yield return new WaitForSeconds(0.2f);
        }
        
        dialogue.text = "Mario: Be on your way and talk to the Cook below deck if you're hungry.\n"
                        + "[A] - (Exit)";

        GameObject.FindGameObjectWithTag("captainPane").GetComponent<Image>().enabled = true;
        GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>().enabled = true;

        Input.ResetInputAxes();
        yield return StartCoroutine(WaitForKeyDown(new KeyCode[] { KeyCode.A }));

        GameObject.FindGameObjectWithTag("captainPane").GetComponent<Image>().enabled = false;
        GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>().enabled = false;
        marioInteracted = true;
        GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = false;
        GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = false;
    }

    private IEnumerator cookConvo(){
        GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = false;
        GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = false;
        
        GameObject.FindGameObjectWithTag("captainPane").GetComponent<Image>().enabled = true;
        GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>().enabled = true;

        dialogue = GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>();

        dialogue.text = "Cook: Lil' swabbie, welcome aboard! I'm the cook of this fine vessel.\n"
                        + "[A] - I'm hungry!\n"
                        + "[B] - ...\n";

        Input.ResetInputAxes();
        yield return StartCoroutine(WaitForKeyDown(new KeyCode[] { KeyCode.A, KeyCode.B, KeyCode.C }));

        switch (option) {
            case ("A"):
                dialogue.text = "Cook: Not meal-time yet.\n";
                points -= 2;
                break;
            case ("B"):
                dialogue.text = "Cook: ...\n";
                points += 2;
                break;
        }
        dialogue.text += "Maybe you can help me out with some things. I need some ingredients. Go find the Fish, Cheese, and Bread and bring them back to me.\n"
                        + "[A] - (Start Minigame)";
        option = "";

        Input.ResetInputAxes();
        yield return StartCoroutine(WaitForKeyDown(new KeyCode[] { KeyCode.A }));

        if(!cookGameStarted){
            cookGameStarted = true;

            StartCoroutine(loadCookGame());

            GameObject.FindGameObjectWithTag("captainPane").GetComponent<Image>().enabled = false;
            GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>().enabled = false;
        }

        while(cookGameStarted){
            yield return new WaitForSeconds(0.2f);
        }

        dialogue.text = "Good work! Off you go...\n"
                        + "[A] - (Exit)";

        GameObject.FindGameObjectWithTag("captainPane").GetComponent<Image>().enabled = true;
        GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>().enabled = true;

        Input.ResetInputAxes();
        yield return StartCoroutine(WaitForKeyDown(new KeyCode[] { KeyCode.A }));

        GameObject.FindGameObjectWithTag("captainPane").GetComponent<Image>().enabled = false;
        GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>().enabled = false;
        cookInteracted = true;
    }

    IEnumerator WaitForKeyDown(KeyCode[] codes) {
        bool pressed = false;
        while (!pressed) {
            foreach (KeyCode k in codes) {
                if (Input.GetKey(k)) {
                    pressed = true;
                    SetOptionTo(k);
                    break;
                }
            }
            yield return null;
        }
    }

    private void SetOptionTo(KeyCode keyCode) {
        switch (keyCode) {
            case (KeyCode.A):
                option = "A";
                break;
            case (KeyCode.B):
                option = "B";
                break;
            case (KeyCode.C):
                option = "C";
                break;
        }
    }

    public IEnumerator StartCountdown(double countdownValue = 17)
    {
        string padZero = "";
        timer = GameObject.FindGameObjectWithTag("time").GetComponent<Text>();
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            t = TimeSpan.FromSeconds(currCountdownValue);
            if((currCountdownValue % 60) < 10){
                padZero = "0";
            } else {
                padZero = "";
            }
            timer.text = (currCountdownValue / 60).ToString().Substring(0, 1) + ":" + padZero + (currCountdownValue % 60);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        currCountdownValue--;
        GameObject.FindGameObjectWithTag("captainInteraction").GetComponent<Image>().enabled = false;
        GameObject.FindGameObjectWithTag("captainInteractionText").GetComponent<Text>().enabled = false;

        dialogue = GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>();

        GameObject.FindGameObjectWithTag("captainPane").GetComponent<Image>().enabled = true;
        GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>().enabled = true;

        dialogue.text = "Time's up! Let's see how you did with the crew...\n";

        yield return new WaitForSeconds(2f);

        dialogue.text += "You got " + points.ToString() + " point(s)...\n";

        GameObject.FindGameObjectWithTag("points").GetComponent<Text>().enabled = false;
        GameObject.FindGameObjectWithTag("time").GetComponent<Text>().enabled = false;
        GameObject.FindGameObjectWithTag("leftDir").GetComponent<Text>().enabled = false;
        GameObject.FindGameObjectWithTag("rightDir").GetComponent<Text>().enabled = false;

        yield return new WaitForSeconds(2f);

        endScreen = true;

        if(points > 1){
            dialogue.text += "You win!";

            yield return new WaitForSeconds(2f);

            GameObject.FindGameObjectWithTag("captainPane").GetComponent<Image>().enabled = false;
            GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>().enabled = false;
            StartCoroutine(loadWin());
        } else {
            dialogue.text += "That's no good!";

            yield return new WaitForSeconds(2f);

            GameObject.FindGameObjectWithTag("captainPane").GetComponent<Image>().enabled = false;
            GameObject.FindGameObjectWithTag("captainPaneText").GetComponent<Text>().enabled = false;
            StartCoroutine(loadLoose());
        }

        while(endScreen){
            yield return new WaitForSeconds(0.2f);
        }

        if(gameover)
        {
            SceneManager.LoadScene(0);
        }
    }
}
