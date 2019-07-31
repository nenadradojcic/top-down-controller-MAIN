using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraType {
    CharacterCamera = 0,
    FreeCamera = 1,
}

[AddComponentMenu("Top Down RPG/Camera/Camera Basic")]
public class TopDownCameraBasic : MonoBehaviour {

    #region Variables
    public CameraType cameraType;

    public Transform td_Target;

    public float distanceDefault = 8f;
    public float distanceMin = 4f;
    public float distanceMax = 10f;

    public float rotationSpeed = 60f;

    public float cameraAngleMin = 30f;
    public float cameraAngleMax = 70f;

    public float yAxisOffset = 1.35f;

    public float freeCameraSpeed = 0.5f;

    private TopDownInputManager td_InputManager;
    private TopDownUIManager td_UiManager;

    public float cameraAxisX = 0f;
    public float cameraAxisY = 0f;
    public float cameraZoomAxis = 0f;

    public LayerMask cameraHeightLayerMask;
    public float freeCamRaycastDistanceCheck;
    public Transform invisibleTarget;

    float x = 0.0f;
    float y = 0.0f;
    float characterSize = 1f;

    public static TopDownCameraBasic instance;

    //These will be used only if TopDownInputManager.cs is not present in scene
    public string defMouseX = "Mouse X";
    public string defMouseY = "Mouse Y";
    public string defMouseScroll = "Mouse ScrollWheel";
    public KeyCode defChangeCamKey = KeyCode.C;
    public KeyCode defRotCamKey = KeyCode.Mouse1;

    public TopDownControllerInteract targetInteractScript;
    #endregion

    public void Awake() {
        instance = this;
    }

    private void Start() {

        if (TopDownCharacterManager.instance == null || TopDownCharacterManager.instance.defaultCharacter == null) {
            if (GameObject.FindGameObjectWithTag("Player")) {
                td_Target = GameObject.FindGameObjectWithTag("Player").transform;
                if (td_Target.GetComponent<TopDownControllerInteract>()) {
                    targetInteractScript = td_Target.GetComponent<TopDownControllerInteract>();
                }
            }
            else {
                Debug.LogWarning("No player character found in scene.");
            }

        }

        td_InputManager = TopDownInputManager.instance;
        td_UiManager = TopDownUIManager.instance;

         Vector3 angles = transform.eulerAngles;
        x = angles.x;
        y = angles.y;

        if (td_Target != null) {
            characterSize = td_Target.localScale.x;
            distanceDefault = distanceDefault * characterSize;
        }
    }

