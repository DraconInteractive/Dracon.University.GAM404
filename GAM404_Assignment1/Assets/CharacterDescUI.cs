using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Final
{
    //Desc UI is the window that pops up when you click a character slot
    public class CharacterDescUI : MonoBehaviour
    {
        //store ref to character. ref, not new var
        [HideInInspector]
        public Character myCharacter;
        //Relevant UI variables
        public Button closeButton;
        public Text ch_NameText, ch_AttType, ch_attributes, ch_statistics;

        public Button sa_lowH, sa_highH, sa_highDPS;
        // Use this for initialization
        void Start()
        {
            //setup button to close panel on click
            closeButton.onClick.AddListener(() => Close());
            sa_lowH.onClick.AddListener(() => SetAttType(0));
            sa_highH.onClick.AddListener(() => SetAttType(1));
            sa_highDPS.onClick.AddListener(() => SetAttType(2));
        }

        // Update is called once per frame
        void Update()
        {

        }
        //Assign the character. 
        //Create some strings to hold attributes and stats, then assign all to relevant ui elements. 
        public void AssignCharacter (Character ch)
        {
            ch_NameText.text = "Character " + ch.ID;
            ch_AttType.text = "Attack Type: " + ch.attackType;
            string att = "";
            att += "Attributes:" + "\n\n";
            att += "Strength: " + ch.attributes.str + "\n";
            att += "Dexterity: " + ch.attributes.dex + "\n";
            att += "Acuity: " + ch.attributes.acu + "\n";
            att += "Stamina: " + ch.attributes.sta + "\n";
            ch_attributes.text = att;
            string stat = "";
            stat += "Statistics: " + "\n\n";
            stat += "Level: " + ch.statistics.level + "\n";
            stat += "Experience: " + ch.statistics.xpCurrent + "/" + ch.statistics.xpTarget + "\n";
            stat += "Health: " + ch.statistics.health + "\n";
            stat += "HitChance: " + ch.statistics.hitChance + "\n";
            ch_statistics.text = stat;
            myCharacter = ch;
        }

        void Close ()
        {
            //... this.
            Destroy(this.gameObject);
        }

        public void SetAttType (int i)
        {
            switch (i)
            {
                case 0:
                    myCharacter.attackType = Character.AttackType.LowestHealth;
                    break;
                case 1:
                    myCharacter.attackType = Character.AttackType.HighestHealth;
                    break;
                case 2:
                    myCharacter.attackType = Character.AttackType.HighestDP;
                    break;
            }
            ch_AttType.text = "Attack Type: " + myCharacter.attackType;
        }
    }
}

