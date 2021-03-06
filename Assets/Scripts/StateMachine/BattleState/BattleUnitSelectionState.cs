﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class BattleUnitSelectionState : BattleBaseState
    {
        public override void Enter()
        {
            base.Enter();
            StartCoroutine(updateUnit());
        }

        public override void Exit()
        {
            base.Exit();
        }

        IEnumerator updateUnit()
        {
            var unit = owner.TimelineController.getNextEnemy();
            //Debug.Log(unit.name);

            owner.hud.TimelineWidget.updateCharacters();

            yield return null;

            yield return new WaitForSeconds(1f);

            unit.Cooldown = Random.Range(5, 10);
            owner.hud.TimelineWidget.updateCharacters();

            owner.SelectedEnemy = unit;

            owner.ChangeToState<BattleIdleState>();
        }
    }
}
