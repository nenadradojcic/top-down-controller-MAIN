using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraType {
    CharacterCamera = 0,
    FreeCamera = 1,
}

public class TopDownCameraBasic : MonoBehaviour {

    #region Variables
    public CameraType cameraType;

    public Transform td_Target;

    public float distanceDefault = 8f;
    public float distanceMin = 2f;
    public float distanceMax = 10f;

    public float rotationSpeed = 60f;

    public float cameraAngle = 55f;

    public float yAxisOffset = 1.25f;

    public float freeCameraSpeed = 0.5f;

    private TopDownInputManager td_InputManager;

    public float cameraAxis = 0f;
    public float cameraZoomAxis = 0f;

    public LayerMask cameraHeightLayerMask;
    public float freeCamRaycastDistanceCheck;
    public Transform invisibleTarget;

    float y = 0.0f;
    float characterSize = 1f;

    public static TopDownCameraBasic instance;
    #endregion

    public void Awake() {
        instance = this;
    }

    private void Start() {

        if (TopDownCharacterManager.instance == null || TopDownCharacterManager.instance.defaultCharacter == null) {
            if (GameObject.FindGameObjectWithTag("Player")) {
                td_Target = GameObject.FindGameObjectWithTag("Player").transform;
            }
            else {
                Debug.LogWarning("No player character found in scene.");
            }

        }
        td_InputManager = TopDownInputManager.instance;

        Vector3 angles = transform.eulerAngles;
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

            cameraZoomAxis = Input.GetAxis(td_InputManager.mouseScrollwheelName);

            if (Input.GetKey(td_InputManager.rotateCamera)) {
                if (TopDownUIManager.instance.checkUi != null) {
                    if (TopDownUIManager.instance.checkUi.IsPointerOverUIObject() == false) {
                        cameraAxis = Input.GetAxis(td_InputManager.mouseXName);
                    }
                    else {
                        cameraAxis = Mathf.Lerp(cameraAxis, 0f, 0.1f);
                    }
                }
                else {
                    cameraAxis = Input.GetAxis(td_InputManager.mouseXName);
                }
            }
            else {
                cameraAxis = Mathf.Lerp(cameraAxis, 0f, 0.1f);
            }

            if (Input.GetKeyDown(td_InputManager.changeCamera)) {
                if(cameraType == CameraType.CharacterCamera) {
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

            if (cameraType == CameraType.CharacterCamera) {
                y += cameraAxis * rotationSpeed * distanceDefault * 0.02f;

                Quaternion rotation = Quaternion.Euler(cameraAngle, y, 0);

                distanceDefault = Mathf.Clamp(distanceDefault - cameraZoomAxis * 5 * characterSize, distanceMin * characterSize, distanceMax * characterSize);

                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distanceDefault);
                Vector3 pos = new Vector3(td_Target.position.x, td_Target.position.y + yAxisOffset, td_Target.position.z);
                Vector3 position = rotation * negDistance + pos;

                transform.rotation = rotation;
                transform.position = position;
            }
            else if(cameraType == CameraType.FreeCamera) {
                y += cameraAxis * rotationSpeed * distanceDefault * 0.02f;

                Quaternion rotation = Quaternion.Euler(cameraAngle, y, 0);

                distanceDefault = Mathf.Clamp(distanceDefault - cameraZoomAxis * 5 * characterSize, distanceMin * characterSize, distanceMax * characterSize);

                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distanceDefault);
                Vector3 pos = new Vector3(invisibleTarget.position.x, invisibleTarget.position.y + yAxisOffset, invisibleTarget.position.z);
                Vector3 position = rotation * negDistance + pos;

                transform.rotation = rotation;
                transform.position = position;
            }

            if(invisibleTarget != null) {
                Vector3 movement = new Vector3(td_InputManager.horizontalAxis, 0f, td_InputManager.verticalAxis);
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
