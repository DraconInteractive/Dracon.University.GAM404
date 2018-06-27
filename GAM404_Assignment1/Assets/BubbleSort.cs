using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekFour
{
    public class BubbleSort : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            List<string> names = new List<string>()
        {
            "A Name",
            "Z Name",
            "H Name",
            "D Name",
            "L Name",
            "V Name"
        };
            SortAlphabet(names);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void SortAlphabet(List<string> names)
        {
            bool didSwap = true;
            while (didSwap)
            {
                didSwap = false;

                for (int i = 0; i < (names.Count - 1); i++)
                {
                    string c = names[i];
                    string n = names[i + 1];
                    //Less than means descending, greater mean ascending
                    if (c.CompareTo(n) > 0)
                    {
                        didSwap = true;

                        names[i] = n;
                        names[i + 1] = c;
                    }
                }
            }

            string s = "";
            foreach (string ss in names)
            {
                s += ss + "\n";
            }
            print(s);
        }
    }

}
