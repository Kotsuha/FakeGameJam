using UnityEngine;
using System.Collections;

[System.Serializable]
// Use this for initialization
// Update is called once per frame
// Get the input vector from keyboard or analog stick
// Get the length of the directon vector and then normalize it
// Dividing by the length is cheaper than normalizing when we already have the length anyway
// Make sure the length is no bigger than 1
// Make the input vector more sensitive towards the extremes and less sensitive in the middle
// This makes it easier to control slow speeds when using analog sticks
// Multiply the normalized direction vector by the modified length
// Apply the direction to the CharacterMotor
// Require a character controller to be attached to the same game object
[UnityEngine.RequireComponent(typeof(CharacterMotor))]
public partial class PlayerInputController : MonoBehaviour // Modified from Unity's FPSInputController
{
    public string HorizontalAxisName = "Horizontal";
    public string VerticalAxisName = "Vertical";
    public string JumpButtonName = "Jump";

    public bool inverseHorizontal;
    public bool inverseVertical;

    private CharacterMotor motor;
    public virtual void Awake()
    {
        this.motor = (CharacterMotor) this.GetComponent(typeof(CharacterMotor));
    }

    public virtual void Update()
    {
        float horizontalAxisValue = Input.GetAxis(HorizontalAxisName);
        float verticalAxisValue = Input.GetAxis(VerticalAxisName);
        if (inverseHorizontal)
            horizontalAxisValue *= -1;
        if (inverseVertical)
            verticalAxisValue *= -1;
        Vector3 directionVector = new Vector3(horizontalAxisValue, 0, verticalAxisValue);
        if (directionVector != Vector3.zero)
        {
            float directionLength = directionVector.magnitude;
            directionVector = directionVector / directionLength;
            directionLength = Mathf.Min(1, directionLength);
            directionLength = directionLength * directionLength;
            directionVector = directionVector * directionLength;
        }
        this.motor.inputMoveDirection = this.transform.rotation * directionVector;
        this.motor.inputJump = Input.GetButton(JumpButtonName);
    }

}