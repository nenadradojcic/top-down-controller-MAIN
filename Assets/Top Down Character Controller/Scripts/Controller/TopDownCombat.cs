using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCombat : MonoBehaviour {

    public GameObject weaponHitAudio;

    private TopDownEquipmentManager equipmentManager;

    private void Start() {
        equipmentManager = GetComponentInParent<TopDownEquipmentManager>();
    }

    public void OnTriggerEnter(Collider other) {
        if (equipmentManager != null) {
            if (equipmentManager.gameObject.tag == "Player") {
                if (other.tag == "Enemy") {
                    equipmentManager.DealDamage(other.gameObject, equipmentManager.damagePointsValue);
                }
            }
            else if (equipmentManager.gameObject.tag == "Enemy") {
                if (other.tag == "Player") {
                    equipmentManager.DealDamage(other.gameObject, equipmentManager.damagePointsValue);
                }
            }

            PlayAudio(weaponHitAudio);
        }
        else {
            print("No equipment manager found.");
        }
    }

    private void PlayAudio(GameObject audio) {
        if (audio != null) {
            Instantiate(audio, transform.position, Quaternion.identity);
        }
    }
}
