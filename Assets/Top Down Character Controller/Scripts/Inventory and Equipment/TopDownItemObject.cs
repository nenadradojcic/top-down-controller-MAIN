using UnityEngine;
using UnityEngine.Events;

public enum ItemType {
    Misc = 0,
    Potion = 1,
    Weapon = 2,
    Shield = 3,
    Head = 4,
    Chest = 5,
    Legs = 6,
    Hands = 7,
    RingL = 8,
    RingR = 9,
    Neck = 10,
    Scroll = 11,
}

public enum SpellType {
    CastOnEnemy = 0,
    CastOnAlly = 1,
    CastOnSelf = 2,
    CastOnCursor = 3,
}

public enum ItemVisualisation {
    None = 0,
    InstantiateObject = 1,
    FindSkinedMesh = 2,
    ReplaceMesh = 3,
}

[CreateAssetMenu(fileName = "New Item", menuName = "Top Down RPG/Inventory/Item")]
public class TopDownItemObject : ScriptableObject {

    public bool isItem = true;

    public string itemName = "New Item";
    public Sprite itemIcon = null;
    public int itemStack;
    public float itemValue;

    [TextArea(2, 5)]
    public string itemDescription;

    public SpellType spellType;
    public GameObject spellFx;
    public GameObject onImpactFx;
    public int castingCost;
    public string castingSpellAnimation;
    public float animationTriggerTime;
    public GameObject spellCastSfx;
    public GameObject spellImpactSfx;

    public UnityEvent spellOnImpactEvents;

    public ItemType itemType;
    public WeaponType weaponType;
    public WeaponHoldingType weaponHoldingType;

    public int armorModifier;
    public int damageModifier;

    public int strengthModifier; //How much damage do we deal
    public int dexterityModifier; //How much damage will we evade
    public int constitutionModifier; //How much health do we have
    public int willpowerModifier; //How much action points do we have

    private TopDownEquipmentManager equipmentManager;

    public TopDownUIItemSlot slotOfThisItem;

    public ItemVisualisation itemVisualisation;
    public Mesh itemMesh;
    public GameObject itemGameObject;
    public string itemSkinnedMeshName; //Used for armor items
    public GameObject instantiatedModel;

    public virtual void UseItem() {

        equipmentManager = TopDownUIInventory.instance.currentEquipmentManager;

        if (equipmentManager.tcc_Main.tdcm_animator.GetBool("Attacking") == false) { //We want to be able to use items(equip/change them) only when we are not attacking
            if (equipmentManager.currentEquipment[(int)itemType] != this) { //If this item is not equipped
                equipmentManager.EquipItem(this);
                //Debug.Log("Equiping item " + itemName);
            }
            else {
                equipmentManager.UnequipItem(this);
                //Debug.Log("Unequiping item " + itemName);
            }
        }
    }

    public virtual void UnuseItem() { //Used only for equipment

        equipmentManager = TopDownUIInventory.instance.currentEquipmentManager;

        if (equipmentManager.tcc_Main.tdcm_animator.GetBool("Attacking") == false) {
            if (equipmentManager.currentEquipment[(int)itemType] == this) {
                equipmentManager.UnequipItem(this);
            }
        }
    }

    public void Spell_Damage(int damageValue) {
        TopDownCharacterCard tdcc = TopDownCharacterManager.instance.controllingCharacter.GetComponent<TopDownRpgSpellcaster>().previousTarget.GetComponent<TopDownCharacterCard>();
        if(tdcc != null) {
            tdcc.health -= damageValue;

            TopDownAI tdccAi = tdcc.GetComponent<TopDownAI>();

            if (tdccAi.playerInSight == false) {
                tdccAi.detected = true;
                tdccAi.playerInSight = true;
                tdccAi.focus = TopDownCharacterManager.instance.controllingCharacter.transform;
            }
        }
    }

    public void Spell_Heal(int healValue) {
        TopDownCharacterCard tdcc = TopDownCharacterManager.instance.controllingCharacter.GetComponent<TopDownRpgSpellcaster>().previousTarget.GetComponent<TopDownCharacterCard>();
        if (tdcc != null) {
            tdcc.health += healValue;
        }
    }
}
