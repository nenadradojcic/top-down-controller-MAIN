using UnityEngine;
using UnityEngine.Events;

public class TopDownRpgOnDeathEvent : MonoBehaviour {

    public UnityEvent OnDeathEvent;

    public void InvokeOnDeathEvent() {
        OnDeathEvent.Invoke();
        Destroy(this);
    }
}
