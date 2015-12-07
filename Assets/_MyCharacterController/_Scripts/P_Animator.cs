using UnityEngine;
using System.Collections;

public class P_Animator : MonoBehaviour 
{
  public enum Direction
  {
    Stationary, Forward, Backward, Left, Right,
      LeftForward, RightForward, LeftBackward, RightBackward
	}

  public static P_Animator S;

  public Direction MoveDirection { get; set; }

  void Awake()
  {
    S = this;
  }

  void Update()
  {

  }

  public void DetermineCurrentMoveDirection()
  {
    var forward = false;
    var backward = false;
    var left = false;
    var right = false;

    if (P_Motor.S.MoveVector.z > 0)
      forward = true;
    if (P_Motor.S.MoveVector.z < 0)
      backward = true;
    if (P_Motor.S.MoveVector.x > 0)
      right = true;
    if (P_Motor.S.MoveVector.x < 0)
      left = true;

    if (forward)
    {
      if (left)
        MoveDirection = Direction.LeftForward;
      else if (right)
        MoveDirection = Direction.RightForward;
      else
        MoveDirection = Direction.Forward;
    }
    else if (backward)
    {
      if (left)
        MoveDirection = Direction.LeftBackward;
      else if (right)
        MoveDirection = Direction.RightBackward;
      else
        MoveDirection = Direction.Backward;
    }
    else if (left)
      MoveDirection = Direction.Left;
    else if (right)
      MoveDirection = Direction.Right;
    else 
      MoveDirection = Direction.Stationary;

  } // End of DetermineCurrentMoveDirection()

}
