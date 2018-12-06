using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TopDownAudioClipPlayer : MonoBehaviour {

    public AudioClip clipToPlay;
    public AudioClip[] audioClips;

    public void Start() {
        clipToPlay = audioClips[Random.Range(0, audioClips.Length)];
        GetComponent<AudioSource>().PlayOneShot(clipToPlay);
    }

    public void Update() {
        Destroy(gameObject, clipToPlay.length + 0.2f);
    }
}
