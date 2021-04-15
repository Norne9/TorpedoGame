using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour {
    public float Horizontal => Input.GetAxis("Horizontal");
    public float Vertical => Input.GetAxis("Vertical");
    public bool Fire => Input.GetButton("Fire1");
    public bool SwapWeapon => Input.mouseScrollDelta.sqrMagnitude > 0;
}
