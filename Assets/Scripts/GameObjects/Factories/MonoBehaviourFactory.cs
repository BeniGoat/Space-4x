using UnityEngine;

namespace Unity.Space4x.Assets.Scripts.GameObjects.Factories
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
                        this.index++;

                        return objectOfType;
                }
        }
}