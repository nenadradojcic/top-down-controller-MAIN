using UnityEngine;

public enum AbilityType {
    Projectile = 0,
    InPlace = 1,
    OnOtherPlace = 2,
    CombatMove = 3,
}

[CreateAssetMenu(fileName = "New Ability", menuName = "Top Down RPG/Abilities/Ability")]
public class TopDownRpgAbilityObject : ScriptableObject {

    public string abilityName;
    [TextArea(0, 10)]
    public string abilityDescription;
    public Sprite abilityIcon;
    public AbilityType abilityType;
    public int energyCost;

    public GameObject abilityFx;

    public string abilityAnim;
    public float animTime;

}
