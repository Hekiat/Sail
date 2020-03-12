using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace sail
{
    public class BattleTileSelectionState : BattleBaseState
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

        protected override void AddListeners()
        {
            InputController.ClickEvent += OnMouseClick;
        }

        protected override void RemoveListeners()
        {
            InputController.ClickEvent -= OnMouseClick;
        }

        IEnumerator updateUnit()
        {
            var unit = owner.timelineController.getNextUnit();
            owner.hud.TimelineWidget.updateCharacters();

            yield return null;

            yield return new WaitForSeconds(2f);

            unit.Cooldown = Random.Range(5, 10);
            owner.hud.TimelineWidget.updateCharacters();

            owner.ChangeToState<BattleSetupActionState>();
        }

        void OnMouseClick(object sender, CustomEventArgs<Vector3> e)
        {
            throw new System.NotImplementedException();
        }
    }
}
