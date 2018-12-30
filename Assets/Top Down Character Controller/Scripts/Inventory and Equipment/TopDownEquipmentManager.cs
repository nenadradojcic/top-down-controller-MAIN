using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum WeaponType {
    NoWeapon = 0,
    Melee = 1,
    Ranged = 2,
}

public enum WeaponHoldingType {
    None = 0,
    OneHanded = 1,
    TwoHanded = 2,
}

public enum CharacterEquipementType {
    None = -1,
    InstantiateMesh = 0,
    ReplaceMesh = 1,
    EnableMesh = 2,
}

[DisallowMultipleComponent]
public class TopDownEquipmentManager : MonoBehaviour {

    public WeaponType weaponTypeUsed;
    public WeaponHoldingType weaponHoldingType;
    public TopDownItemObject[] currentEquipment;

    public int armorPointsValue;
    public int damagePointsValue;

    public static TopDownEquipmentManager instance;

    public Transform weaponMountPoint;
    public Transform shieldMountPoint;

    public Transform weaponHolsterMountPoint;
    public Transform shieldHolsterMountPoint;

    public TopDownControllerMain tcc_Main;
    public TopDownControllerInteract tcc_Interact;
    public TopDownCharacterCard td_characterCard;

    [Tooltip("Equipment type means in which way will we equip items on our character.\n\n1. Instantiate Mesh will instantiate gameObject at runtime as child of set transform.\n" +
        "\n2. Replace Mesh will replace mesh in SkinnedMesh Component. BEST USED IF ALL MESHES SHARE SAME MATERIAL.\n" +
        "\n3. Enable Mesh will search for specified mesh name already part of character model to enable/disable it.\n")]
    public CharacterEquipementType characterEquipmentType;

    //Used for Instantiate Mesh equipment type
    public Transform bodyTransform;
    public Transform headTransform;
    public Transform neckTransform;
    public Transform handsTransform;
    public Transform leggsTransform;
    public Transform helmTransform;
    //Used for Replace Mesh equipment type
    public Mesh defaultBodyMesh; //We need to set this up so that we can revert to it back when unequiping armor.
    public Mesh defaultHandsMesh;
    public Mesh defaultLeggsMesh;
    public SkinnedMeshRenderer bodyMesh;
    public SkinnedMeshRenderer headMesh;
    public SkinnedMeshRenderer neckMesh;
    public SkinnedMeshRenderer handsMesh;
    public SkinnedMeshRenderer leggsMesh;
    public SkinnedMeshRenderer helmMesh;
    //Used fot Enable Mesh equipment type
    public SkinnedMeshRenderer[] itemsOnCharacter;

    private void Awake() {
        instance = this;
        tcc_Main = GetComponent<TopDownControllerMain>();
        tcc_Interact = GetComponent<TopDownControllerInteract>();
        td_characterCard = GetComponent<TopDownCharacterCard>();
    }

    private void Start() {

        int numSlots = System.Enum.GetNames(typeof(ItemType)).Length;

        if (currentEquipment.Length != numSlots) {
            currentEquipment = new TopDownItemObject[numSlots];
        }

        if (gameObject.tag == "Enemy") { //If this is not player, it must be ai so we must set equipment
            if (currentEquipment[2] != null) {
                SetAIEquipment();
            }
        }
        for (int i = 0; i < itemsOnCharacter.Length; i++) {
            itemsOnCharacter[i].enabled = false;
        }
    }

