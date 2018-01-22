using UnityEngine;

public class CameraOrbit : MonoBehaviour {

    protected Transform _XForm_Camera;
    protected Transform _XForm_Parent;
    protected Vector3 _LocalRotation;
    protected float _CameraDistance = 10f;

    [Header("Mouse Controls")]
    public float MouseSensitivity = 4f;
    public float ScrollSensitivity = 2f;
    [Space]
    public float OrbitDampening = 10f;
    public float ScrollDampening = 6f;

    public float CameraMinDistance = 2f;

    public bool CameraDisabled = false;

    void Start () {
        this._XForm_Camera = this.transform;
        this._XForm_Parent = this.transform.parent;

    }

    void Update()
    {
        if( Input.GetMouseButton(0) || Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            Debug.Log("MouseButtonDown");
            CameraDisabled = false;
        }
        else
    {
            CameraDisabled = true;
        }
    }

    void LateUpdate () {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            CameraDisabled = !CameraDisabled;

        if(!CameraDisabled )
        {

            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                _LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
                _LocalRotation.y -= Input.GetAxis("Mouse Y") * MouseSensitivity;

                // Clamp the y rotation to horizon and not flipping over at the top
                _LocalRotation.y = Mathf.Clamp(_LocalRotation.y, 0f, 90f);
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0f )
            {
                float scrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity;
                scrollAmount *= (this._CameraDistance * 0.3f);

                this._CameraDistance += scrollAmount * -1f;
                this._CameraDistance = Mathf.Clamp(this._CameraDistance, CameraMinDistance, 100f);
            }
        }

        Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
        this._XForm_Parent.rotation = Quaternion.Lerp(this._XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);

        if(this._XForm_Camera.localPosition.z != this._CameraDistance * -1f)
        {
            this._XForm_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this._XForm_Camera.localPosition.z, this._CameraDistance * -1f, Time.deltaTime * ScrollDampening));
        }

    }
}
