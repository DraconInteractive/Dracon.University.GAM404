using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Final
{
    //class made to interface with the team ui panels
    public class TeamContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        //make a public ref so i can access easily from other classes
        public static List<TeamContainer> teamContainers = new List<TeamContainer>();
        public bool mouseOver = false;
        //various references to UI elements. tempTeam is to store the team while the character builds it. 
        public Text teamText;
        public List<Character> tempTeam;

        public Text teamStr, teamDex, teamSta, teamAcu;

        void OnEnable ()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        void OnDisable ()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }
        // Use this for initialization
        void Start()
        {
            if (!teamContainers.Contains(this))
            {
                teamContainers.Add(this);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
        //Detect whether mouse is over team ui. Dont think i use this anymore, replaced it with a raycast but i dont have time to remove it
        public void OnPointerEnter(PointerEventData eventData)
        {
            mouseOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            mouseOver = false;
        }
        //public method for setting team UI
        public bool AddCharacterToTeam(Character c)
        {
            //Check for team size limit
            if (tempTeam.Count < 5)
            {
                tempTeam.Add(c);
            }
            else
            {
                return false;
            }
            //All one big text unit, bullet points useful AF. 
            //foreach character, we are going to add them to the list. Then, get the combined value of their statistics and show that too. 
            string s = "";
            foreach (Character ch in tempTeam)
            {
                s += "•" + " Character " + ch.ID + "\n";
            }

            s += "\n\n";
            int str = 0;
            int dex = 0;
            int acu = 0;
            int sta = 0;
            foreach (Character ch in tempTeam)
            {
                str += ch.attributes.str;
                dex += ch.attributes.dex;
                acu += ch.attributes.acu;
                sta += ch.attributes.sta;
            }

            s += "Totals: " + "\n\n";
            s += "Strength: " + str.ToString() + "\n";
            s += "Dexterity: " + dex.ToString() + "\n";
            s += "Stamina: " + sta.ToString() + "\n";
            s += "Acuity: " + acu.ToString() + "\n";
            teamText.text = s;
            //after applying the change, check to see if we reached the team limit. If so, make it green to signify that the player is done with this team. 
            if (tempTeam.Count == 5)
            {
                GetComponent<Image>().color = new Color(0.3f, 0.7f, 0.3f, 0.7f);
            }
            return true;
        }

        void OnLevelFinishedLoading (Scene scene, LoadSceneMode mode)
        {
            if (!teamContainers.Contains (this))
            {
                teamContainers.Add(this);
            }
        }
    }
}

