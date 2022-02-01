using System.Collections;
using OrbitModel = Space4x.Models.Orbit;

namespace Space4x.GameManagement.StateManagement.Orbit
{
        public class OrbitProcessingTurn : OrbitBaseState
        {
                public override IEnumerator Enter(OrbitStateManager stateManager, OrbitModel orbit)
                {
                        yield return orbit.TravelOrbit();

                        stateManager.SwitchState(stateManager.PlayerInput);
                }
        }
}
