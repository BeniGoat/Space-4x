using UnityEngine;

namespace Space4x.GameManagement.Managers
{
        public class InputManager : MonoBehaviour
        {
                [SerializeField] private KeyCode endTurnKey;
                public bool IsEndTurnButtonPressed()
                {
                        return Input.GetKeyDown(this.endTurnKey);
                }
        }
}
