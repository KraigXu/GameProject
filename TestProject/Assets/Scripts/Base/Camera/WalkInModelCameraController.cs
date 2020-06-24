using UnityEngine;
using System.Collections;

public class WalkInModelCameraController : MonoBehaviour
{
    public float m_defaultInertia;

    public Vector2 m_targetAngle = new Vector2(-28f, 20f);

    public Vector2 m_currentAngle = new Vector2(-28f, 20f);

    public float m_targetSize = 780f;

    public float m_currentSize = 780f;

    public float m_minDistance = 40f;

    public float m_maxDistance = 3000f;

    public float m_distanceToBottom = 2f;

    private float m_zoomVelocity;

    private Vector3 m_velocity;

    private Vector2 m_angleVelocity;

    private float m_mouseSensitivity = 0.5f;

    private Rigidbody m_rigidbody;

    private bool m_isZoomIn; //ture 为前进；false为后退

    private void OnEnable()
    {
        //m_targetPosition = transform.position;
    }

    private void OnDisable()
    {

    }

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        UpdateTargetPosition();
        UpdateCurrentPosition();
        UpdateTransform();
        transform.position = ClampHeight();
    }

    private void FixedUpdate()
    {

    }

    private void HandleMouseEvents(float multiplier)
    {
        Vector2 zero = Vector2.zero;
        if (Input.GetMouseButton(1))
        {
            zero = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
            zero *= 0.3f;
            if (zero.magnitude > 0.05f && Cursor.visible)
            {
                Cursor.visible = !Cursor.visible;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        else if (!Cursor.visible)
        {
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = CursorLockMode.None;
        }
        this.m_angleVelocity += zero * (12f * this.m_mouseSensitivity * multiplier);

    }

    private void HandleScrollWheelEvent(float multiplier)
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float axis = Input.GetAxis("Mouse ScrollWheel");
            axis *= 0.3f;
            this.m_zoomVelocity += axis * (-20f * multiplier);
        }
    }

    private void HandleKeyEvents(float multiplier)
    {
        Vector3 zero = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            zero.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            zero.x += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            zero.z -= 1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            zero.z += 1;
        }
        this.m_velocity += zero * multiplier * Time.deltaTime;

        Vector2 zero2 = Vector2.zero;
        if (Input.GetKey(KeyCode.Q))
        {
            zero2.x += 200f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            zero2.x -= 200f;
        }
        if (Input.GetKey(KeyCode.R))
        {
            zero2.y += 200f;
        }
        if (Input.GetKey(KeyCode.F))
        {
            zero2.y -= 200f;
        }
        this.m_angleVelocity += zero2 * multiplier * Time.deltaTime;
    }

    private void UpdateTargetPosition()
    {
        float num = Mathf.Max(0.001f, Time.deltaTime);
        float f = m_defaultInertia;
        float num2 = Mathf.Pow(f, num);
        this.m_velocity *= num2;
        this.m_angleVelocity *= num2;
        this.m_zoomVelocity *= num2;

        num2 = 1f / num * (1f - num2);

        HandleMouseEvents(num2);
        HandleKeyEvents(num2);
        HandleScrollWheelEvent(num2);

        Vector2 vector = Vector2.ClampMagnitude(this.m_angleVelocity * num, 360f);
        if (Mathf.Abs(this.m_angleVelocity.x) > 0.001f)
        {
            this.m_targetAngle.x = this.m_targetAngle.x + vector.x;
            if (this.m_targetAngle.x > 180)
            {
                this.m_targetAngle.x -= 360f;
            }
            if (this.m_targetAngle.x < -180)
            {
                this.m_targetAngle.x += 360f;
            }
        }
        if (Mathf.Abs(this.m_angleVelocity.y) > 0.001f)
        {
            this.m_targetAngle.y = this.m_targetAngle.y + vector.y;
            if (this.m_targetAngle.y > 90f)
            {
                this.m_targetAngle.y = 90f;
            }
            if (this.m_targetAngle.y < 0f)
            {
                this.m_targetAngle.y = 0f;
            }
        }
        float num3 = this.m_targetSize;
        if (Mathf.Abs(this.m_zoomVelocity) > 0.001f)
        {
            if (this.m_zoomVelocity < 0f)
            {
                num3 *= Mathf.Pow(1.1f, -this.m_zoomVelocity * num);
                m_isZoomIn = true;
            }
            else
            {
                num3 *= Mathf.Pow(1.1f, this.m_zoomVelocity * num);
                m_isZoomIn = false;
                if (this.m_targetAngle.y < 5)
                {
                    this.m_targetAngle.y = 5;
                }
                Disable();
            }
        }
        this.m_targetSize = Mathf.Clamp(num3, this.m_minDistance, this.m_maxDistance);

        if (Vector3.SqrMagnitude(this.m_velocity) > 0.001f)
        {
            Vector3 point = this.m_velocity;
            Vector3 right = base.transform.right;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.right, right);
            point = rotation * point;
            point.y = 0f;
            //point.x *= 0.1f;
            //this.m_targetPosition += point.normalized * (this.m_velocity.magnitude * num3 * num * 1.5f);
            //m_rigidbody.AddForce(point.normalized * (this.m_velocity.magnitude * num * 1.5f), ForceMode.VelocityChange);
            m_rigidbody.velocity = point * num2 * 0.3f;
            m_targetSize = m_minDistance;
            m_currentSize = m_minDistance;
        }
        else
        {
            m_rigidbody.velocity = Vector3.zero;
        }
    }

    private void UpdateCurrentPosition()
    {
        float t3 = Mathf.Pow(1E-08f, Time.deltaTime);
        //this.m_currentPosition = Vector3.Lerp(this.m_targetPosition, this.m_currentPosition, t3);
        this.m_currentSize = Mathf.Lerp(this.m_targetSize, this.m_currentSize, t3);
        this.m_currentAngle.x = Mathf.LerpAngle(this.m_targetAngle.x, this.m_currentAngle.x, t3);
        //this.m_currentAngle.y = Mathf.Lerp(num, this.m_currentAngle.y, t3);
        this.m_currentAngle.y = Mathf.LerpAngle(this.m_targetAngle.y, this.m_currentAngle.y, t3);
        //this.m_currentHeight = Mathf.Lerp(this.m_targetHeight, this.m_currentHeight, t3);
    }

    private void UpdateTransform()
    {
        Quaternion rotation = Quaternion.AngleAxis(this.m_currentAngle.x, Vector3.up) * Quaternion.AngleAxis(this.m_currentAngle.y, Vector3.right);
        base.transform.rotation = rotation;
        transform.position = ClampHeight();
        //if (Mathf.Abs(m_targetSize - m_currentSize) < 0.01f)
        //{
        //    m_targetSize = m_minDistance;
        //    m_currentSize = m_minDistance;
        //}
        //if ((Mathf.Abs(m_currentSize) - m_minDistance) > 0f)
        //{
        //    if (m_isZoomIn)
        //    {
        //        //m_rigidbody.velocity = -transform.up * (m_currentSize - m_minDistance);
        //    }
        //    else
        //    {
        //        //m_rigidbody.velocity = transform.up * (m_currentSize - m_minDistance);
        //    }
        //}
    }

    private Vector3 ClampHeight()
    {
        Ray m_ray = new Ray(transform.position, Vector3.down);
        RaycastHit m_hit;
        if (transform.position.y < 20f && Physics.Raycast(m_ray, out m_hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("GuanLang")))
        {
            Vector3 v3 = transform.position;
            m_hit.point += Vector3.up * m_distanceToBottom;
            v3 += (m_hit.point - transform.position) * 0.1f;
            return v3;
        }
        return transform.position;
    }


    private bool Check()
    {
        Ray m_ray = new Ray(transform.position, Vector3.down);
        RaycastHit m_hit;
        if (transform.position.y >= 20f || !Physics.Raycast(m_ray, out m_hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("GuanLang")))
        {
            return true;
        }
        return false;
    }

    private void Disable()
    {
        this.enabled = false;
        OverLookCameraController overCtrl = GetComponent<OverLookCameraController>();
        overCtrl.enabled = true;
        float scale = (16 - transform.position.y) / Mathf.Clamp(transform.forward.y, 0.1f, 1f);
        overCtrl.m_targetPosition = transform.position + scale * transform.forward;
        overCtrl.m_targetPosition.y = 16;
        overCtrl.m_currentPosition = overCtrl.m_targetPosition;
        overCtrl.m_targetSize = Vector3.Distance(transform.position, overCtrl.m_targetPosition);
        overCtrl.m_currentSize = overCtrl.m_targetSize;
        overCtrl.m_targetAngle = m_targetAngle;
        overCtrl.m_currentAngle = m_currentAngle;

        GetComponent<SphereCollider>().enabled = false;
    }
}
