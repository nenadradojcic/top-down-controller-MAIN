using UnityEngine;

[CreateAssetMenu(fileName = "New Voice Set", menuName = "Top Down RPG/Characters/Voice Set")]
public class TopDownVoiceSet : ScriptableObject {

    public string setName;

    public GameObject idleVoice;

    public GameObject detectVoice;

    public GameObject getHitVoice;

    public GameObject deathVoice;
	
}
