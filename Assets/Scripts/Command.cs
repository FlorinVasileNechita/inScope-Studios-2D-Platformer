using UnityEngine;
using System.Collections;
using System;

public abstract class Command {
    public abstract void Execute();
}

// Commands directly access Player instance.
// This is bad.  
public class ThrowKnifeAction : Command {
    public override void Execute() {
        Player.Instance.mAnimator.SetTrigger("throw");
    }
}

public class JumpAction : Command {
    public override void Execute() {
        Player.Instance.mAnimator.SetTrigger("jump");
    }
}

public class SlideAction : Command {
    public override void Execute() {
        Player.Instance.mAnimator.SetTrigger("slide");
    }
}

public class AttackAction : Command {
    public override void Execute() {
        Player.Instance.mAnimator.SetTrigger("attack");
    }
}