    public void EquipItem(TopDownItemObject item) {
        int slotIndex = (int)item.itemType;

        if (currentEquipment[slotIndex] != null) {
            for (int i = 0; i < TopDownUIInventory.instance.slots.Length; i++) {
                if (TopDownUIInventory.instance.slots[i].itemInSlot == currentEquipment[slotIndex]) {
                    //TopDownUIInventory.instance.slots[i].GetComponent<Image>().color = TopDownUIInventory.instance.slots[i].normalSlotColor;
                }
            }
        }

        currentEquipment[slotIndex] = item;

        //Instantiate weapon model as weapon mount points child

        if (item.itemType == ItemType.Weapon) {
            if (item.weaponType == WeaponType.Melee) {
                if (item.itemGameObject != null) {
                    GameObject itemGo = Instantiate(item.itemGameObject) as GameObject;
                    for (int i = 0; i < itemGo.transform.childCount; i++) {
                        itemGo.transform.GetChild(i).gameObject.layer = gameObject.layer;
                        //print(itemGo.transform.GetChild(i).gameObject.name + " new layer is " + gameObject.layer);
                    }
                    itemGo.transform.SetParent(weaponHolsterMountPoint);
                    itemGo.transform.localPosition = Vector3.zero;
                    itemGo.transform.localEulerAngles = Vector3.zero;
                }
            }
            else if(item.weaponType == WeaponType.Ranged) {
                if (item.itemGameObject != null) {
                    GameObject itemGo = Instantiate(item.itemGameObject) as GameObject;
                    for (int i = 0; i < itemGo.transform.childCount; i++) {
                        itemGo.transform.GetChild(i).gameObject.layer = gameObject.layer;
                    }
                    itemGo.transform.SetParent(shieldHolsterMountPoint);
                    itemGo.transform.localPosition = Vector3.zero;
                    itemGo.transform.localEulerAngles = Vector3.zero;
                }
            }
            weaponTypeUsed = item.weaponType;
            weaponHoldingType = item.weaponHoldingType;
        }
        else if (item.itemType == ItemType.Shield) {
            if (item.itemGameObject != null) {
                GameObject itemGo = Instantiate(item.itemGameObject) as GameObject;
                for (int i = 0; i < itemGo.transform.childCount; i++) {
                    itemGo.transform.GetChild(i).gameObject.layer = gameObject.layer;
                    //print(itemGo.transform.GetChild(i).gameObject.name + " new layer is " + gameObject.layer);
                }
                itemGo.transform.SetParent(shieldHolsterMountPoint);
                itemGo.transform.localPosition = Vector3.zero;
                itemGo.transform.localEulerAngles = Vector3.zero;
            }
        }
        else {
            if (characterEquipmentType == CharacterEquipementType.EnableMesh) {
                for (int i = 0; i < itemsOnCharacter.Length; i++) {
                    if (item.itemSkinnedMeshName == itemsOnCharacter[i].name) {
                        itemsOnCharacter[i].enabled = true;
                    }
                }
            }
            else if (characterEquipmentType == CharacterEquipementType.ReplaceMesh) {
                if(item.itemType == ItemType.Chest) {
                    bodyMesh.sharedMesh = item.itemMesh;
                }
                else if (item.itemType == ItemType.Head) {
                    helmMesh.sharedMesh = item.itemMesh;
                }
                else if (item.itemType == ItemType.Legs) {
                    leggsMesh.sharedMesh = item.itemMesh;
                }
                else if (item.itemType == ItemType.Hands) {
                    handsMesh.sharedMesh = item.itemMesh;
                }
                else if (item.itemType == ItemType.Neck) {
                    neckMesh.sharedMesh = item.itemMesh;
                }
            }
        }

        for (int i = 0; i < TopDownUIInventory.instance.slots.Length; i++) {
            if (TopDownUIInventory.instance.slots[i].itemInSlot == item) {
                //TopDownUIInventory.instance.slots[i].GetComponent<Image>().color = TopDownUIInventory.instance.slots[i].equipedSlotColor;
            }
        }

        if (TopDownAudioManager.instance.inventoryItemUseAudio != null) {
            Instantiate(TopDownAudioManager.instance.inventoryItemUseAudio, Vector3.zero, Quaternion.identity);
        }

        AddPointModifiers(item);
        if (TopDownUIManager.instance.characterPortraitType == CharacterPortraitType.Runtime) {
            GetComponent<TopDownCharacterCard>().UpdatePortrait();
        }
    }

