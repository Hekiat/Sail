using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace sail
{
    public class MotionController : MonoBehaviour
    {
        public MotionState CurrentState { get; private set; } = null;

        Animator Animator = null;

        const int Layer = 0;

        void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        void Start()
        {
            //var Animator = GetComponent<Animator>();

            //RuntimeAnimatorController rac = anim.runtimeAnimatorController;
            //AnimationClip[] clips = rac.animationClips;
            //
            //var anim_clip_count = clips.Length;
            //
            //var animation_names = new string[anim_clip_count];
            //
            //// store names in a table
            //int i = 0;
            //foreach (AnimationClip clip in clips)
            //{
            //    Debug.Log("" + clip.name);
            //    animation_names[i] = clip.name;
            //    i++;
            //}
            //
            //var a = anim.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
            //
            ////for (int i = 0; i < sm.stateCount; i++)
            //foreach (var state in a.layers[0].stateMachine.states)
            //{
            //    Debug.Log(string.Format("State: {0}", state.state.name));
            //}
        }

        void Update()
        {

        }

        // Blend Time in sec
        public void requestMotion(MotionState state, float blendTime = 0.25f)
        {
            Assert.IsTrue(Animator.HasState(0, state.Hash), "@MotionController: motion State not found: " + state.Name);

            Animator.CrossFade(state.Hash, blendTime);
            CurrentState = state;
        }

        public float currentStateNormalizedTime()
        {
            // TODO FIX ACTION FLOW 1st FRAME
            //if (Animator.GetCurrentAnimatorStateInfo(Layer).shortNameHash != CurrentState.Hash && isTransitioning() == false)
            //{
            //    return 0f;
            //}

            return currentStateInfo().normalizedTime;
        }

        private AnimatorStateInfo currentStateInfo()
        {
            if (isTransitioning())
            {
                return Animator.GetNextAnimatorStateInfo(Layer);
            }

            return Animator.GetCurrentAnimatorStateInfo(Layer);
        }

        private bool isTransitioning()
        {
            return Animator.GetNextAnimatorClipInfoCount(Layer) != 0;
        }

        //Debug.Log("NExt STate: " + Animator.GetNextAnimatorStateInfo(0).shortNameHash);
        //
        //for (int i = 0; i < 2; ++i)
        //{
        //    foreach (var clipInfo in Animator.GetCurrentAnimatorClipInfo(i))
        //    {
        //        Debug.Log(clipInfo.clip.name + ": " + clipInfo.weight);
        //    }
        //}
        //Debug.Log("");
    }
}
