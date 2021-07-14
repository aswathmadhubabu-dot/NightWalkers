using Assets.SimpleSteering.Scripts.Movement;
using Assets.SuperGoalie.Scripts.FSMs;
using System;
using UnityEngine;

namespace Assets.SuperGoalie.Scripts.Entities
{
    [RequireComponent(typeof(GoalKeeperFSM))]
    [RequireComponent(typeof(RPGMovement))]
    public class GoalKeeper : MonoBehaviour
    {
        [SerializeField] float _diveSpeed = 4f;

        [SerializeField] float _goalKeeping = 0.85f;
        
        float _height = 1.9f;


        [SerializeField] float _jumpDistance = 1;
        [SerializeField] float _jumpHeight = 0.5f;
        [SerializeField] float _reach = 0.5f;

        [SerializeField] float _tendGoalDistance = 3f;

        [SerializeField] float _tendGoalSpeed = 3f;

        [SerializeField] Animator _animator;


        [SerializeField] Ball _ball;

        [SerializeField] Goal _goal;


        [SerializeField] Transform _modelRoot;

        public Action OnHasNoBall;

        public Action OnHasBall;

        public Action OnPunchBall;

        public delegate void BallLaunched(float flightPower, float velocity, Vector3 initial, Vector3 target);

        public BallLaunched OnBallLaunched;

        public bool HasBall { get; set; }

        public float BallFlightTime { get; set; }

        public Vector3 BallHitTarget { get; set; }

        public Vector3 BallInitialPosition { get; internal set; }

        public GoalKeeperFSM FSM { get; set; }

        public RPGMovement RPGMovement { get; set; }

        private void Awake()
        {
            FSM = GetComponent<GoalKeeperFSM>();
            RPGMovement = GetComponent<RPGMovement>();
        }

        public bool IsBallWithChasingDistance()
        {
            return DistanceOfBallToGoal() <= 10f;
        }

        public bool IsBallWithThreateningDistance()
        {
            return DistanceOfBallToGoal() <= 20f;
        }

        public bool IsShotOnTarget()
        {
            return _goal.IsPositionWithinGoalMouthFrustrum(BallHitTarget);
        }

        public float DistanceOfBallToGoal()
        {
            return Vector3.Distance(_ball.transform.position, _goal.transform.position);
        }

        public void Instance_OnBallLaunched(float flightTime, float velocity, Vector3 initial, Vector3 target)
        {
            BallLaunched temp = OnBallLaunched;
            temp?.Invoke(flightTime, velocity, initial, target);
        }

        public Vector3 Position => transform.position;

        public float DiveReach => JumpDistance + Reach;

        public float DiveSpeed
        {
            get => _diveSpeed;

            set => _diveSpeed = value;
        }

        public float GoalKeeping
        {
            get => _goalKeeping;

            set => _goalKeeping = value;
        }

        public float JumpDistance
        {
            get => _jumpDistance;

            set => _jumpDistance = value;
        }

        public float JumpReach => Height + JumpHeight;

        public float Reach
        {
            get => _reach;

            set => _reach = value;
        }

        public float TendGoalDistance
        {
            get => _tendGoalDistance;

            set => _tendGoalDistance = value;
        }

        public float TendGoalSpeed
        {
            get => _tendGoalSpeed;

            set => _tendGoalSpeed = value;
        }

        public Animator Animator
        {
            get => _animator;

            set => _animator = value;
        }

        public Ball Ball
        {
            get => _ball;

            set => _ball = value;
        }

        public Goal Goal
        {
            get => _goal;

            set => _goal = value;
        }

        public float Height
        {
            get => _height;

            set => _height = value;
        }

        public float JumpHeight
        {
            get => _jumpHeight;

            set => _jumpHeight = value;
        }

        public Transform ModelRoot
        {
            get => _modelRoot;

            set => _modelRoot = value;
        }

        public float BallVelocity { get; internal set; }
    }
}