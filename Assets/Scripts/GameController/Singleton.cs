using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGGE
{
    namespace Patterns
    {
        //Singleton class that ensures only one instance of a specified component type exists in the scene.
        public abstract class Singleton<T> : MonoBehaviour where T : Component
        {
            private static T s_instance;

            public static T Instance
            {
                get
                {
                    //If the instance is null, attempt to find an existing instance in the scene
                    if (s_instance == null)
                    {
                        s_instance = FindObjectOfType<T>();
                        if (s_instance == null)
                        {
                            GameObject obj = new GameObject();
                            obj.name = typeof(T).Name;
                            s_instance = obj.AddComponent<T>();
                        }
                    }
                    //Return the instance 
                    return s_instance;
                }
            }

            protected virtual void Awake()
            {
                //If the singleton instance is null, set it to this instance and mark it to not be destroyed on scene load
                if (s_instance == null)
                {
                    s_instance = this as T;
                    DontDestroyOnLoad(gameObject);
                }
                else
                {
                    //If another instance already exists, destroy this instance
                    Destroy(gameObject);
                }
            }
        }
    }
}