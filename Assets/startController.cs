using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startController : MonoBehaviour
{
    // Show controls when controls button is clicked and return to main menu when back button is clicked
    public void switchMenus(bool isMainMenu){
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

    // Switch to gameplay scene when start button is clicked
    public void startClicked(){
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex +1) % SceneManager.sceneCountInBuildSettings);
    }
}