using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Final
{
    //Main controller for the game, all the fun stuff. 
    public class StatisticsController : MonoBehaviour
    {
        //singletons are love and life itself. 
        public static StatisticsController controller;
        //level constant sets characters levels
        //memebers to balance controls how many members of team two are adjusted
        //balance modifier controls how much the adjusted members are ... adjusted
        public int levelConstant, membersToBalance, balanceModifier;
        //global storage for characters
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
        //All the ui piecess. CharacterSlotUI is used for the draggable slot interfaces
        [Header("UI")]
        public CharacterSlotUI[] characterSlots;
        public Button completeRosterButton;
        public GameObject stageOneCanvas, stageTwoCanvas;
        public CharacterSlotUI[] ss_teamOne, ss_teamTwo;
        public Text resultText;
        public Button fightButton;
        //Lets the player know how many rounds have passed
        public int rounderCounter = 1;

        //changing team reference to a public variable due to bug
        public TeamContainer teamOneC, teamTwoC;
        private void Awake()
        {
            //assign the singleton and setup the button listeners. Then, get control type (manual vs auto) from a persistent menu object. 
            controller = this;
            completeRosterButton.onClick.AddListener(() => AssignTeamsManual());
            fightButton.onClick.AddListener(() => CompleteManualBattleRound());

            controlType = MenuController.cType;
            GameObject g = GameObject.FindGameObjectWithTag("menu");
            Destroy(g);
        }
        // Use this for initialization
        void Start()
        {
            //set balance modifier (hardcoded, i know) and members to balance. 
            //Then, populate the roster (make the characters)
            balanceModifier = (int)(levelConstant * 0.15f);
            membersToBalance = 1 + (int)(levelConstant * 0.025f);
            PopulateRoster();
            //if the control is manual, the player can use the buttons. Otherwise, this coroutine will cycle the actions on a timer. 
            if (controlType == ControlType.Auto)
            {
                StartCoroutine(AutoController());
            }

            /*
            if (fightOnStart)
            {
                //DoBattle();
                DoTournament();
            }
            */
        }

        // Update is called once per frame
        void Update()
        {

        }
        //controls automatic actions
        IEnumerator AutoController ()
        {
            //Let the scene settle, then part the teams and balance them.

            yield return new WaitForSeconds(1);
            AssignTeams();
            BalanceTeams();
            //Apply the data changes too the UI
            foreach (Character c in teamOne)
            {
                teamOneC.AddCharacterToTeam(c);
            }
            //put a wait in the middle to help the player keep pace
            yield return new WaitForSeconds(1);

            foreach (Character c in teamTwo)
            {
                teamTwoC.AddCharacterToTeam(c);
            }
            //Set the button to green to signify that the player can progress to the next stage
            completeRosterButton.GetComponent<Image>().color = Color.green;
            //idle until the player gets the idea
            while (!stageTwoCanvas.activeSelf)
            {
                yield return null;
            }
            //Pace it with a wait
            yield return new WaitForSeconds(2);
            //later in the code I change the fightbutton text to "Retry" instead of "Fight". Since this is every 2 seconds, im fine with a get comp, but when i go back through this ima store that earlier. 
            while (fightButton.GetComponentInChildren<Text>().text != "Retry")
            {
                //While waiting, do a battle (process expalined in later comments), advance the round and then wait. Maybe longer than 2 seconds, but ill get back to this lateer
                CompleteManualBattleRound();
                rounderCounter++;
                yield return new WaitForSeconds(2);
            }
            //Now that there is a winner, turn the retry button green to signify the player should do the thing. 
            fightButton.GetComponent<Image>().color = Color.green;
            yield break;
        }
        //function to make all the characters
        public void PopulateRoster()
        {
            //clear the roster to begin with
            allCharacters.Clear();
            //Add 15 new characters, assign ID's as nessecary. With more work, characcters get random gen names, but thats for later. Maybe i should start using the TODO tool :P
            //Using a list because I want to be able to resize it if neccesary. 4 man teams etc. 
            for (int i = 1; i < 16; i++)
            {
                allCharacters.Add(new Character(levelConstant)
                {
                    ID = i
                });
            }
            //Assign characters to slots in the main window. 
            if (allCharacters.Count > 0 && characterSlots.Length > 0)
            {
                for (int i = 0; i < allCharacters.Count; i++)
                {
                    characterSlots[i].AssignCharacter(allCharacters[i]);
                }
            }
        }
        //Used to split the character list into two teams of 5 (for now)
        public void AssignTeams()
        {
            //Copy the main character list into a temp so i can screw with it without consequences
            List<Character> aCharacters = new List<Character>(allCharacters);
            //Clear any previous progress
            teamOne.Clear();
            teamTwo.Clear();
            //get a random character from the list. Assign team, add them to list and then remove them from the temp so we dont get any doubles. 
            //redo 4 times. 
            for (int i = 0; i < 5; i++)
            {
                Character toAdd = aCharacters[Random.Range(0, aCharacters.Count - 1)];
                toAdd.team = Character.Team.One;
                teamOne.Add(toAdd);
                aCharacters.Remove(toAdd);
                //ss_teamOne[i].AssignCharacter(toAdd);
            }
            //Do again but for team 2
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
            //Check the ui is there, and that characters have been given to teams. As this is for manual control, you can have custom team sizes if you wish, to a maximum of 5. 
            if (teamOneC.tempTeam.Count == 0 || teamTwoC.tempTeam.Count == 0)
            {
                if (teamOneC.tempTeam.Count == 0)
                {
                    print("team one has no people");
                }
                if (teamTwoC.tempTeam.Count == 0)
                {
                    print("team two has no people");
                }
                return;
            }
            //Clear old values
            teamOne.Clear();
            teamTwo.Clear();
            //Get values from the team container UI and propogate into the global controller
            teamOne = new List<Character>(teamOneC.tempTeam);
            teamTwo = new List<Character>(teamTwoC.tempTeam);
            //Balance them
            BalanceTeams();
            //This function marks the end of the team choice, so switch the UI to the fight screen
            stageOneCanvas.SetActive(false);
            stageTwoCanvas.SetActive(true);
            //pass team data to fight screen character slots. 
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
        //Get the first x memebers of team two, and give them y extra skill points to balance out the first move advantage
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
        //function that gets all the characters to hit each other, then return a string composed of results
        public string CompleteBattleRound()
        {
            //setup string
            string win = "";
            //create container for results
            List<DamageResult> roundResults = new List<DamageResult>();
            //if team two has alive players, get a target from team two (depending on targetting type, passed through the character input of the GetTarget function). 
            //Once the character has a target, hit it. Subtract health etc with regards to variaables (explored further in DamageTarget)
            //If all team two is dead, just set the text to state that Team One wins. 
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
            //If team one hasnnt one, and team two is thus still alive, check to see if team one is alive. 
            //if so, target and damage a character. If not, set text to Team two winner. 
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

            //Make a string for the final text return. Initialise it with the roundcounter. 
            //Add in the team sizes (alive). 
            //Then, include results for every hit. Result includes hitter, hitee and damage. 
            //If the hitee dies, include that in the report. 
            //Add appropriate spacing then return the string. 
            string result = "Round: " + rounderCounter.ToString() + "\n\n";
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
        //Characters hitting each other ... manually
        public void CompleteManualBattleRound()
        {
            //Get a single round of hit. Store the result. 
            string s = CompleteBattleRound();
            //Set the UI
            resultText.text = s;
            //Check the characters for deadness. If dead then set slot to red signifying it. 
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
            //If someone won, then set the text of the fight button to "Retry" (See, told you I would explain later). Then, reset the listener of the fightbutton to restart the level. 
            if (s.ToLower().Contains("winner"))
            {
                fightButton.GetComponentInChildren<Text>().text = "Retry";
                fightButton.onClick.AddListener(() => RestartLevel());
            }
        }
        //Pretty simple. Just load the menu up. Using integer because of laziness / time constraints. 
        //Okay, ran into a bug. Redoing manual after retry kills team one instantly. 
        //#2 I tried wiping the lists, but now i cant advance past team selection. I was SO CLOSE!
        void RestartLevel ()
        {
            completeRosterButton.onClick.AddListener(() => AssignTeamsManual());
            fightButton.onClick.AddListener(() => CompleteManualBattleRound());
            allCharacters.Clear();
            teamOne.Clear();
            teamTwo.Clear();
            teamOneC.tempTeam.Clear();
            teamTwoC.tempTeam.Clear();
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        //Editor UI control. Makes them hit each other until ne team wins. Would have used it or auto but auto needed the timing for awesomeness. 
        [ContextMenu("Do Battle")]
        public void DoBattle()
        {
            //Start counting rounds. While both of the teams are alive, hit each other, then advance the round. 
            int roundCounter = 0;
            while (teamOne.Count > 0 && teamTwo.Count > 0)
            {
                CompleteBattleRound();
                roundCounter++;
            }
            //Get winner and print it. 
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
        //Again, an editor function. This will create the players, make teams, balance them and then pit them against each other until a team wins. Then...it will do it 10 times.
        //I use this for the balancing function
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
        //function that returns a character from a list of targets, based upon Character c's targetting preference
        public Character GetTarget(List<Character> targets, Character c)
        {
            Character target = null;
            switch (c.attackType)
            {
                //If type is random, just use random range to get a target and return it. 
                case Character.AttackType.Random:
                    target = targets[Random.Range(0, targets.Count - 1)];
                    break;
                case Character.AttackType.LowestHealth:
                    //If lowest health, cycle through the team testing for lowest health. At the end, return. 
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
                    //Exactly the same as lowest health, but highest instead. 
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
                    //Im 100% certain you get the gist by now. Same thing, different variable
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
        //Function to apply damage to character statistics
        public DamageResult DamageTarget(Character target, Character origin, List<Character> targetTeam)
        {
            //track whether the attacked character dies. Get damage from the supplied attackers statistics variable
            bool killingBlow = false;
            int dmg = origin.statistics.damage;
            //My attempt to integrate hitchance into the combat. It needs a LOT more testing, but basically, if the result of the attackers hitchance and the defenders dodgechance is less than an adjusted level, then defender dodges. I need to finetune mod!
            float rnd = Random.Range(0, origin.statistics.level);
            float mod = 1.5f;
            if (rnd * mod < (origin.statistics.hitChance - target.statistics.dodgeChance))
            {
                //If the character hits, subtract damage from health. If health runs out, remove the target from the team (so other characters dont target it), then set killingBlow to true. 
                target.statistics.health -= origin.statistics.damage;
                if (target.statistics.health <= 0)
                {
                    targetTeam.Remove(target);
                    killingBlow = true;
                }
            } 
            else
            {
                //Dodgey stuff right here. 
                dmg = 0;
            }


            //Create a result to store the events of the hit, then return it. 
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
    //A class to contain all the character variables. contains a constructor to generate a character from level input as well. 
    [System.Serializable]
    public class Character
    {
        //Base Attributes
        public AttributeSet attributes;
        //Resultant Statistics
        public StatisticsSet statistics;
        //Only used in auto
        public enum Team
        {
            One,
            Two
        };

        public Team team;
        //Adjust targetting with this
        public enum AttackType
        {
            Random,
            LowestHealth,
            LowestDP,
            HighestHealth,
            HighestDP
        }

        public AttackType attackType;
        //Used for ID until I do name gen
        public int ID;
        //take an int, and feed it into constructors for attributes and statistics. 
        public Character(int lvl)
        {
            attributes = new AttributeSet(lvl);
            statistics = new StatisticsSet(attributes, lvl);
        }
        //Used in balancing teams, redoes the stats 
        public void Recalculate (int changeInLevel)
        {
            attributes.AddToAttributes(changeInLevel);
            statistics.GenerateStatistics(attributes, statistics.level + changeInLevel);
        }

    }
    //Class to store base attributes
    [System.Serializable]
    public class AttributeSet
    {
        public int dex, sta, str, acu;
        
        //input level, get out attributes
        public AttributeSet(int lvl)
        {
            GenerateAttributes(lvl);
        }

        //null the stats, then give each an equal chance to advance. Do this once for every level of the character. 
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
            
            //I did have a (thing that does a thing when a thing happens that i cant remember the name of). Dont have it now, so this is just for s&g
            //onGenerateAttributes();
        }
        //same thing as above, but dont null them at beginning, thus adding to existing values. Used for balancing additions. 
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
    //Container for resultant statistics
    [System.Serializable]
    public class StatisticsSet
    {

        public int level;
        public float xpCurrent, xpTarget;
        public int dodgeChance, health, damage, hitChance;
        //leaving this here so I can have an empty stat set
        public StatisticsSet()
        {

        }
        //Input pregenerated attributes and level for stat generation
        public StatisticsSet(AttributeSet att, int lvl)
        {
            GenerateStatistics(att, lvl);
        }

        public void GenerateStatistics(AttributeSet att, int lvl)
        {
            //straight up set level. 
            level = lvl;
            //Set up a base level. The numbers are hardcoded, i know, but ive been tinkering to get nice values out of them. 
            //The base values let every character have at least a certain level of a statistic. 
            //From there, add advantages from point assignation. 
            float dg = (level * 2) + (att.dex * 3.5f);
            float hp = (level * 10f) + (att.sta * 7.5f);
            float dmg = (level * 5) + (att.str * 3.5f);
            float hit = (level * 4.5f) + (att.acu * 8);
            //Round to nearest int, computer style, not normal style. Too tired to fully comprehend that, but you understand. I hope.
            dodgeChance = (int)dg;
            health = (int)hp;
            damage = (int)dmg;
            hitChance = (int)hit;
        }
    }
    //prementioned container for damage results. 
    public class DamageResult
    {
        public Character aggressor, defender;
        public bool defenderDead;
        public int damage;

    }

}

