using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyRobotAIState
{
    public abstract class StateMachine
    {
        public EnemyRobotAI ai;
        protected StateMachine(EnemyRobotAI ai) => this.ai = ai;
        public abstract void Start();
        public abstract void Update();

        public abstract void SwitchState(BaseState state);
    }

    public class MovementStateMachine : StateMachine
    {
        public MovementBaseState currentState;

        public MovementStateMachine(EnemyRobotAI ai) : base(ai) { }

        public override void Start()
        {
            SwitchState(new MovementStopState());
        }

        public override void Update()
        {
            currentState.Update(this);
        }

        public override void SwitchState(BaseState state)
        {
            currentState = (MovementBaseState)state;
            state.Start(this);
        }
    }

    public class AttackStateMachine : StateMachine
    {
        public AttackBaseState currentState;

        public AttackStateMachine(EnemyRobotAI ai) : base(ai) { }

        public override void Start()
        {
            SwitchState(new AttackIdleState());
        }

        public override void Update()
        {
            currentState.Update(this);
        }

        public override void SwitchState(BaseState state)
        {
            currentState = (AttackBaseState)state;
            state.Start(this);
        }
    }
}
