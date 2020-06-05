using System.Collections;
using UnityEngine;
using UnityEditor;


[AddComponentMenu("Top Down RPG/Input/Input Manager")]
public class TopDownInputManager : MonoBehaviour {

    #region Variables
    public string horizontalAxisName = "Horizontal";
    public string verticalAxisName = "Vertical";
    public string mouseXName = "Mouse X";
    public string mouseYName = "Mouse Y";
    public string mouseScrollwheelName = "Mouse ScrollWheel";

    public KeyCode pauseGameKey = KeyCode.Space;
    public bool pausedGame = false;
    public KeyCode interactKey = KeyCode.Mouse0;
    public KeyCode inventoryKeyCode = KeyCode.I;
    public KeyCode questLogKeyCode = KeyCode.J;

    public KeyCode changeCamera = KeyCode.C;
    public KeyCode rotateCamera = KeyCode.Mouse1;

    public KeyCode currentKeyPressed;

    public float horizontalAxis = 0f;
    public float verticalAxis = 0f;
    public float controllerType = 0f;

    public static TopDownInputManager instance;
    #endregion

    private void Awake() {
        instance = this;
    }

    public void LateUpdate() {
        horizontalAxis = Input.GetAxisRaw(horizontalAxisName);
        verticalAxis = Input.GetAxisRaw(verticalAxisName);
    }

    public void DetectPressedKey() { //We want to call this only when we are reasigning input from the game
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))) {
            if (Input.GetKeyDown(vKey)) {
                currentKeyPressed = vKey;
                Debug.Log("Key pressed: " + currentKeyPressed);
                StartCoroutine(KeyResetTimer());
                return;
            }
        }
    }

    private IEnumerator KeyResetTimer() {
        yield return new WaitForSecondsRealtime(0.05f);
        currentKeyPressed = KeyCode.None;
        Debug.Log("Key pressed: " + currentKeyPressed);
    }
}
