using Space4x.Models.Factories;
using UnityEngine;

namespace Space4x.Models.Factories
{
        public class MonoBehaviourFactory<T> : IFactory<T> where T : MonoBehaviour
        {
                private readonly string name;
                private int index = 1;

                public MonoBehaviourFactory() : this("MonoBehaviour") { }

                public MonoBehaviourFactory(string name)
                {
                        this.name = name;
                }

                public T Create()
                {
                        GameObject tempGameObject = Object.Instantiate(new GameObject());

                        tempGameObject.name = $"{this.name}_{this.index}";
                        tempGameObject.AddComponent<T>();
                        T objectOfType = tempGameObject.GetComponent<T>();
                        if (objectOfType == null)
                        {
                                objectOfType = tempGameObject.GetComponentInChildren<T>();
                                objectOfType.name = $"{this.name}_{this.index}";
                        }

                        this.index++;

                        return objectOfType;
                }
        }
}