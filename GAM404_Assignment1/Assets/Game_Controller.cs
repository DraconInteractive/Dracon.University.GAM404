using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Week1;

namespace Week2
{
    public class Game_Controller : MonoBehaviour
    {

        public Character sceneCharacter;
        public Text nameText, levelText, experienceText, classText;
        public Button increaseLevel_B, addLevel_B, cycleClass_B, setClass_B;
        public InputField levelInput;
        public Dropdown classDropdown;
        // Use this for initialization
        void Start()
        {
            SetupButtons();
            sceneCharacter = new Character(0);
            UpdateUI();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void UpdateUI()
        {
            nameText.text = sceneCharacter.Username;
            levelText.text = "Level: " + sceneCharacter.Level.ToString();
            experienceText.text = "Experience: " + sceneCharacter.currentXP.ToString() + "/" + sceneCharacter.targetXP.ToString();
            classText.text = "Class: " + sceneCharacter.Class.ToString();
        }

        void SetupButtons()
        {
            increaseLevel_B.onClick.AddListener(() => IncreaseLevel());
            addLevel_B.onClick.AddListener(() => IncreaseLevels(int.Parse(levelInput.text)));
            cycleClass_B.onClick.AddListener(() => CycleClass());
            setClass_B.onClick.AddListener(() => SetClass());
        }

        public void IncreaseLevel()
        {
            sceneCharacter.Level++;
            UpdateUI();
        }

        public void IncreaseLevels(int amount)
        {
            sceneCharacter.Level += amount;
            UpdateUI();
        }

        public void CycleClass()
        {
            sceneCharacter.Class = GetCycledClass();
            UpdateUI();
        }

        public void SetClass()
        {
            sceneCharacter.Class = GetClassFromDropDown(classDropdown.value);
            UpdateUI();
        }

        public Character.Type GetCycledClass()
        {
            switch (sceneCharacter.Class)
            {
                case Character.Type.Knight:
                    return Character.Type.Mage;
                    break;
                case Character.Type.Mage:
                    return Character.Type.Rogue;
                    break;
                case Character.Type.Rogue:
                    return Character.Type.Knight;
                    break;
                default:
                    //Because "not all code paths return a value" :P
                    return Character.Type.Knight;
                    break;
            }
        }

        public Character.Type GetClassFromDropDown(int option)
        {

            switch (option)
            {
                case 0:
                    return Character.Type.Knight;
                    break;
                case 1:
                    return Character.Type.Mage;
                    break;
                case 2:
                    return Character.Type.Rogue;
                    break;
                default:
                    //Because "not all code paths return a value" :P
                    return Character.Type.Knight;
                    break;
            }
        }
    }
}

