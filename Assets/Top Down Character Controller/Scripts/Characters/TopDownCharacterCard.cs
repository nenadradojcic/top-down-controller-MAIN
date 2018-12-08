using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
public class TopDownCharacterCard : MonoBehaviour {
    // AI
    public string aiName;
    public TopDownVoiceSet aiVoiceSet;
    public SphereCollider aiVision;
    public float aiFieldOfView = 160f;
    public float aiDetectRadius = 5f;

    //PLAYER
    public TopDownCharacter character;
    public int td_characterIndex = -1;

    public new string name;

    public float health = 10f;
    public float maxHealth = 10f;

    public float energy = 10f;
    public float maxEnergy = 10f;

    public int strength = 2;
    public int dexterity = 2;
    public int constitution = 2;
    public int willpower = 2;
    public int attributePoints = 0;

    public int armorPoints = 0;
    public int damagePoints = 0;

    public int level = 1;
    public int experience = 0;
    public int experienceToLevel = 200;
    public int skillPoints = 0;

    //PLAYER WHEN NOT ACTIVE CHARACTER
    public bool activePlayerAi;
    public Transform activeFocus;
    public Transform enemyFocus;
    public float distanceToActive;
    public float distanceToActivesFocus;

    public GameObject inventoryCamera;
    public TopDownCharacterEquipmentSlots characterInventory;
    public TopDownControllerMain main;
    public TopDownControllerInteract interact;
    public TopDownEquipmentManager equipmentManager;
    public TopDownCharacterManager characterManager;
    public TopDownUINpcNameBar npcBar;

    public bool mouseOver;
    public Camera mainCamera;

    public bool placeBarOverHead = false;
    public Transform nameBarPosition;

