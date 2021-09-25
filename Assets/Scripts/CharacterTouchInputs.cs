using CMF;
using UnityEngine;

public class CharacterTouchInputs : CharacterInput {
    [SerializeField] private Joystick joystick;

    public override float GetHorizontalMovementInput() {
        return joystick.Horizontal;
    }

    public override float GetVerticalMovementInput() {
        return joystick.Vertical;
    }
}
