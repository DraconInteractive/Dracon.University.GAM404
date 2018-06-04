using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreationController : MonoBehaviour {

    public Button createButton;

    public InputField usernameInput, levelInput;
    public Dropdown classInput;

    public Text finalUsername, finalLevel, finalExperience, finalClass;

    public List<Character> createdCharacters = new List<Character>();
	// Use this for initialization
	void Start () {
        createButton.onClick.AddListener(() => CreateCharacter());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateCharacter()
    {
        string u = usernameInput.text;
        int l = 0;
        bool tryLevel = int.TryParse(levelInput.text, out l);
        Character.Type c;
        int tryClass = classInput.value;
        switch (tryClass)
        {
            case 0:
                c = Character.Type.Knight;
                break;
            case 1:
                c = Character.Type.Mage;
                break;
            case 2:
                c = Character.Type.Rogue;
                break;
            default:
                c = Character.Type.Knight;
                break;
        }

        Character newCharacter = new Character()
        {
            Username = u,
            Level = l,
            Class = c,
            currentXP = 0,
            targetXP = l * 123
        };

        createdCharacters.Add(newCharacter);

        finalUsername.text = u;
        finalLevel.text = l.ToString();
        finalClass.text = c.ToString();
        finalExperience.text = newCharacter.currentXP + "/" + newCharacter.targetXP;
    }
}

[System.Serializable]
public class Character
{
    public string Username;
    public int Level;
    public int currentXP, targetXP;
    public enum Type
    {
        Knight, 
        Rogue,
        Mage
    };
    public Type Class;

    public Character ()
    {

    }

    public Character (int random)
    {
        string[] usernames = new string[]
        {
            "Gorok",
            "Ushnark",
            "Argie",
            "Gordon"
        };

        Username = usernames[Random.Range(0, usernames.Length)];
        Level = Random.Range(0, 10);
        currentXP = Random.Range(0, 1000);
        targetXP = Random.Range(currentXP, 10000);
        Class = Type.Knight;
    }
}
