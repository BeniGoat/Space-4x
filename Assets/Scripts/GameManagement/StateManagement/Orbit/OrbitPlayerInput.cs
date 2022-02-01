using OrbitModel = Space4x.Models.Orbit;

namespace Space4x.GameManagement.StateManagement.Orbit
{
        public class OrbitPlayerInput : OrbitBaseState
        {
                public override void HandleInput(OrbitStateManager stateManager, OrbitModel orbit)
                {
                        if (Managers.GameManager.Instance.InputManager.IsEndTurnButtonPressed())
                        {
                                stateManager.SwitchState(stateManager.ProcessingTurn);
                        }
                }
        }
}
