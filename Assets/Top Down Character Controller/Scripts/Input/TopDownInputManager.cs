using System.Collections;
using XInputDotNetPure;
using UnityEngine;
using UnityEditor;

public enum InputType {
    KeyboardAndMouse = 0,
    Xbox360Gamepad = 1,
    XboxOneGamepad = 2,
    Mobile = 3,
}

[AddComponentMenu("Top Down RPG/Input/Input Manager")]
public class TopDownInputManager : MonoBehaviour {

    #region Variables
    [Header("Input Type")]
    public InputType inputType;

    public string horizontalAxisName = "Horizontal";
    public string verticalAxisName = "Vertical";
    public string mouseXName = "Mouse X";
    public string mouseYName = "Mouse Y";
    public string mouseScrollwheelName = "Mouse ScrollWheel";

    public KeyCode pauseGameKey = KeyCode.Space;
    public bool pausedGame = false;
    public KeyCode interactKey = KeyCode.Mouse0;
    public KeyCode inventoryKeyCode = KeyCode.I;

    public KeyCode changeCamera = KeyCode.C;
    public KeyCode rotateCamera = KeyCode.Mouse1;

    public KeyCode currentKeyPressed;

    public float horizontalAxis = 0f;
    public float verticalAxis = 0f;
    public float controllerType = 0f;

    public int Xbox360_Controller = 0;
    public int XboxOne_Controller = 0;

    public bool playerIndexSet = false;
    public PlayerIndex playerIndex;
    public GamePadState state;
    public GamePadState prevState;

    public bool mobile;

    public static TopDownInputManager instance;
    #endregion

    private void Awake() {
        instance = this;
    }

    #region DetectInputDevice
    private void Update() {        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected and use it
        if (!playerIndexSet || !prevState.IsConnected) {
            for (int i = 0; i < 4; ++i) {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected) {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);

        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++) {
            //print(names[x].Length);
            if (names[x].Length == 33) {
                //set a controller bool to true
                XboxOne_Controller = 0;
                Xbox360_Controller = 1;

            }
            if (names[x].Length == 51) {
                //set a controller bool to true
                XboxOne_Controller = 1;
                Xbox360_Controller = 0;

            }
        }
        
        if(Input.touchCount > 0) {
            mobile = true;
        }

        if (Xbox360_Controller == 1) {
            inputType = InputType.Xbox360Gamepad;
        }
        else if (XboxOne_Controller == 1) {
            inputType = InputType.XboxOneGamepad;
        }
        else if(mobile) {
            inputType = InputType.Mobile;
        }
        else {
            inputType = InputType.KeyboardAndMouse;
        }
    }
    #endregion

    public void LateUpdate() {
        if (inputType == InputType.KeyboardAndMouse) {
            horizontalAxis = Input.GetAxisRaw(horizontalAxisName);
            verticalAxis = Input.GetAxisRaw(verticalAxisName);
        }
        else if (inputType == InputType.Xbox360Gamepad) {
            /*horizontalAxis = state.ThumbSticks.Left.X;
            verticalAxis = state.ThumbSticks.Left.Y;

            if (td_checkUI.IsPointerOverUIObject() == false) {
                cameraAxis = state.ThumbSticks.Right.X / 4f;
            }

            if (prevState.DPad.Down == ButtonState.Pressed) {
                cameraZoomAxis = state.ThumbSticks.Right.Y / 40f;
            }

            if (prevState.Buttons.LeftStick == ButtonState.Released && state.Buttons.LeftStick == ButtonState.Pressed) {
                controllerType = 1f;
            }
            else {
                controllerType = 0f;
            }

            if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed) {
                //mouseLClick = 1f;
            }
            else {
                //mouseLClick = 0f;
            }*/
        }
        else if (inputType == InputType.Mobile) {
            /*verticalAxis = CrossPlatformInputManager.GetAxis("Vertical");
            horizontalAxis = CrossPlatformInputManager.GetAxis("Horizontal");*/
        }
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
