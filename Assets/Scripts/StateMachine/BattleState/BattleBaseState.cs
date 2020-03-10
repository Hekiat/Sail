using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class BattleBaseState : State
    {
        protected BattleFSM owner;
        public Board board { get { return owner.board; } }
        public List<Unit> units { get { return owner.units; } }

        protected virtual void Awake()
        {
            owner = GetComponent<BattleFSM>();
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}