    private void Start() {

        if (character != null) {
            name = character.name;
            health = character.health;
            maxHealth = character.health;
            energy = character.energy;
            maxEnergy = character.energy;
        }

        main = GetComponent<TopDownControllerMain>();
        interact = GetComponent<TopDownControllerInteract>();
        equipmentManager = GetComponent<TopDownEquipmentManager>();
        if (TopDownCharacterManager.instance != null) {
            characterManager = TopDownCharacterManager.instance;
        }
        if (TopDownUIManager.instance != null) {
            npcBar = TopDownUIManager.instance.npcWorldName.GetComponent<TopDownUINpcNameBar>();
            placeBarOverHead = npcBar.placeBarOverHead;
        }

        if (GameObject.FindGameObjectWithTag("MainCamera")) {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        else if (GameObject.FindObjectOfType<Camera>()) {
            mainCamera = GameObject.FindObjectOfType<Camera>();
        }

        if (GetComponent<TopDownAI>()) {
            maxHealth = health;
            GetComponent<TopDownAI>().name = aiName;
            GetComponent<TopDownAI>().voiceSet = aiVoiceSet;
            GetComponent<TopDownAI>().visionCollider = aiVision;
            GetComponent<TopDownAI>().detectRadius = aiDetectRadius;
            GetComponent<TopDownAI>().fieldOfViewAngle = aiFieldOfView;
        }

        if(GetComponent<TopDownControllerInteract>()) {

            int layerInt = gameObject.layer;

            if (inventoryCamera != null) {
                inventoryCamera.GetComponent<Camera>().cullingMask = 1 << layerInt;

                if (TopDownCharacterManager.instance != null) {
                    if (TopDownCharacterManager.instance.activeCharacter != gameObject) {
                        inventoryCamera.SetActive(false);
                    }
                }
            }
        }

        Transform[] allChildren = GetComponentsInChildren<Transform>();
        for(int i = 0; i < allChildren.Length; i ++) {
            if (allChildren[i].name.Contains("head") || allChildren[i].name.Contains("HEAD") || allChildren[i].name.Contains("Head")) {
                nameBarPosition = allChildren[i];
            }
        }
    }

    public void SetAiActive() {
        if (main != null) {
            activePlayerAi = true;
        }
        else {
            Debug.LogWarningFormat("<b>TopDownPlayableCharacter</b> component should be on the same game object where is <b>TopDownControllerMain</b> component.");
        }
    }

    public void DeactivateAi() {
        if (main != null) {
            activePlayerAi = false;
            activeFocus = null;
            enemyFocus = null;
            distanceToActive = 0f;
        }
        else {
            Debug.LogWarningFormat("<b>TopDownPlayableCharacter</b> component should be on the same game object where is <b>TopDownControllerMain</b> component.");
        }
    }

    public void Update() {
        if (activePlayerAi == true) {
            if(main.tdcm_NavMeshAgent != null) {
                if(activeFocus != characterManager.activeCharacter) {
                    activeFocus = characterManager.activeCharacter.transform;
                }

                if(activeFocus != null) {
                    distanceToActive = Vector3.Distance(transform.position, activeFocus.position);

                    if (activeFocus.GetComponent<Animator>().GetBool("Attacking") == false) {
                        if (distanceToActive > 3f) {
                            main.tdcm_NavMeshAgent.SetDestination(activeFocus.position);
                            main.TDCC_MoveCharacter(main.tdcm_NavMeshAgent.desiredVelocity);

                        }
                        else {
                            main.tdcm_NavMeshAgent.SetDestination(transform.position);
                            main.TDCC_MoveCharacter(Vector3.zero);
                        }
                    }
                    else {
                        if(activeFocus.GetComponent<TopDownControllerInteract>().focusedTarget != null) {
                            if (enemyFocus == null) {
                                enemyFocus = activeFocus.GetComponent<TopDownControllerInteract>().focusedTarget;
                            }

                            //float distanceToActivesFocus = Vector3.Distance(transform.position, enemyFocus.position);

                            if (main.tdcm_NavMeshAgent.remainingDistance > interact.enemyStopDistance) {
                                main.TDCC_MoveCharacter(main.tdcm_NavMeshAgent.desiredVelocity);
                                if (main.vegetationMoveWindZone != null) {
                                    main.vegetationMoveWindZone.gameObject.SetActive(true);
                                }

                                main.tdcm_NavMeshAgent.SetDestination(enemyFocus.position);

                                main.tdcm_animator.SetBool("TargetInFront", false);
                            }
                            else {
                                main.TDCC_MoveCharacter(Vector3.zero);
                                main.tdcm_NavMeshAgent.SetDestination(transform.position);

                                if (main.vegetationMoveWindZone != null) {
                                    main.vegetationMoveWindZone.gameObject.SetActive(false);
                                }

                                //We needed to put this as sometimes our agent wont rotate all the way toward target
                                Quaternion targetRotation = Quaternion.LookRotation(enemyFocus.position - transform.position);
                                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * interact.faceEnemyRotSpeed);

                                //COMBAT
                                if (enemyFocus.GetComponent<TopDownCharacterCard>() && enemyFocus.GetComponent<TopDownCharacterCard>().IsDead() == false) {
                                    //here we only attack with melee
                                    int i = (int)equipmentManager.weaponTypeUsed;
                                    int h = (int)equipmentManager.weaponHoldingType;

                                    main.tdcm_animator.SetBool("TargetInFront", true);
                                    main.tdcm_animator.SetFloat("WeaponType", (float)i);
                                    main.tdcm_animator.SetFloat("WeaponHoldingType", (float)h);
                                    main.tdcm_animator.SetBool("Attacking", true);
                                }
                                else {
                                    main.tdcm_animator.SetBool("TargetInFront", false);
                                    main.tdcm_animator.SetBool("Attacking", false);
                                }
                            }
                        }
                        else {
                            enemyFocus = null;
                        }
                    }
                }
            }
        }

        if (health < 0f) {
            maxHealth = 0f;
            Die();
        }
        if (health > maxHealth) {
            health = maxHealth;
        }
        if (TopDownUIManager.instance != null) {
            if (placeBarOverHead != npcBar.placeBarOverHead) {
                placeBarOverHead = npcBar.placeBarOverHead;
            }

            if (mouseOver == true) {
                if (placeBarOverHead == false) {
                    Vector2 tmp = mainCamera.WorldToScreenPoint(transform.position);
                    Vector2 namePos = new Vector3(tmp.x, tmp.y + (npcBar.yOffset * npcBar.screenY));

                    npcBar.transform.position = namePos;
                }
                else {
                    Vector2 tmp = mainCamera.WorldToScreenPoint(nameBarPosition.position);
                    Vector2 namePos = new Vector3(tmp.x, tmp.y + (npcBar.yOffsetOverHead * npcBar.screenY));
                    npcBar.transform.position = namePos;
                }
            }
        }
    }