    public void UnequipItem(TopDownItemObject item) {

        if (TopDownAudioManager.instance.inventoryItemUseAudio != null) {
            Instantiate(TopDownAudioManager.instance.inventoryItemUseAudio, Vector3.zero, Quaternion.identity);
        }

        RemovePointModifiers(item);

        int slotIndex = (int)item.itemType;

        currentEquipment[slotIndex] = null;
        weaponTypeUsed = WeaponType.NoWeapon;
        weaponHoldingType = WeaponHoldingType.None;

        if (item.itemType == ItemType.Weapon) {
            if (item.weaponType == WeaponType.Melee) {
                if (weaponHolsterMountPoint.childCount > 0) {
                    foreach (Transform itemGo in weaponHolsterMountPoint) {
                        Destroy(itemGo.gameObject);
                    }
                }
            }
            else if(item.weaponType == WeaponType.Ranged) {
                if (shieldHolsterMountPoint.childCount > 0) {
                    foreach (Transform itemGo in shieldHolsterMountPoint) {
                        Destroy(itemGo.gameObject);
                    }
                }
            }
        }
        else if (item.itemType == ItemType.Shield) {
            if (shieldHolsterMountPoint.childCount > 0) {
                foreach (Transform itemGo in shieldHolsterMountPoint) {
                    Destroy(itemGo.gameObject);
                }
            }
        }
        else {
            if (characterEquipmentType == CharacterEquipementType.EnableMesh) {
                for (int i = 0; i < itemsOnCharacter.Length; i++) {
                    if (item.itemSkinnedMeshName == itemsOnCharacter[i].name) {
                        itemsOnCharacter[i].enabled = false;
                    }
                }
            }
            else if (characterEquipmentType == CharacterEquipementType.ReplaceMesh) {
                if (item.itemType == ItemType.Chest) {
                    bodyMesh.sharedMesh = defaultBodyMesh;
                }
                else if (item.itemType == ItemType.Head) {
                    helmMesh.sharedMesh = null;
                }
                else if (item.itemType == ItemType.Legs) {
                    leggsMesh.sharedMesh = defaultLeggsMesh;
                }
                else if (item.itemType == ItemType.Hands) {
                    handsMesh.sharedMesh = defaultHandsMesh;
                }
                else if (item.itemType == ItemType.Neck) {
                    neckMesh.sharedMesh = null;
                }
            }
        }

        for (int i = 0; i < TopDownUIInventory.instance.slots.Length; i++) {
            if (TopDownUIInventory.instance.slots[i].itemInSlot == item) {
                //TopDownUIInventory.instance.slots[i].GetComponent<Image>().color = TopDownUIInventory.instance.slots[i].normalSlotColor;
            }
        }

        if (TopDownUIManager.instance.characterPortraitType == CharacterPortraitType.Runtime) {
            GetComponent<TopDownCharacterCard>().UpdatePortrait();
        }
    }

    public void SetAIEquipment() {
        for(int i = 0; i < currentEquipment.Length; i++) {
            if(currentEquipment[i] != null) {
                if (currentEquipment[i].itemType == ItemType.Weapon) {
                    if (currentEquipment[i].itemGameObject != null) {
                        GameObject itemGo = Instantiate(currentEquipment[i].itemGameObject) as GameObject;
                        itemGo.transform.SetParent(weaponHolsterMountPoint);
                        itemGo.transform.localPosition = Vector3.zero;
                        itemGo.transform.localEulerAngles = Vector3.zero;
                    }
                    weaponTypeUsed = currentEquipment[i].weaponType;
                    weaponHoldingType = currentEquipment[i].weaponHoldingType;
                }
                else if (currentEquipment[i].itemType == ItemType.Shield) {
                    if (currentEquipment[i].itemGameObject != null) {
                        GameObject itemGo = Instantiate(currentEquipment[i].itemGameObject) as GameObject;
                        itemGo.transform.SetParent(shieldHolsterMountPoint);
                        itemGo.transform.localPosition = Vector3.zero;
                        itemGo.transform.localEulerAngles = Vector3.zero;
                    }
                }
                AddPointModifiers(currentEquipment[i]);
            }
        }
    }

