using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class OverLookCameraController : MonoBehaviour
{
    public float m_defaultInertia;

    public Vector2 m_targetAngle = new Vector2(-28f, 20f);

    public Vector2 m_currentAngle = new Vector2(-28f, 20f);

    public Vector3 m_targetPosition = new Vector3(450f, 110f, -610f);

    public Vector3 m_currentPosition = new Vector3(450f, 110f, -610f);

    public float m_targetSize = 780f;

    public float m_currentSize = 780f;

    public float m_targetHeight = 60f;

    public float m_currentHeight = 60f;

    public float m_minDistance = 40f;

    public float m_maxDistance = 3000f;

    public float m_maxDistance2 = 6000;

    public float m_maxTiltDistance = 5000f;

    public LayerMask m_wallMask;

    public float m_terrainVisibileHeight = 100;

    public Bounds m_targetLimit;

    public float m_zoomVelocity;

    public Vector3 m_velocity;

    public Vector2 m_angleVelocity;

    public float m_originalNearPlane;

    public float m_originalFarPlane;

    public float m_mouseSensitivity = 0.5f;

    public Camera m_camera;

    public Ray m_ray;

    public RaycastHit m_hit;

    public Terrain[] m_terrains;

    public float m_detailObjectDistance = 80;

    public float m_treeDistance = 500;

    void Awake()
    {
        m_camera = GetComponent<Camera>();

        this.m_originalNearPlane = this.m_camera.nearClipPlane;
        this.m_originalFarPlane = this.m_camera.farClipPlane;
    }

    void Start()
    {
        m_terrains = Terrain.activeTerrains;
        Bounds bounds = m_terrains[0].terrainData.bounds;
        m_targetLimit.center = bounds.center;
        m_targetLimit.size = bounds.size;
        m_targetLimit.size = new Vector3(m_targetLimit.size.x, 100, m_targetLimit.size.z);
        //  gameObject.AddComponent<BleedBehavior>();
    }
    void LateUpdate()
    {
        UpdateTargetPosition();
        UpdateCurrentPosition();
        UpdateTransform();
        //UpdateTransformLate();
        if (Check())
        {

            Switch2Inside();
        }
        SetTerrainProperty();
        if (Mathf.Abs(m_maxDistance - m_currentSize) < 0.1f)
        {
            m_maxDistance = m_maxDistance2;
            //m_defaultInertia = 0.05f;
        }
        if (Mathf.Abs(m_maxDistance2 - m_currentSize) < 0.1f)
        {
            //BleedBehavior bleed = GameObject.Find("Main Camera").GetComponent<BleedBehavior>();
            //if (!bleed.enabled)
            //{
            //    bleed.FadeInFinished = FadeInFinished;
            //    bleed.enabled = true;
            //    bleed.FadeIn();
            //}
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawSphere(m_targetPosition, 10);
        Gizmos.DrawCube(m_targetLimit.center, m_targetLimit.size);
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
                num3 /= Mathf.Pow(1.1f, -this.m_zoomVelocity * num);
            }
            else
            {
                num3 *= Mathf.Pow(1.1f, this.m_zoomVelocity * num);
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
            this.m_targetPosition += point.normalized * (this.m_velocity.magnitude * num3 * num * 1.5f);
            this.m_targetPosition = ClampTargetPosition(this.m_targetPosition);
        }
    }

    private void UpdateCurrentPosition()
    {
        //float num = 90f - (90f - this.m_targetAngle.y) * (this.m_maxTiltDistance * 0.5f / (this.m_maxTiltDistance * 0.5f + this.m_targetSize));
        float t3 = Mathf.Pow(1E-08f, Time.deltaTime);
        this.m_currentPosition = Vector3.Lerp(this.m_targetPosition, this.m_currentPosition, t3);
        this.m_currentSize = Mathf.Lerp(this.m_targetSize, this.m_currentSize, t3);
        this.m_currentAngle.x = Mathf.LerpAngle(this.m_targetAngle.x, this.m_currentAngle.x, t3);
        //this.m_currentAngle.y = Mathf.Lerp(num, this.m_currentAngle.y, t3);
        this.m_currentAngle.y = Mathf.LerpAngle(this.m_targetAngle.y, this.m_currentAngle.y, t3);
        this.m_currentHeight = Mathf.Lerp(this.m_targetHeight, this.m_currentHeight, t3);
    }

    private void UpdateTransform()
    {
        //this.m_camera.nearClipPlane = Mathf.Min(this.m_originalNearPlane, this.m_currentSize * 0.005f);
        this.m_camera.nearClipPlane = this.m_currentSize * 0.015f;
        this.m_camera.farClipPlane = Mathf.Max(this.m_originalFarPlane, this.m_currentSize * 5f);
        //float num = this.m_currentSize * (1f - this.m_currentHeight / this.m_maxDistance) / Mathf.Tan(Mathf.Deg2Rad * this.m_camera.fieldOfView);
        float num = this.m_currentSize;
        Quaternion rotation = Quaternion.AngleAxis(this.m_currentAngle.x, Vector3.up) * Quaternion.AngleAxis(this.m_currentAngle.y, Vector3.right);
        Vector3 vector = this.m_currentPosition + rotation * new Vector3(0f, 0f, -num);
        vector = this.ClampCameraPosition(vector);
        //vector += this.m_cameraShake * Mathf.Sqrt(num);
        base.transform.rotation = rotation;
        base.transform.position = vector;
    }

    private void HandleMouseEvents(float multiplier)
    {
        Vector2 zero = Vector2.zero;
        if (Input.GetMouseButton(1))
        {
            zero = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
#if UNITY_WEBGL
            zero *= 0.3f;
#endif
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

        Vector3 zero2 = Vector3.zero;
        if (Input.GetMouseButton(2))
        {
            zero2 = new Vector3(-Input.GetAxis("Mouse X"), 0, -Input.GetAxis("Mouse Y"));
        }
        this.m_velocity += zero2;
    }

    private void HandleScrollWheelEvent(float multiplier)
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            float axis = Input.GetAxis("Mouse ScrollWheel");
#if UNITY_WEBGL
            axis *= 0.3f;
#endif
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

    private Vector3 ClampTargetPosition(Vector3 position)
    {
        if (position.x < m_targetLimit.min.x)
        {
            position.x = m_targetLimit.min.x;
        }
        if (position.x > m_targetLimit.max.x)
        {
            position.x = m_targetLimit.max.x;
        }
        if (position.z < m_targetLimit.min.z)
        {
            position.z = m_targetLimit.min.z;
        }
        if (position.z > m_targetLimit.max.z)
        {
            position.z = m_targetLimit.max.z;
        }
        return position;
    }

    private Vector3 ClampCameraPosition(Vector3 position)
    {
        float num = 8640f;
        if (position.x < -num)
        {
            position.x = -num;
        }
        if (position.x > num)
        {
            position.x = num;
        }
        if (position.z < -num)
        {
            position.z = -num;
        }
        if (position.z > num)
        {
            position.z = num;
        }
        Terrain activeTerrain = GetCurrentTerrain(position);
        float height;
        if (activeTerrain == null)
        {
            height = m_targetPosition.y;
        }
        else
        {
            height = activeTerrain.SampleHeight(position);
        }
        if (position.y < height + m_camera.nearClipPlane * 1.414f)
        {
            position.y = height + m_camera.nearClipPlane * 1.414f;
        }
        //position = GetWallPoint(position);
        return position;
    }

    private void Switch2Inside()
    {
        this.enabled = false;
        WalkInModelCameraController walk = GetComponent<WalkInModelCameraController>();
        walk.enabled = true;
        walk.m_targetAngle = m_targetAngle;
        walk.m_currentAngle = m_currentAngle;
        walk.m_targetSize = walk.m_minDistance;
        walk.m_currentSize = walk.m_minDistance;
        m_camera.nearClipPlane = 0.03f;
        walk.GetComponent<SphereCollider>().enabled = true;
    }

    private bool Check()
    {
        m_ray = new Ray(transform.position, Vector3.down);
        if (transform.position.y < 20f && Physics.Raycast(m_ray, out m_hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("GuanLang")))
        {
            return true;
        }
        return false;
    }

    private void SetTerrainProperty()
    {
        //for (int i = 0; i < m_terrains.Length; i++)
        //{
        //    m_terrains[i].detailObjectDistance = Mathf.Clamp(m_detailObjectDistance - transform.position.y, 0, m_detailObjectDistance);
        //    m_terrains[i].treeDistance = Mathf.Clamp(m_treeDistance - transform.position.y * 10, 0, m_treeDistance);
        //}
        //if (m_maxDistance != m_maxDistance2)
        //{
        //    RenderSettings.fogStartDistance = m_camera.farClipPlane / 2;
        //    RenderSettings.fogEndDistance = m_camera.farClipPlane - 100f;
        //}
        //else
        //{
        //    RenderSettings.fogStartDistance = Mathf.Clamp(m_maxDistance2 - m_currentSize, 0, m_camera.farClipPlane / 2);
        //    if (RenderSettings.fogStartDistance > RenderSettings.fogEndDistance - 10)
        //    {
        //        RenderSettings.fogEndDistance = RenderSettings.fogStartDistance + 10;
        //    }
        //}
        RenderSettings.fogEndDistance = m_camera.farClipPlane;
        for (int i = 0; i < m_terrains.Length; i++)
        {
            m_terrains[i].detailObjectDistance = Mathf.Clamp(m_detailObjectDistance - transform.position.y, 0, m_detailObjectDistance);
            m_terrains[i].treeDistance = m_treeDistance;
        }
    }

    private Terrain GetCurrentTerrain(Vector3 position)
    {
        int x = Convert.ToInt32(Math.Floor(position.x / 1000f));
        int z = Convert.ToInt32(Math.Floor(position.z / 1000f));
        //Debug.Log(x + "  " + z);
        GameObject go = GameObject.Find(string.Format("Terrain {0},{1}", x, z));
        if (go != null)
        {
            return go.GetComponent<Terrain>();
        }
        return null;
    }

}