    private void Die() {
        if (GetComponent<TopDownAI>()) {
            TopDownAI ai = GetComponent<TopDownAI>();
            if (ai.voiceSet != null) {
                Instantiate(ai.voiceSet.deathVoice, transform.position, Quaternion.identity);
            }
        }

        mouseOver = false;
        npcBar.nameText.text = string.Empty;
        npcBar.healthBar.fillAmount = 0f;
        npcBar.energyBar.fillAmount = 0f;
        npcBar.portraitImage.enabled = false;

        npcBar.transform.position = new Vector2(-100f, 0f);

        main.tdcm_animator.SetLayerWeight(0, 0f);
        main.tdcm_animator.SetLayerWeight(1, 0f);

        //tdcm_animator.SetBool("Attacking", false);
        //tdcm_animator.SetBool("TargetInFront", false);

        main.tdcm_animator.SetBool("Dead", true);

        //gameObject.tag = "Dead";

        main.onDeadEvent.Invoke();

        Destroy(main.tdcm_rigidbody);
        Destroy(GetComponent<TopDownAI>());
        Destroy(GetComponent<TopDownEquipmentManager>());
        Destroy(this);

        return;
    }

    public bool IsDead() {
        if (health == 0f) {
            return true;
        }

        return false;
    }

    public void OnMouseOver() {
        if (TopDownUIManager.instance != null) {
            mouseOver = true;
            if (gameObject.tag == "Enemy") {
                npcBar.nameText.text = aiName;
                npcBar.healthBar.enabled = true;
                npcBar.healthBar.fillAmount = health / maxHealth;
                npcBar.energyBar.enabled = false;
                npcBar.energyBar.fillAmount = 0f;
                npcBar.portraitImage.enabled = false;
                npcBar.portraitImage.sprite = null;
            }
            if (gameObject.tag == "Player") {
                if (mainCamera.GetComponent<TopDownCameraBasic>()) {
                    if (mainCamera.GetComponent<TopDownCameraBasic>().cameraType == CameraType.CharacterCamera) {
                        if (characterManager.activeCharacter != gameObject) {
                            npcBar.nameText.text = name;
                            npcBar.healthBar.enabled = true;
                            npcBar.healthBar.fillAmount = health / maxHealth;
                            npcBar.energyBar.enabled = true;
                            npcBar.energyBar.fillAmount = energy / maxEnergy;
                            npcBar.portraitImage.enabled = true;
                            npcBar.portraitImage.sprite = character.icon;
                        }
                    }
                    else {
                        npcBar.nameText.text = name;
                        npcBar.healthBar.enabled = true;
                        npcBar.healthBar.fillAmount = health / maxHealth;
                        npcBar.energyBar.enabled = true;
                        npcBar.energyBar.fillAmount = energy / maxEnergy;
                        npcBar.portraitImage.enabled = true;
                        npcBar.portraitImage.sprite = character.icon;
                    }
                }
            }
            if (gameObject.tag == "NPC") {
                npcBar.nameText.text = name;
                npcBar.healthBar.enabled = false;
                npcBar.healthBar.fillAmount = 0f;
                npcBar.energyBar.enabled = false;
                npcBar.energyBar.fillAmount = 0f;
                npcBar.portraitImage.enabled = false;
                npcBar.portraitImage.sprite = null;
            }
            //Debug.LogFormat("<b>" + gameObject.name + "</b> called OnMouseOver in <i>TopDownCharacterCard.cs</i>.");
        }
    }

    public void OnMouseExit() {
        if (TopDownUIManager.instance != null) {
            mouseOver = false;
            npcBar.nameText.text = string.Empty;
            npcBar.healthBar.enabled = false;
            npcBar.healthBar.fillAmount = 0f;
            npcBar.energyBar.enabled = false;
            npcBar.energyBar.fillAmount = 0f;
            npcBar.portraitImage.enabled = false;
            npcBar.portraitImage.sprite = null;

            npcBar.transform.position = new Vector2(-100f, 0f);

            //Debug.LogFormat("<b>" + gameObject.name + "</b> called OnMouseExit in <i>TopDownCharacterCard.cs</i>.");
        }
    }
}