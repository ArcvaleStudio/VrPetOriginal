﻿using UnityEngine;
using System.Collections;


namespace MalbersAnimations
{
    /// <summary>
    /// Is Used to execute random animations in a State Machine 
    /// </summary>
    public class RandomBehavior : StateMachineBehaviour
    {
        public string Parameter = "IDInt";
        public int Range;

        override public void OnStateEnter(Animator animator,AnimatorStateInfo info, int stateMachinePathHash)
        {
            int newParam = Random.Range(1, Range + 1);
            animator.SetInteger(Parameter, newParam);

            Animal animal = animator.GetComponent<Animal>();
            if (animal) animal.SetIntID(newParam);
        }
    }
}