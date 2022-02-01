using System.Collections;
using OrbitModel = Space4x.Models.Orbit;

namespace Space4x.GameManagement.StateManagement.Orbit
{
        public abstract class OrbitBaseState
        {
                public virtual IEnumerator Enter(OrbitStateManager stateManager, OrbitModel orbit)
                {
                        yield return null;
                }

                public virtual void HandleInput(OrbitStateManager stateManager, OrbitModel orbit)
                {

                }
        }
}
