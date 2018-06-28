using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Final
{
    public class CharacterDescUI : MonoBehaviour
    {
        [HideInInspector]
        public Character myCharacter;
        public Button closeButton;
        public Text ch_NameText, ch_AttType, ch_attributes, ch_statistics;
        // Use this for initialization
        void Start()
        {
            closeButton.onClick.AddListener(() => Close());
        }

        // Update is called once per frame
        void Update()
        {

        }

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
            Destroy(this.gameObject);
        }
    }
}

