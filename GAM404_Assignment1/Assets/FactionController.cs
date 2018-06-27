using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Final;
namespace Week3
{
    public class FactionController : MonoBehaviour
    {
        public List<Character> allCharacters = new List<Character>();
        public Character[] teamOne, teamTwo;
        public int teamSize;
        // Use this for initialization
        void Start()
        {
            GenerateAll();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void GenerateAll ()
        {
            for (int i = 0; i < teamSize * 2; i++)
            {
                allCharacters.Add(new Character(1));
            }
        }

        public void CreateAndAssignTeams ()
        {
            List<Character> available = new List<Character>(allCharacters);
            teamOne = new Character[teamSize];
            teamTwo = new Character[teamSize];

            for (int i = 0; i < teamSize; i++)
            {
                teamOne[i] = available[Random.Range(0, available.Count)];
                available.Remove(teamOne[i]);
            }
            for (int i = 0; i < teamSize; i++)
            {
                teamTwo[i] = available[Random.Range(0, available.Count)];
                available.Remove(teamTwo[i]);
            }
        }

        public void DoThing ()
        {
            //Honestly cant think of a while loop use for this, so forgive the weird convoluted way i did this
            List<Character> available = new List<Character>(allCharacters);
            while (available.Count > 0)
            {
                available[0].statistics.level += 1;
                available.Remove(available[0]);
            }
        }
    }
}

