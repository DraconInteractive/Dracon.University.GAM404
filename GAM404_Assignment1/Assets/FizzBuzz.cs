using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekFour
{
    public class FizzBuzz : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            DoFizzBuzz();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void DoFizzBuzz ()
        {
            //Going to 101 so it calcs 100, for my OCD :P
            for (int i = 0; i < 101; i++)
            {
                bool d = false;
                string s = "";
                if (i % 3 == 0)
                {
                    s += "Fizz";
                    d = true;
                }

                if (i % 5 == 0)
                {
                    s += "Buzz";
                    d = true;
                }

                if (!d)
                {
                    s += i.ToString();
                }

                print(s);
            }
        }
    }
}

