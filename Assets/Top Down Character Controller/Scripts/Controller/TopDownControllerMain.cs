using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[DisallowMultipleComponent]
[AddComponentMenu("Top Down RPG/Controller/Third Person Controller")]
public class TopDownControllerMain : MonoBehaviour {

    public float tdcm_turningSpeed = 560;
    public float tdcm_movingSpeed = 1f;
    public float tdcm_animPlaySpeed = 1f;

    public Animator tdcm_animator;
    public Rigidbody tdcm_rigidbody;
    public CapsuleCollider tdcm_Capsule;
    public NavMeshAgent tdcm_NavMeshAgent;
    public Camera tdcm_Camera;
    public WindZone vegetationMoveWindZone;

    public TopDownInputManager tdcm_InputManager;
    public float tdcm_TurningAmount;
    public float tdcm_MoveAmount;

    public UnityEvent onDeadEvent;

    void Start() {
        tdcm_animator = GetComponent<Animator>();
        tdcm_rigidbody = GetComponent<Rigidbody>();
        tdcm_Capsule = GetComponent<CapsuleCollider>();
        tdcm_NavMeshAgent = GetComponent<NavMeshAgent>();
        tdcm_InputManager = TopDownInputManager.instance;
        if (GameObject.FindGameObjectWithTag("MainCamera")) {
            tdcm_Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        else if (GameObject.FindObjectOfType<Camera>()) {
            tdcm_Camera = GameObject.FindObjectOfType<Camera>();
        }
        tdcm_rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        if(GetComponentInChildren<WindZone>()) {
            vegetationMoveWindZone = GetComponentInChildren<WindZone>();
        }
    }

    public void TDCC_MoveCharacter(Vector3 move) {

        if (move.magnitude > 1f) {
            move.Normalize();
        }

        move = transform.InverseTransformDirection(move);

        tdcm_TurningAmount = Mathf.Atan2(move.x, move.z);

        tdcm_MoveAmount = move.z;

        TDCM_CharacterTurnExtra();
        
        TDCM_AnimatorUpdate(move);
    }

    void TDCM_AnimatorUpdate(Vector3 move) {
        tdcm_animator.SetFloat("Forward", tdcm_MoveAmount, 0.1f, Time.deltaTime);
        tdcm_animator.SetFloat("Turn", tdcm_TurningAmount, 0.1f, Time.deltaTime);

        if (move.magnitude > 0) {
            tdcm_animator.speed = tdcm_animPlaySpeed;
        }
        else {
            tdcm_animator.speed = 1;
        }
    }

    void TDCM_CharacterTurnExtra() {
        transform.Rotate(0, tdcm_TurningAmount * tdcm_turningSpeed * Time.deltaTime, 0);
    }

    public void OnAnimatorMove() {
        if (Time.deltaTime > 0 && tdcm_rigidbody != null) {
            Vector3 v = (tdcm_animator.deltaPosition * tdcm_movingSpeed) / Time.deltaTime;
            
            v.y = tdcm_rigidbody.velocity.y;
            tdcm_rigidbody.velocity = v;
        }
    }
}