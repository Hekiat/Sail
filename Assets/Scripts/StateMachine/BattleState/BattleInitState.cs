using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace sail
{
    public class BattleInitState : BattleBaseState
    {
        public override void Enter()
        {
            base.Enter();
            StartCoroutine(Init());
        }

        IEnumerator Init()
        {
            board.Generate();

            if (owner.HUDPrefab)
            {
                var go = Instantiate(owner.HUDPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
                owner.hud = go.GetComponent<BattleHUD>();
            }

            yield return null;

            owner.ChangeToState<BattleSetupActionState>();
        }
    }
}
