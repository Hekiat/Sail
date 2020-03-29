using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sail.animation;


namespace sail
{
    public static class ActionFunctionLibrary
    {
        public static IEnumerator moveTo(GameObject go, TileCoord goal)
        {
            var goalPosition = BattleFSM.Instance.board.getTile(goal).transform.position;
            goalPosition.y = go.transform.position.y;

            var tweener = go.transform.MoveTo(goalPosition);

            yield return new WaitUntil(() => tweener == null);

            //while (tweener)
            //{
            //    yield return null;
            //}
        }
    }
}
