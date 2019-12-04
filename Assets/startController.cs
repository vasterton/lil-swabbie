using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startController : MonoBehaviour
{
    bool mainMenu = true;
    int time = 0;

    void Update(){
        // Only enable/disable attract mode if the main menu is on screen
        if(mainMenu){
            // If the mouse moves or a key is pressed, disable attract mode
            if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 || Input.anyKeyDown){
                time = 0;
                Camera.main.transform.SetPositionAndRotation(new Vector3(-24.96f, 15.53f, -43.58f), Quaternion.Euler(new Vector3(0, 0, 0)));
                attractMode(false);
            }
            // If the mouse and keyboard are idle for about 15 seconds, enable attract mode
            else{
                if(time > 2000){
                    Camera.main.transform.RotateAround(new Vector3(-24.96f, 0, -12.5f), Vector3.up, .05f);
                    attractMode(true);
                }
                else{
                    time++;
                }
            }
        }
    }

    // Show controls when controls button is clicked and return to main menu when back button is clicked
    public void switchMenus(bool isMainMenu){
        mainMenu = isMainMenu;

        GameObject.FindGameObjectWithTag("title").GetComponent<Image>().enabled = isMainMenu;
        GameObject.FindGameObjectWithTag("titleText").GetComponent<Text>().enabled = isMainMenu;

        GameObject.FindGameObjectWithTag("startButton").GetComponent<Image>().enabled = isMainMenu;
        GameObject.FindGameObjectWithTag("startButtonText").GetComponent<Text>().enabled = isMainMenu;

        GameObject.FindGameObjectWithTag("controlsButton").GetComponent<Image>().enabled = isMainMenu;
        GameObject.FindGameObjectWithTag("controlsButtonText").GetComponent<Text>().enabled = isMainMenu;

        GameObject.FindGameObjectWithTag("controls").GetComponent<Image>().enabled = !isMainMenu;
        GameObject.FindGameObjectWithTag("controlsText").GetComponent<Text>().enabled = !isMainMenu;

        GameObject.FindGameObjectWithTag("backButton").GetComponent<Image>().enabled = !isMainMenu;
        GameObject.FindGameObjectWithTag("backButtonText").GetComponent<Text>().enabled = !isMainMenu;
    }

    // Hide or show main menu when attract mode is enabled or disabled
    public void attractMode(bool attractMode){
        GameObject.FindGameObjectWithTag("title").GetComponent<Image>().enabled = !attractMode;
        GameObject.FindGameObjectWithTag("titleText").GetComponent<Text>().enabled = !attractMode;

        GameObject.FindGameObjectWithTag("startButton").GetComponent<Image>().enabled = !attractMode;
        GameObject.FindGameObjectWithTag("startButtonText").GetComponent<Text>().enabled = !attractMode;

        GameObject.FindGameObjectWithTag("controlsButton").GetComponent<Image>().enabled = !attractMode;
        GameObject.FindGameObjectWithTag("controlsButtonText").GetComponent<Text>().enabled = !attractMode;
    }

    // Switch to gameplay scene when start button is clicked
    public void startClicked(){
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex +1) % SceneManager.sceneCountInBuildSettings);
    }
}