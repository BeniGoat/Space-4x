using UnityEngine;
using OrbitModel = Space4x.Models.Orbit;

namespace Space4x.GameManagement.StateManagement.Orbit
{
        public class OrbitStateManager : MonoBehaviour
        {
                private OrbitBaseState currentState;
                private OrbitModel orbit;

                public OrbitPlayerInput PlayerInput { get; set; }
                public OrbitProcessingTurn ProcessingTurn { get; set; }

                private void Awake()
                {
                        this.orbit = this.GetComponent<OrbitModel>();
                        this.PlayerInput = new OrbitPlayerInput();
                        this.ProcessingTurn = new OrbitProcessingTurn();
                }

                private void Start()
                {
                        this.SwitchState(this.PlayerInput);
                }

                private void Update()
                {
                        this.currentState.HandleInput(this, this.orbit);
                }

                public void SwitchState(OrbitBaseState state)
                {
                        this.StopAllCoroutines();
                        this.currentState = state;
                        this.StartCoroutine(this.currentState.Enter(this, this.orbit));
                }
        }
}
