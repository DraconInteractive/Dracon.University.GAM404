using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Final
{
    //Made a main menu because thoroughness. 
    public class MenuController : MonoBehaviour
    {
        //UI elements, and a variable so the player can choose control type. 
        public Button auto, manual, exit;

        public static StatisticsController.ControlType cType;

        
        // Use this for initialization
        void Start()
        {
            //Make this persistent so it can pass on its control type. 
            DontDestroyOnLoad(this);
            //Setup button listeners to the appropriate functions. 

            auto.onClick.AddListener(() => Auto());
            manual.onClick.AddListener(() => Manual());
            exit.onClick.AddListener(() => Exit());

        }

        // Update is called once per frame
        void Update()
        {

        }
        //Set control type to auto then start game. Int level load is lazy, but it works!
        public static void Auto ()
        {
            cType = StatisticsController.ControlType.Auto;
            SceneManager.LoadScene(1);
        }
        //that ^ but manual
        public static void Manual ()
        {
            cType = StatisticsController.ControlType.Manual;
            SceneManager.LoadScene(1);
        }
        //Quit the application. reminder to self, this doesnt work in editor, but if you put the editor bit in it wont compile, unless you use the directive which is just too much work atm. 
        public void Exit ()
        {
            Application.Quit();
        }
    }
}