    public IEnumerator HolsterWeaponCoroutine(int holster) {
        if (weaponHolsterMountPoint != null && shieldHolsterMountPoint != null) {
            if (holster == 1) {
                if (weaponTypeUsed == WeaponType.Ranged) {
                    for (int i = 0; i < shieldMountPoint.childCount; i++) {
                        Transform shield = shieldMountPoint.GetChild(i).transform;
                        shield.SetParent(shieldHolsterMountPoint);
                        shield.localPosition = Vector3.zero;
                        shield.localEulerAngles = Vector3.zero;
                    }
                }
                else {
                    for (int i = 0; i < weaponMountPoint.childCount; i++) {
                        Transform weapon = weaponMountPoint.GetChild(i).transform;
                        weapon.SetParent(weaponHolsterMountPoint);
                        weapon.localPosition = Vector3.zero;
                        weapon.localEulerAngles = Vector3.zero;
                    }
                    for (int i = 0; i < shieldMountPoint.childCount; i++) {
                        Transform shield = shieldMountPoint.GetChild(i).transform;
                        shield.SetParent(shieldHolsterMountPoint);
                        shield.localPosition = Vector3.zero;
                        shield.localEulerAngles = Vector3.zero;
                    }
                }
                tcc_Main.tdcm_animator.SetBool("WeaponInHands", false);
            }
            else if (holster == 0) {
                if (weaponTypeUsed == WeaponType.Ranged) {
                    for (int i = 0; i < shieldHolsterMountPoint.childCount; i++) {
                        Transform shield = shieldHolsterMountPoint.GetChild(i).transform;
                        shield.SetParent(shieldMountPoint);
                        shield.localPosition = Vector3.zero;
                        shield.localEulerAngles = Vector3.zero;
                    }
                }
                else {
                    for (int i = 0; i < weaponHolsterMountPoint.childCount; i++) {
                        Transform weapon = weaponHolsterMountPoint.GetChild(i).transform;
                        weapon.SetParent(weaponMountPoint);
                        weapon.localPosition = Vector3.zero;
                        weapon.localEulerAngles = Vector3.zero;
                    }
                    for (int i = 0; i < shieldHolsterMountPoint.childCount; i++) {
                        Transform shield = shieldHolsterMountPoint.GetChild(i).transform;
                        shield.SetParent(shieldMountPoint);
                        shield.localPosition = Vector3.zero;
                        shield.localEulerAngles = Vector3.zero;
                    }
                }
                tcc_Main.tdcm_animator.SetBool("WeaponInHands", true);
            }
            if (TopDownAudioManager.instance.weaponSheatheAudio != null) {
                Instantiate(TopDownAudioManager.instance.weaponSheatheAudio, Vector3.zero, Quaternion.identity);
            }
        }
        else {
            Debug.LogWarning("Weapon Holster game object not set for " + gameObject.name);
        }
        yield return new WaitForEndOfFrame();
    }

    public void HolsterWeapon(int holster) {
        StartCoroutine(HolsterWeaponCoroutine(holster));
    }

    //enum PointModifiers { Armor, Damage, Strength, Dexterity, Constitution, Willpower}

    public void AddPointModifiers(TopDownItemObject item) {
        if (gameObject.tag == "Player") {
            armorPointsValue += item.armorModifier;
            damagePointsValue += item.damageModifier;
            //strength and others also

            //Than update UI
            TopDownCharacterManager.instance.activeCharacter.GetComponent<TopDownCharacterCard>().characterInventory.armorPointsTxt.text = "AP: " + armorPointsValue.ToString();
            TopDownCharacterManager.instance.activeCharacter.GetComponent<TopDownCharacterCard>().characterInventory.damagePointsTxt.text = "DP: " + damagePointsValue.ToString();
            return;
        }
    }

    public void RemovePointModifiers(TopDownItemObject item) {
        if (gameObject.tag == "Player") {
            armorPointsValue -= item.armorModifier;
            damagePointsValue -= item.damageModifier;
            //strength and others also

            //Than update UI
            TopDownCharacterManager.instance.activeCharacter.GetComponent<TopDownCharacterCard>().characterInventory.armorPointsTxt.text = "AP: " + armorPointsValue.ToString();
            TopDownCharacterManager.instance.activeCharacter.GetComponent<TopDownCharacterCard>().characterInventory.damagePointsTxt.text = "DP: " + damagePointsValue.ToString();
            return;
        }
    }

    public void DealDamage(GameObject target, float damage) {
        if(target.GetComponent<TopDownControllerMain>()) {

            TopDownCharacterCard td_characterCard = target.GetComponent<TopDownCharacterCard>();

            if (td_characterCard.health == 0f) {
                return;
            }
            else {
                if (target.GetComponent<TopDownEquipmentManager>()) {
                    TopDownEquipmentManager equipmentManager = target.GetComponent<TopDownEquipmentManager>();

                    if ((equipmentManager.armorPointsValue > 0)) {
                        float defensePercentage = equipmentManager.armorPointsValue / damage;
                        float finalDamage = damage - (defensePercentage * 0.25f);

                        if (finalDamage > 0f) {
                            td_characterCard.health -= finalDamage;
                        }
                        else {
                            td_characterCard.health -= 0.9f;
                        }
                    }
                    else {
                        if (damage > 0) {
                            float finalDamage = damage + (damage * 0.5f);
                            td_characterCard.health -= finalDamage;
                        }
                        else {
                            td_characterCard.health -= 0.9f;
                        }
                    }
                }
                else {
                    td_characterCard.health -= damage;
                }
            }
        }
    }

    public void PlayAudio(GameObject audio) {
        if (audio != null) {
            Instantiate(audio, transform.position, Quaternion.identity);
        }
    }

