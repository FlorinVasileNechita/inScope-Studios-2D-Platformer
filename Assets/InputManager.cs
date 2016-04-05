using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
    Command throwKnife;
    Command meleeAttack;
    Command slide;
    Command jump;

    public InputManager() {
        throwKnife = new ThrowKnifeAction();
        meleeAttack = new AttackAction();
        slide = new SlideAction();
        jump = new JumpAction();
    }

	void Update () {
	    if (Input.GetKeyDown(KeyCode.Mouse1)) {
            throwKnife.Execute();
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift)) {
            slide.Execute();
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            jump.Execute();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0)) {
            meleeAttack.Execute();
        }
    }
}
