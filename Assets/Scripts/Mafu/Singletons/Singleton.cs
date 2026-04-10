using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafu.Singletons
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        // Singleton classes allows for only a single game object to exist and be passed between scenes

        // public property docs: https://www.w3schools.com/cs/cs_properties.php
        // static keyword docs: https://www.geeksforgeeks.org/static-keyword-in-c-sharp/#

        // Declare a private variable
        private static T instance;

        // Declare a public variable with a built in getter and potentially setter method
        public static T Instance { get { return instance; } }


        // Protected means that it will only be visible to itself and children classes
        protected virtual void Awake()
        {
            // If this instance and gameobject already exist then destroy new copy
            if (instance != null && this.gameObject != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                // Else, assign instance to gameobject this is attached to, cast as generic type
                instance = (T)this;
            }

            if (!gameObject.transform.parent)
            {
                // Do not destroy this game object on load
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
