namespace Unity.Space4x.Assets.Scripts.GameObjects.Factories
{
        /// <summary>
        /// Factory design pattern with generic.
        /// </summary>
        public class Factory<T> : IFactory<T> where T : new()
        {
                public T Create()
                {
                        return new T();
                }
        }
}