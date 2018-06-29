using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Final
{
    public class StatisticsController : MonoBehaviour
    {
        public static StatisticsController controller;
        public int levelConstant, membersToBalance, balanceModifier;
        public List<Character> allCharacters = new List<Character>();
        public List<Character> teamOne = new List<Character>();
        public List<Character> teamTwo = new List<Character>();

        public enum ControlType
        {
            Auto,
            Manual
        }

        public ControlType controlType;
        public bool fightOnStart;

        [Header("UI")]
        public CharacterSlotUI[] characterSlots;
        public Button completeRosterButton;
        public GameObject stageOneCanvas, stageTwoCanvas;
        public CharacterSlotUI[] ss_teamOne, ss_teamTwo;
        public Text resultText;
        public Button fightButton;
        private void Awake()
        {
            controller = this;
            completeRosterButton.onClick.AddListener(() => AssignTeamsManual());
            fightButton.onClick.AddListener(() => CompleteManualBattleRound());
        }
        // Use this for initialization
        void Start()
        {
            balanceModifier = (int)(levelConstant * 0.15f);
            membersToBalance = 1 + (int)(levelConstant * 0.025f);
            PopulateRoster();
            if (controlType == ControlType.Auto)
            {
                AssignTeams();
                BalanceTeams();
            }


            if (fightOnStart)
            {
                //DoBattle();
                DoTournament();
            }

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PopulateRoster()
        {
            allCharacters.Clear();
            for (int i = 1; i < 16; i++)
            {
                allCharacters.Add(new Character(levelConstant)
                {
                    ID = i
                });
            }

            if (allCharacters.Count > 0 && characterSlots.Length > 0)
            {
                for (int i = 0; i < allCharacters.Count; i++)
                {
                    characterSlots[i].AssignCharacter(allCharacters[i]);
                }
            }
        }

        public void AssignTeams()
        {
            List<Character> aCharacters = new List<Character>(allCharacters);

            teamOne.Clear();
            teamTwo.Clear();

            for (int i = 0; i < 5; i++)
            {
                Character toAdd = aCharacters[Random.Range(0, aCharacters.Count - 1)];
                toAdd.team = Character.Team.One;
                teamOne.Add(toAdd);
                aCharacters.Remove(toAdd);
                //ss_teamOne[i].AssignCharacter(toAdd);
            }

            for (int i = 0; i < 5; i++)
            {
                Character toAdd = aCharacters[Random.Range(0, aCharacters.Count - 1)];
                toAdd.team = Character.Team.Two;
                teamTwo.Add(toAdd);
                aCharacters.Remove(toAdd);
                //ss_teamTwo[i].AssignCharacter(toAdd);
            }
        }

        public void AssignTeamsManual()
        {
            if (TeamContainer.teamContainers[0].tempTeam.Count == 0 || TeamContainer.teamContainers[1].tempTeam.Count == 0)
            {
                return;
            }
            teamOne.Clear();
            teamTwo.Clear();

            teamOne = new List<Character>(TeamContainer.teamContainers[0].tempTeam);
            teamTwo = new List<Character>(TeamContainer.teamContainers[1].tempTeam);

            BalanceTeams();

            stageOneCanvas.SetActive(false);
            stageTwoCanvas.SetActive(true);

            for (int i = 0; i < ss_teamOne.Length; i++)
            {
                ss_teamOne[i].AssignCharacter(teamOne[i]);
            }

            for (int i = 0; i < ss_teamTwo.Length; i++)
            {
                ss_teamTwo[i].AssignCharacter(teamTwo[i]);
            }
            print("finished assigning teams");
        }

        public void BalanceTeams()
        {
            for (int i = 0; i < membersToBalance; i++)
            {
                teamTwo[i].Recalculate(balanceModifier);
            }
            //foreach (Character c in teamTwo)
            //{
            //c.Recalculate(balanceModifier);
            //}
        }

        public string CompleteBattleRound()
        {
            string win = "";
            List<DamageResult> roundResults = new List<DamageResult>();
            foreach (Character c in teamOne)
            {
                if (teamTwo.Count > 0)
                {
                    Character target = GetTarget(teamTwo, c);

                    roundResults.Add(DamageTarget(target, c, teamTwo));
                }
                else
                {
                    win = "TEAM ONE WINNER";
                    break;
                }

            }
            if (win == "")
            {
                foreach (Character c in teamTwo)
                {
                    if (teamOne.Count > 0)
                    {
                        Character target = GetTarget(teamOne, c);
                        roundResults.Add(DamageTarget(target, c, teamOne));
                    }
                    else
                    {
                        win = "TEAM TWO WINNER";
                        break;
                    }

                }
            }


            string result = "";
            result += "TEAM ONE: " + teamOne.Count + "\n\n" + "TEAM TWO: " + teamTwo.Count + "\n\n";
            foreach (DamageResult r in roundResults)
            {
                result += "Char " + r.aggressor.ID + " hits " + "Char " + r.defender.ID + ". DMG: " + r.damage + ".";
                if (r.defenderDead)
                {
                    result += " Defender killed";
                }
                result += "\n";
            }
            result += "\n " + win;

            return result;
            //print(result);
        }

        public void CompleteManualBattleRound()
        {
            string s = CompleteBattleRound();
            resultText.text = s;

            foreach (CharacterSlotUI ui in ss_teamOne)
            {
                if (ui.myCharacter.statistics.health < 0)
                {
                    ui.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                }
            }

            foreach (CharacterSlotUI ui in ss_teamTwo)
            {
                if (ui.myCharacter.statistics.health < 0)
                {
                    ui.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                }
            }

            if (s.ToLower().Contains("winner"))
            {
                fightButton.GetComponentInChildren<Text>().text = "Retry";
                fightButton.onClick.AddListener(() => RestartLevel());
            }
        }

        void RestartLevel ()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        [ContextMenu("Do Battle")]
        public void DoBattle()
        {
            int roundCounter = 0;
            while (teamOne.Count > 0 && teamTwo.Count > 0)
            {
                CompleteBattleRound();
                roundCounter++;
            }
            string w = "";
            if (teamOne.Count <= 0)
            {
                w = "Team Two Wins";
            }
            else if (teamTwo.Count <= 0)
            {
                w = "Team One Wins";
            }
            print("Rounds to win: " + roundCounter + " " + w);
        }

        [ContextMenu("Do Tournament")]
        public void DoTournament ()
        {
            for (int i = 0; i < 10; i++)
            {
                PopulateRoster();
                AssignTeams();
                BalanceTeams();
                DoBattle();
            }
        }
        public Character GetTarget(List<Character> targets, Character c)
        {
            Character target = null;
            switch (c.attackType)
            {
                case Character.AttackType.Random:
                    target = targets[Random.Range(0, targets.Count - 1)];
                    break;
                case Character.AttackType.LowestHealth:
                    float bigHealth = Mathf.Infinity;
                    foreach (Character ch in targets)
                    {
                        if (ch.statistics.health < bigHealth)
                        {
                            bigHealth = ch.statistics.health;
                            target = ch;
                        }
                    }
                    break;
                case Character.AttackType.HighestHealth:
                    float lowHealth = -Mathf.Infinity;
                    foreach (Character ch in targets)
                    {
                        if (ch.statistics.health > lowHealth)
                        {
                            lowHealth = ch.statistics.health;
                            target = ch;
                        }
                    }
                    break;
                case Character.AttackType.LowestDP:
                    float bigDP = Mathf.Infinity;
                    foreach (Character ch in targets)
                    {
                        if (ch.statistics.damage < bigDP)
                        {
                            bigDP = ch.statistics.damage;
                            target = ch;
                        }
                    }
                    break;
                case Character.AttackType.HighestDP:
                    float lowDP = -Mathf.Infinity;
                    foreach (Character ch in targets)
                    {
                        if (ch.statistics.damage > lowDP)
                        {
                            lowDP = ch.statistics.damage;
                            target = ch;
                        }
                    }
                    break;
            }
            return target;
        }

        public DamageResult DamageTarget(Character target, Character origin, List<Character> targetTeam)
        {
            bool killingBlow = false;
            int dmg = origin.statistics.damage;

            float rnd = Random.Range(0, origin.statistics.level);
            if (rnd < (origin.statistics.hitChance - target.statistics.dodgeChance))
            {
                target.statistics.health -= origin.statistics.damage;
                if (target.statistics.health <= 0)
                {
                    targetTeam.Remove(target);
                    killingBlow = true;
                }
            } 
            else
            {
                dmg = 0;
            }



            DamageResult result = new DamageResult()
            {
                aggressor = origin,
                defender = target,
                defenderDead = killingBlow,
                damage = dmg
            };

            return result;
        }
    }

    [System.Serializable]
    public class Character
    {
        //Base Attributes
        public AttributeSet attributes;
        //Resultant Statistics
        public StatisticsSet statistics;

        public enum Team
        {
            One,
            Two
        };

        public Team team;

        public enum AttackType
        {
            Random,
            LowestHealth,
            LowestDP,
            HighestHealth,
            HighestDP
        }

        public AttackType attackType;

        public int ID;

        public Character(int lvl)
        {
            attributes = new AttributeSet(lvl);
            statistics = new StatisticsSet(attributes, lvl);
        }

        public void Recalculate (int changeInLevel)
        {
            attributes.AddToAttributes(changeInLevel);
            statistics.GenerateStatistics(attributes, statistics.level + changeInLevel);
        }

    }

    [System.Serializable]
    public class AttributeSet
    {
        public int dex, sta, str, acu;
        //public delegate void OnGenerateAttributes();
        //public OnGenerateAttributes onGenerateAttributes;

        public AttributeSet(int lvl)
        {
            GenerateAttributes(lvl);
        }

        public void GenerateAttributes(int lvl)
        {
            int counter = 0;

            dex = 0;
            sta = 0;
            str = 0;
            acu = 0;

            while (counter < lvl)
            {
                float f = Random.Range(0, 100);
                if (f > 75)
                {
                    dex++;
                }
                else if (f > 50)
                {
                    sta++;
                }
                else if (f > 25)
                {
                    str++;
                }
                else if (f > 0)
                {
                    acu++;
                }
                counter++;
            }
            

            //onGenerateAttributes();
        }

        public void AddToAttributes (int levelsToAdd)
        {
            int counter = 0;

            while (counter < levelsToAdd)
            {
                float f = Random.Range(0, 100);
                if (f > 75)
                {
                    dex++;
                }
                else if (f > 50)
                {
                    sta++;
                }
                else if (f > 25)
                {
                    str++;
                }
                else if (f > 0)
                {
                    acu++;
                }
                counter++;
            }
        }
    }

    [System.Serializable]
    public class StatisticsSet
    {
        public int level;
        public float xpCurrent, xpTarget;
        public int dodgeChance, health, damage, hitChance;

        public StatisticsSet()
        {

        }

        public StatisticsSet(AttributeSet att, int lvl)
        {
            GenerateStatistics(att, lvl);
        }

        public void GenerateStatistics(AttributeSet att, int lvl)
        {
            level = lvl;
            //Notes on algorithm.
            //Set up a base level. The numbers are hardcoded, i know, but ive been tinkering to get nice values out of them. 
            //The base values let every character have at least a certain level of a statistic. 
            //From there, add advantages from point assignation. 
            float dg = (level * 2) + (att.dex * 3.5f);
            float hp = (level * 10f) + (att.sta * 7.5f);
            float dmg = (level * 5) + (att.str * 3.5f);
            float hit = (level * 4.5f) + (att.acu * 8);

            dodgeChance = (int)dg;
            health = (int)hp;
            damage = (int)dmg;
            hitChance = (int)hit;
        }
    }

    public class DamageResult
    {
        public Character aggressor, defender;
        public bool defenderDead;
        public int damage;

    }

}

