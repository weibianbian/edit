using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Experimental.GraphView;

namespace CopyBT
{
    public class AbigailBrain : Brain
    {
        protected override void OnStart()
        {
            CopyBTBehaviourNode watch_game = BehaviourNodeExtension.WhileNode(ShouldWatchMinigame, "Watching Game",
                new PriorityNode(new List<CopyBTBehaviourNode>()
                {
                    new Follow(""),
                }, 0.25f)
                );

            CopyBTBehaviourNode dance = BehaviourNodeExtension.WhileNode(ShouldDanceParty, "Dance Party",
                new PriorityNode(new List<CopyBTBehaviourNode>()
                {
                    new ActionNode("Action",()=>{ UnityEngine.Debug.Log("DanceParty"); })
                }, 0.25f)
                ); ;

            CopyBTBehaviourNode defensive_mode = BehaviourNodeExtension.WhileNode(IsDefensive, "DefensiveMove",
                new PriorityNode(new List<CopyBTBehaviourNode>()
                {
                    dance,
                    watch_game,

                    BehaviourNodeExtension.WhileNode(DefensiveCanFight,"CanFight",new ChaseAndAttack("")),
                }));
            CopyBTBehaviourNode aggressive_mode = new PriorityNode(new List<CopyBTBehaviourNode>()
                {
                    dance,
                    watch_game,

                   BehaviourNodeExtension.WhileNode(DefensiveCanFight,"CanFight",new ChaseAndAttack("")),
                });
            CopyBTBehaviourNode root = new PriorityNode(new List<CopyBTBehaviourNode>()
            {
                defensive_mode,
                aggressive_mode
            }); ;

            bt = new BehaviourTree(root);
        }

        public bool ShouldWatchMinigame()
        {
            return true;
        }
        public bool IsDefensive()
        {
            return true;
        }
        public bool ShouldDanceParty()
        {
            return true;
        }
        public bool DefensiveCanFight()
        {
            return true;
        }
    }
}

