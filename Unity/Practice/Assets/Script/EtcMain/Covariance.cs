using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HanPractice
{
    public class Covariance : BaseExampleClass
    {
        public override void Run()
        {
            Debug.Log("Convariance Run");

            ICreate<Dog> dogCreator = new DogFactory();
            ICreate<Animal> creator = dogCreator;
            Animal ani = creator.Create();
            ani.Move();

            IEnumerable<string> strings = new List<string>();
            IEnumerable<object> objects = strings;
            objects = new List<string>();

            Func<Animal, Animal> testFunc = delegate (Animal a)
            {
                return a;
            };

            testFunc(new Dog());

            List<ICreate<Animal>> animalList = null;
            List<ICreate<Dog>> doglList = null;
            // Error
//            animalList = doglList;
        }

    }

    class Animal
    {
        public int Age { get; set; }
        public virtual void Move() { Debug.Log("Animal Move"); }
    }
    class Dog : Animal
    {
        public void Bark() { }
        public override void Move() { Debug.Log("Dog Move"); }
    }
    class K9 : Dog
    {
        public void FindDrug() { }
        public override void Move() { Debug.Log("K9 Move"); }
    }

    // 제네릭 인터페이스: Covariance 
    interface ICreate<out T>
    {
        T Create();
    }

    class DogFactory : ICreate<Dog>
    {
        public Dog Create()
        {
            return new Dog();
        }
    }
}
