using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownAudioManager : MonoBehaviour {

    public GameObject inventoryOpenAudio;
    public GameObject inventoryItemPickupAudio;
    public GameObject inventoryItemUseAudio;

    public GameObject weaponSheatheAudio;
    public GameObject meleeSwingAudio;
    public GameObject meleeHitAudio;

    public GameObject footstepAudio;
    
    public static TopDownAudioManager instance;

    public void Start() {
        instance = this;
    }
}
