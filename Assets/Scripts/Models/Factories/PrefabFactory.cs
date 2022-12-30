using UnityEngine;

namespace Space4x.Models.Factories
{
        public class PrefabFactory<T> : IFactory<T> where T : MonoBehaviour
        {
                private GameObject prefab;
                private readonly string name;
                private int index = 1;

                public PrefabFactory(GameObject prefab) : this(prefab, prefab.name) { }

                public PrefabFactory(GameObject prefab, string name)
                {
                        this.prefab = prefab;
                        this.name = name;
                }

                public T Create()
                {
                        GameObject tempGameObject = Object.Instantiate(this.prefab);
                        tempGameObject.name = $"{this.name}_{this.index}";
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