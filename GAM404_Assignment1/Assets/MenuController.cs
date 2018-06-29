using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Final
{
    public class MenuController : MonoBehaviour
    {
        public Button auto, manual, exit;

        public static StatisticsController.ControlType cType;

        
        // Use this for initialization
        void Start()
        {
            DontDestroyOnLoad(this);

            auto.onClick.AddListener(() => Auto());
            manual.onClick.AddListener(() => Manual());
            exit.onClick.AddListener(() => Exit());

        }

        // Update is called once per frame
        void Update()
        {

        }

        public static void Auto ()
        {
            cType = StatisticsController.ControlType.Auto;
            SceneManager.LoadScene(1);
        }

        public static void Manual ()
        {
            cType = StatisticsController.ControlType.Manual;
            SceneManager.LoadScene(1);
        }

        public void Exit ()
        {
            Application.Quit();
        }
    }
}

