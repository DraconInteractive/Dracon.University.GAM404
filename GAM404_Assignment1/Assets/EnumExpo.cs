using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumExpo : MonoBehaviour {

    public enum ClassType
    {
        Hunter,
        Barbarian,
        Warlord,
        Junker,
        Pickpocket,
        Tailor,
        NullClass
    };

    public ClassType myClass;

    public List<ClassType> allClasses = new List<ClassType>();

	// Use this for initialization
	void Start () {
        PopulateAllContainer();
        CycleEnum();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            CycleEnum();
        }
	}

    void PopulateAllContainer ()
    {
        allClasses.Clear();
        allClasses.Add(ClassType.Hunter);
        allClasses.Add(ClassType.Barbarian);
        allClasses.Add(ClassType.Warlord);
        allClasses.Add(ClassType.Junker);
        allClasses.Add(ClassType.Pickpocket);
        allClasses.Add(ClassType.Tailor);
        allClasses.Add(ClassType.NullClass);
    }
    void CycleEnum ()
    {
        //Using this opportunity to look at the technique Fey mentioned a couple weeks ago, cycling through a enum container to go from one to the next. 
        //Thus, this algorithm will change class to the next available class;
        int index = allClasses.IndexOf(myClass);
        index++;
        if (index >= allClasses.Count)
        {
            index = 0;
        }
        myClass = allClasses[index];
    }
}
