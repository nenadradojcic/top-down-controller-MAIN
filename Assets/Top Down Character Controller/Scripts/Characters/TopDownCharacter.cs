using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Top Down RPG/Characters/Character")]
public class TopDownCharacter : ScriptableObject {

    public Sprite icon;

    public new string name;

    public float health;

    public float energy;

    public TopDownVoiceSet voiceSet;
}