    void LateUpdate() {
        if (td_Target) {

            if(td_Target.localScale.x != characterSize) {
                characterSize = td_Target.localScale.x;
                distanceDefault = distanceDefault * characterSize;
            }

            if (td_InputManager != null) {
                cameraZoomAxis = Input.GetAxis(td_InputManager.mouseScrollwheelName);
            }
            else {
                cameraZoomAxis = Input.GetAxis(defMouseScroll);
            }

            if (td_UiManager != null) {
                if (td_UiManager.checkUi != null) {
                    if (td_UiManager.checkUi.IsPointerOverUIObject() == false) {

                        if (Input.GetKey(td_InputManager.interactKey) && Input.GetKey(td_InputManager.rotateCamera)) {
                            Cursor.lockState = CursorLockMode.Locked;
                            Cursor.visible = false;
                        }
                        else if (!Input.GetKey(td_InputManager.interactKey) && Input.GetKey(td_InputManager.rotateCamera)) {
                            Cursor.lockState = CursorLockMode.Confined;
                            Cursor.visible = false;
                        }
                        else {
                            Cursor.lockState = CursorLockMode.None;
                            Cursor.visible = true;
                        }

                        if (Input.GetKey(td_InputManager.rotateCamera)) {
                            cameraAxisX = Input.GetAxis(td_InputManager.mouseYName);
                            cameraAxisY = Input.GetAxis(td_InputManager.mouseXName);
                        }
                        else {
                            cameraAxisX = Mathf.Lerp(cameraAxisX, 0f, 0.1f);
                            cameraAxisY = Mathf.Lerp(cameraAxisY, 0f, 0.1f);
                        }
                    }
                    else {
                        cameraAxisX = Mathf.Lerp(cameraAxisX, 0f, 0.1f);
                        cameraAxisY = Mathf.Lerp(cameraAxisY, 0f, 0.1f);
                    }
                }
                else {
                    if (td_InputManager != null) {
                        cameraAxisX = Input.GetAxis(td_InputManager.mouseYName);
                        cameraAxisY = Input.GetAxis(td_InputManager.mouseXName);
                    }
                    else {
                        cameraAxisX = Input.GetAxis(defMouseY);
                        cameraAxisY = Input.GetAxis(defMouseX);
                    }
                }
            }
            else {
                if (td_InputManager != null) {
                    if (Input.GetKey(td_InputManager.interactKey) && Input.GetKey(td_InputManager.rotateCamera)) {
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                    }
                    else if (!Input.GetKey(td_InputManager.interactKey) && Input.GetKey(td_InputManager.rotateCamera)) {
                        Cursor.lockState = CursorLockMode.Confined;
                        Cursor.visible = false;
                    }
                    else {
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                    }

                    if (Input.GetKey(td_InputManager.rotateCamera)) {
                        cameraAxisX = Input.GetAxis(td_InputManager.mouseYName);
                        cameraAxisY = Input.GetAxis(td_InputManager.mouseXName);
                    }
                    else {
                        cameraAxisX = Mathf.Lerp(cameraAxisX, 0f, 0.1f);
                        cameraAxisY = Mathf.Lerp(cameraAxisY, 0f, 0.1f);
                    }
                }
                else {
                    if (Input.GetKey(targetInteractScript.defInteractKey) && Input.GetKey(defRotCamKey)) {
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                    }
                    else if (!Input.GetKey(targetInteractScript.defInteractKey) && Input.GetKey(defRotCamKey)) {
                        Cursor.lockState = CursorLockMode.Confined;
                        Cursor.visible = false;
                    }
                    else {
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                    }

                    if (Input.GetKey(defRotCamKey)) {
                        cameraAxisX = Input.GetAxis(defMouseY);
                        cameraAxisY = Input.GetAxis(defMouseX);
                    }
                    else {
                        cameraAxisX = Mathf.Lerp(cameraAxisX, 0f, 0.1f);
                        cameraAxisY = Mathf.Lerp(cameraAxisY, 0f, 0.1f);
                    }
                }
            }

            if (td_InputManager != null) {
                if (Input.GetKeyDown(td_InputManager.changeCamera)) {
                    if (cameraType == CameraType.CharacterCamera) {
                        invisibleTarget = new GameObject().transform;
                        invisibleTarget.position = td_Target.position;
                        invisibleTarget.name = "TD_CameraFollowTemp";
                        invisibleTarget.SetSiblingIndex(transform.GetSiblingIndex() + 1);
                        cameraType = CameraType.FreeCamera;
                    }
                    else {
                        Destroy(invisibleTarget.gameObject);
                        invisibleTarget = null;
                        cameraType = CameraType.CharacterCamera;
                    }
                }
            }
            else {
                if (Input.GetKeyDown(defChangeCamKey)) {
                    if (cameraType == CameraType.CharacterCamera) {
                        invisibleTarget = new GameObject().transform;
                        invisibleTarget.position = td_Target.position;
                        invisibleTarget.name = "TD_CameraFollowTemp";
                        invisibleTarget.SetSiblingIndex(transform.GetSiblingIndex() + 1);
                        cameraType = CameraType.FreeCamera;
                    }
                    else {
                        Destroy(invisibleTarget.gameObject);
                        invisibleTarget = null;
                        cameraType = CameraType.CharacterCamera;
                    }
                }
            }

            if (cameraType == CameraType.CharacterCamera) {
                x += -cameraAxisX * rotationSpeed * distanceDefault * 0.02f;
                y += cameraAxisY * rotationSpeed * distanceDefault * 0.02f;

                x = Mathf.Clamp(x, cameraAngleMin, cameraAngleMax);

                Quaternion rotation = Quaternion.Euler(x, y, 0);

                distanceDefault = Mathf.Clamp(distanceDefault - cameraZoomAxis * 5 * characterSize, distanceMin * characterSize, distanceMax * characterSize);

                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distanceDefault);
                Vector3 pos = new Vector3(td_Target.position.x, td_Target.position.y + yAxisOffset, td_Target.position.z);
                Vector3 position = rotation * negDistance + pos;

                transform.rotation = rotation;
                transform.position = position;
            }
            else if(cameraType == CameraType.FreeCamera) {
                x += -cameraAxisX * rotationSpeed * distanceDefault * 0.02f;
                y += cameraAxisY * rotationSpeed * distanceDefault * 0.02f;

                x = Mathf.Clamp(x, cameraAngleMin, cameraAngleMax);

                Quaternion rotation = Quaternion.Euler(x, y, 0);

                distanceDefault = Mathf.Clamp(distanceDefault - cameraZoomAxis * 5 * characterSize, distanceMin * characterSize, distanceMax * characterSize);

                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distanceDefault);
                Vector3 pos = new Vector3(invisibleTarget.position.x, invisibleTarget.position.y + yAxisOffset, invisibleTarget.position.z);
                Vector3 position = rotation * negDistance + pos;

                transform.rotation = rotation;
                transform.position = position;
            }

            if(invisibleTarget != null) {
                Vector3 movement;
                if (td_InputManager != null) {
                    movement = new Vector3(td_InputManager.horizontalAxis, 0f, td_InputManager.verticalAxis);
                }
                else {
                    movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
                }
                movement = transform.TransformDirection(movement);
                movement.y = 0f;

                invisibleTarget.Translate(movement * freeCameraSpeed * characterSize);
                
                Ray ray = new Ray(transform.position, Vector3.down);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, distanceDefault + 1f, cameraHeightLayerMask)) {
                    Vector3 tmp = new Vector3(invisibleTarget.position.x, hit.point.y, invisibleTarget.position.z);
                    invisibleTarget.position = tmp;
                }
            }
        }
    }
}
