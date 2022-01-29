using System;
using UnityEngine;

namespace Unity.Space4x.Assets.Scripts.GameObjects.Factories
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
                        GameObject tempGameObject = UnityEngine.Object.Instantiate(this.prefab);
                        tempGameObject.name = $"{this.name}_{this.index}";
                        T objectOfType = tempGameObject.GetComponent<T>();
                        this.index++;

                        return objectOfType;
                }
        }
}