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
            BehaviourNode watch_game = WhileNode(ShouldWatchMinigame, "Watching Game",
                new PriorityNode(new List<BehaviourNode>()
                {
                    new Follow(""),
                }, 0.25f)
                );

            BehaviourNode dance = WhileNode(ShouldDanceParty, "Dance Party",
                new PriorityNode(new List<BehaviourNode>()
                {
                    new ActionNode("Action",()=>{ UnityEngine.Debug.Log("DanceParty"); })
                }, 0.25f)
                ); ;

            BehaviourNode defensive_mode = WhileNode(IsDefensive, "DefensiveMove",
                new PriorityNode(new List<BehaviourNode>()
                {
                    dance,
                    watch_game,

                    WhileNode(DefensiveCanFight,"CanFight",new ChaseAndAttack("")),
                }));
            BehaviourNode  aggressive_mode= new PriorityNode(new List<BehaviourNode>()
                {
                    dance,
                    watch_game,

                    WhileNode(DefensiveCanFight,"CanFight",new ChaseAndAttack("")),
                });
            BehaviourNode root = new PriorityNode(new List<BehaviourNode>()
            {
                defensive_mode,
                aggressive_mode
            }); ;


        }
        public BehaviourNode WhileNode(Func<bool> cond, string name, BehaviourNode node)
        {
            return new ParallelNode(new List<BehaviourNode>()
            {
                new ConditionNode(cond),
                node
            });
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

