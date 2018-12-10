using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopDownUIShowNotification : MonoBehaviour {

    public Animator anim;
    public Text notificationText;

    private void Start() {
        NotificationReset();
    }

    public void ShowNotification(string notification) {
        if (anim.GetBool("SHOW") == false) { //if this is true that means there is already text showing up
            anim.SetBool("SHOW", true);
            notificationText.text = notification;
        }
    }

    public void NotificationReset() {
        anim.SetBool("SHOW", false);
        notificationText.text = string.Empty;
    }
}