    public void AttackFocusedTarget() {
        if(tcc_Interact != null) {
            if (tcc_Interact.focusedTarget != null && td_characterCard.enemyFocus == null) {
                DealDamage(tcc_Interact.focusedTarget.gameObject, damagePointsValue);
                if (TopDownAudioManager.instance != null && TopDownAudioManager.instance.meleeHitAudio != null) {
                    Instantiate(TopDownAudioManager.instance.meleeHitAudio, transform.position, Quaternion.identity);
                }
                if(TopDownParticleManager.instance != null && TopDownParticleManager.instance.hitParticle != null) {
                    GameObject[] hit = TopDownParticleManager.instance.hitParticle;

                    if (tcc_Interact.focusedTarget.GetComponent<NavMeshAgent>()) {
                        float height = tcc_Interact.focusedTarget.GetComponent<NavMeshAgent>().height;
                        Vector3 center = new Vector3(tcc_Interact.focusedTarget.position.x, height * 0.5f, tcc_Interact.focusedTarget.position.z);
                        Instantiate(hit[Random.Range(0, hit.Length)], center, Quaternion.identity);
                    }
                    else {
                        Vector3 center = new Vector3(tcc_Interact.focusedTarget.position.x, 1.5f, tcc_Interact.focusedTarget.position.z);
                        Instantiate(hit[Random.Range(0, hit.Length)], center, Quaternion.identity);
                    }


                    if (tcc_Interact.focusedTarget.GetComponent<TopDownAI>()) {
                        TopDownAI ai = tcc_Interact.focusedTarget.GetComponent<TopDownAI>();
                        if (ai.voiceSet != null) {
                            Instantiate(ai.voiceSet.getHitVoice, transform.position, Quaternion.identity);
                        }
                    }
                }
            }
            else if(td_characterCard.enemyFocus != null) {
                DealDamage(td_characterCard.enemyFocus.gameObject, damagePointsValue);
                if (TopDownAudioManager.instance != null && TopDownAudioManager.instance.meleeHitAudio != null) {
                    Instantiate(TopDownAudioManager.instance.meleeHitAudio, transform.position, Quaternion.identity);
                }
                if (TopDownParticleManager.instance != null && TopDownParticleManager.instance.hitParticle != null) {
                    GameObject[] hit = TopDownParticleManager.instance.hitParticle;

                    if (td_characterCard.enemyFocus.GetComponent<NavMeshAgent>()) {
                        float height = td_characterCard.enemyFocus.GetComponent<NavMeshAgent>().height;
                        Vector3 center = new Vector3(td_characterCard.enemyFocus.position.x, height * 0.5f, td_characterCard.enemyFocus.position.z);
                        Instantiate(hit[Random.Range(0, hit.Length)], center, Quaternion.identity);
                    }
                    else {
                        Vector3 center = new Vector3(td_characterCard.enemyFocus.position.x, 1.5f, td_characterCard.enemyFocus.position.z);
                        Instantiate(hit[Random.Range(0, hit.Length)], center, Quaternion.identity);
                    }


                    if (td_characterCard.enemyFocus.GetComponent<TopDownAI>()) {
                        TopDownAI ai = td_characterCard.enemyFocus.GetComponent<TopDownAI>();
                        if (ai.voiceSet != null) {
                            Instantiate(ai.voiceSet.getHitVoice, transform.position, Quaternion.identity);
                        }
                    }
                }
            }
        }
        else {
            if(GetComponent<TopDownAI>()) {

                //print("This is AI character.");

                TopDownAI ai = GetComponent<TopDownAI>();

                if(ai.focus != null) {
                    DealDamage(ai.focus.gameObject, damagePointsValue);
                    if (TopDownAudioManager.instance != null && TopDownAudioManager.instance.meleeHitAudio != null) {
                        Instantiate(TopDownAudioManager.instance.meleeHitAudio, transform.position, Quaternion.identity);
                    }
                    if (TopDownParticleManager.instance != null && TopDownParticleManager.instance.hitParticle != null) {
                        GameObject[] hit = TopDownParticleManager.instance.hitParticle;

                        float height = ai.focus.GetComponent<NavMeshAgent>().height;
                        Vector3 center = new Vector3(ai.focus.position.x, height * 0.5f, ai.focus.position.z);

                        Instantiate(hit[Random.Range(0, hit.Length)], center, Quaternion.identity);
                    }
                }
            }
        }
    }
}
