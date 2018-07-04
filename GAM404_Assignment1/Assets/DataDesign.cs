using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week6
{
    public class DataDesign : MonoBehaviour
    {
        List<Being> allEntities = new List<Being>();
        // Use this for initialization
        void Start()
        {
            CreateAndSetup();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void CreateAndSetup()
        {

            //Humans
            Human aaron = new Human("Aaron", 100, 55, null, null);
            allEntities.Add(aaron);
            Human bob = new Human("Bob", 75, 62, null, null);
            allEntities.Add(bob);
            Human charlie = new Human("Charlie", 110, 25, null, null);
            allEntities.Add(charlie);
            Human darien = new Human("Darien", 40, 22, null, null);
            allEntities.Add(darien);

            //Pets
            Pet dog = new Pet("Dog", 30, 3, null, null, 3);
            allEntities.Add(dog);
            Pet cat = new Pet("Cat", 25, 5, null, null, 2);
            allEntities.Add(cat);
            Pet rabbit = new Pet("Rabbit", 30, 3, null, null, 1);
            allEntities.Add(rabbit);

            //Wild Animals
            Animal wolf1 = new Animal("Wolf_1", 55, 4, null, 5);
            Animal wolf2 = new Animal("Wolf_2", 55, 4, null, 5);
            Animal wolf3 = new Animal("Wolf_3", 55, 4, null, 5);
            Animal wolf4 = new Animal("Wolf_4", 55, 4, null, 5);
            Animal bear = new Animal("Bear", 120, 3, null, 7);
            allEntities.Add(wolf1);
            allEntities.Add(wolf2);
            allEntities.Add(wolf3);
            allEntities.Add(wolf4);
            allEntities.Add(bear);

            //Setup
            aaron.friends.Add(charlie);
            aaron.friends.Add(darien);
            bob.friends.Add(charlie);
            charlie.friends.Add(aaron);
            charlie.friends.Add(bob);
            darien.friends.Add(aaron);

            aaron.pet = cat;
            charlie.pet = rabbit;
            darien.pet = dog;

            cat.pet = aaron;
            rabbit.pet = charlie;
            dog.pet = darien;

            List<Animal> wolfpack = new List<Animal>()
        {
            wolf1,
            wolf2,
            wolf3,
            wolf4
        };
            wolf1.pack = wolfpack;
            wolf2.pack = wolfpack;
            wolf3.pack = wolfpack;
            wolf4.pack = wolfpack;
        }
    }

    public class Being
    {
        public struct Resource
        {
            public float maximum;
            public float current;


            public Resource(float _maximum)
            {
                maximum = _maximum;
                current = maximum;
            }

            public void Change(float x)
            {
                current = Mathf.Clamp(current + x, 0, maximum);
            }
        }

        public string name;
        public Resource health;
        public int age;

        public Being(string _name, int _mHealth, int _age)
        {
            name = _name;
            health = new Resource(_mHealth);
            age = _age;
        }
    }

    public class Human : Being
    {
        public List<Human> friends = new List<Human>();
        public Animal pet;

        public Human(string _name, int _mHealth, int _age, List<Human> _s, Animal _p) : base(_name, _mHealth, _age)
        {
            friends = new List<Human>(_s);
            pet = _p;
        }
    }

    public class Animal : Being
    {
        public List<Animal> pack = new List<Animal>();
        public int predatorLevel;
        public Animal(string _name, int _mHealth, int _age, List<Animal> _pack, int _pred) : base(_name, _mHealth, _age)
        {
            pack = new List<Animal>(_pack);
            predatorLevel = _pred;
        }
    }

    public class Pet : Animal
    {
        public Pet(string _name, int _mHealth, int _age, List<Animal> _pack, Human _pet, int _pred) : base(_name, _mHealth, _age, _pack, _pred)
        {
            pet = _pet;
        }

        public Human pet;
    }
}


