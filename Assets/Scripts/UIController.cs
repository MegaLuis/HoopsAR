using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.iOS
{
    public class UIController : MonoBehaviour
    {

        public GameObject startMenu;
        public GameObject mainMenu;
        public GameObject scoreUI;
        public GameObject placeBallCanvas;
        public GameObject arCameraManager;
        public GameObject planeGenerator;
        public GameObject basket;
        public GameObject ball;

        public GameObject freePlayController;
        public GameObject timeTrialController;

        bool freePlay;
        bool timeTrial;

        // Use this for initialization
        void Start()
        {
            startMenu.SetActive(true);
            mainMenu.SetActive(false);
            scoreUI.SetActive(false);
            placeBallCanvas.SetActive(false);
            arCameraManager.SetActive(false);
            planeGenerator.SetActive(false);
            basket.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void GoToMainMenu()
        {
            startMenu.SetActive(false);
            mainMenu.SetActive(true);
        }

        public void StartFreePlay()
        {
            freePlay = true;
            mainMenu.SetActive(false);
            placeBallCanvas.SetActive(true);
            arCameraManager.SetActive(true);
            planeGenerator.SetActive(true);
            basket.SetActive(true);
        }

        public void StartTimeAttack()
        {
            timeTrial = true;
            mainMenu.SetActive(false);
            placeBallCanvas.SetActive(true);
            arCameraManager.SetActive(true);
            planeGenerator.SetActive(true);
            basket.SetActive(true);
        }

        public void PlaceBasket()
        {
            basket.GetComponent<PlaceHoop>().basketIsPlaced = true;

            GameObject[] planeArray = GameObject.FindGameObjectsWithTag("plane");
            foreach(GameObject obj in planeArray){
                Destroy(obj);
            }

            placeBallCanvas.SetActive(false);
            if (freePlay)
            {
                freePlayController.SetActive(true);
            }
            else if (timeTrial)
            {
                timeTrialController.SetActive(true);
            }

            ball.SetActive(true);
        }
    }
}