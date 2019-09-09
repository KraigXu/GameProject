using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CrossSectionObjectSetup))]
public class Planar_xyzClippingSection : MonoBehaviour
{
    CrossSectionObjectSetup cs_setup;
    private GameObject xyzSectionPanel;
    private Text topSliderLabel, middleSliderLabel, bottomSliderLabel;
    private Slider slider;
    private Toggle xtoggle, ytoggle, ztoggle, gizmotoggle;

    //public Color sectionColor;

    //private List<Material> matList;
    //private List<Material> clipMatList;
    //private Renderer[] renderers;
    //private Dictionary<Renderer, int[]> matDict;

	//private Vector3 boundsCentre;
    private Vector3 sectionplane = Vector3.up;
	
	public Transform ZeroAlignment;
    //public bool accurateBounds = true;

    public enum ConstrainedAxis { X, Y, Z };
    public ConstrainedAxis selectedAxis = ConstrainedAxis.Y;

	//private float sliderRange = 0f;

    //public Bounds boundbox;

    private GameObject rectGizmo;

    private Vector3 zeroAlignmentVector = Vector3.zero;

    public bool gizmoOn = true;

    private Vector3 sliderRange = Vector3.zero;

    private float sectionX = 0;
    private float sectionY = 0;
    private float sectionZ = 0;

    void Awake()
    {
        //renderers = GetComponentsInChildren<Renderer>();
        xyzSectionPanel = GameObject.Find("xyzSectionPanel");
        if (xyzSectionPanel)
        {
            slider = xyzSectionPanel.GetComponentInChildren<Slider>();
            topSliderLabel = xyzSectionPanel.transform.Find("sliderPanel/MaxText").GetComponent<Text>();
            middleSliderLabel = xyzSectionPanel.transform.Find("sliderPanel/Slider").GetComponentInChildren<Text>();
            bottomSliderLabel = xyzSectionPanel.transform.Find("sliderPanel/MinText").GetComponent<Text>();
            if (xyzSectionPanel.transform.Find("axisOptions"))
            {
                xtoggle = xyzSectionPanel.transform.Find("axisOptions/Panel/X_Toggle").GetComponent<Toggle>();
                ytoggle = xyzSectionPanel.transform.Find("axisOptions/Panel/Y_Toggle").GetComponent<Toggle>();
                ztoggle = xyzSectionPanel.transform.Find("axisOptions/Panel/Z_Toggle").GetComponent<Toggle>();
                xtoggle.isOn = selectedAxis == ConstrainedAxis.X;
                ytoggle.isOn = selectedAxis == ConstrainedAxis.Y;
                ztoggle.isOn = selectedAxis == ConstrainedAxis.Z;
            }
            if (xyzSectionPanel.transform.Find("gizmoToggle"))
            {
                gizmotoggle = xyzSectionPanel.transform.Find("gizmoToggle").GetComponent<Toggle>();
                gizmotoggle.isOn = gizmoOn;
            }
        }
        if (ZeroAlignment) zeroAlignmentVector = ZeroAlignment.position;
        cs_setup = gameObject.GetComponent<CrossSectionObjectSetup>();
    }

	void Start () {	
        if (slider) slider.onValueChanged.AddListener(SliderListener);
        if (xyzSectionPanel) xyzSectionPanel.SetActive(enabled);
        Shader.DisableKeyword("CLIP_TWO_PLANES");
        Shader.EnableKeyword("CLIP_PLANE");
        Shader.SetGlobalVector("_SectionPlane", Vector3.up);
        if (xtoggle) xtoggle.onValueChanged.AddListener(delegate { SetAxis(xtoggle.isOn, ConstrainedAxis.X); });
        if (ytoggle) ytoggle.onValueChanged.AddListener(delegate { SetAxis(ytoggle.isOn, ConstrainedAxis.Y); });
        if (ztoggle) ztoggle.onValueChanged.AddListener(delegate { SetAxis(ztoggle.isOn, ConstrainedAxis.Z); });
        if (gizmotoggle) gizmotoggle.onValueChanged.AddListener(GizmoOn);

        sliderRange = new Vector3((float)SignificantDigits.CeilingToSignificantFigures((decimal)(1.08f * 2 * cs_setup.bounds.extents.x), 2),
        (float)SignificantDigits.CeilingToSignificantFigures((decimal)(1.08f * 2 * cs_setup.bounds.extents.y), 2),
        (float)SignificantDigits.CeilingToSignificantFigures((decimal)(1.08f * 2 * cs_setup.bounds.extents.z), 2));
        sectionX = cs_setup.bounds.min.x + sliderRange.x;
        sectionY = cs_setup.bounds.min.y + sliderRange.y;
        sectionZ = cs_setup.bounds.min.z + sliderRange.z;
        setupGizmo();
        setSection();
	}

    public void SliderListener(float value)
    {
        if (middleSliderLabel) middleSliderLabel.text = value.ToString("0.0");

        switch (selectedAxis)
        {
            case ConstrainedAxis.X:
                sectionX = value + zeroAlignmentVector.x;
                if (rectGizmo) rectGizmo.transform.position = new Vector3(sectionX, cs_setup.bounds.center.y, cs_setup.bounds.center.z);
                break;
            case ConstrainedAxis.Y:
                sectionY = value + zeroAlignmentVector.y;
                if (rectGizmo) rectGizmo.transform.position = new Vector3(cs_setup.bounds.center.x, sectionY, cs_setup.bounds.center.z);
                break;
            case ConstrainedAxis.Z:
                sectionZ = value + zeroAlignmentVector.z;
                if (rectGizmo) rectGizmo.transform.position = new Vector3(cs_setup.bounds.center.x, cs_setup.bounds.center.y, sectionZ);
                break;
        }
        Shader.SetGlobalVector("_SectionPoint", new Vector3(sectionX,sectionY,sectionZ));
    }

    public void SetAxis(bool b, ConstrainedAxis a)
    {
        if (b) 
        { 
            selectedAxis = a;
            Debug.Log(a);
            RectGizmo rg = rectGizmo.GetComponent<RectGizmo>();
            rg.transform.position = Vector3.zero;
            rg.SetSizedGizmo(cs_setup.bounds.size, selectedAxis);
            setSection();
        }
    }

    void setSection()
    {
        float sliderMaxVal = 0f;
        float sliderVal = 0f;
        float sliderMinVal = 0f;
        Vector3 sectionpoint = new Vector3(sectionX,sectionY,sectionZ);
        Debug.Log(cs_setup.bounds.ToString());
        Debug.Log(selectedAxis.ToString());
        switch (selectedAxis)
        {
            case ConstrainedAxis.X:
                sectionplane = Vector3.right;
                rectGizmo.transform.position = new Vector3(sectionX, cs_setup.bounds.center.y, cs_setup.bounds.center.z);
                sliderMaxVal = cs_setup.bounds.min.x + sliderRange.x - zeroAlignmentVector.x;
                sliderVal = sectionX - zeroAlignmentVector.x;
                sliderMinVal = cs_setup.bounds.min.x - zeroAlignmentVector.x;
                break;
            case ConstrainedAxis.Y:
                sectionplane = Vector3.up;
                rectGizmo.transform.position = new Vector3(cs_setup.bounds.center.x, sectionY, cs_setup.bounds.center.z);
                sliderMaxVal = cs_setup.bounds.min.y + sliderRange.y - zeroAlignmentVector.y;
                sliderVal = sectionY - zeroAlignmentVector.y;
                sliderMinVal = cs_setup.bounds.min.y - zeroAlignmentVector.y;
                break;
            case ConstrainedAxis.Z:
                sectionplane = Vector3.forward;
                rectGizmo.transform.position = new Vector3(cs_setup.bounds.center.x, cs_setup.bounds.center.y, sectionZ);
                sliderMaxVal = cs_setup.bounds.min.z + sliderRange.z - zeroAlignmentVector.z;
                sliderVal = sectionZ - zeroAlignmentVector.z;
                sliderMinVal = cs_setup.bounds.min.z - zeroAlignmentVector.z;
                break;
            default:
                Debug.Log("case default");
                break;
        }

        Shader.SetGlobalVector("_SectionPoint", sectionpoint);
        Shader.SetGlobalVector("_SectionPlane", sectionplane);


        if (topSliderLabel) topSliderLabel.text = sliderMaxVal.ToString("0.0");
        if (bottomSliderLabel) bottomSliderLabel.text = sliderMinVal.ToString("0.0");

        if (slider)
        {
            slider.maxValue = sliderMaxVal;

            slider.value = sliderVal;
            middleSliderLabel.text = sliderVal.ToString("0.0");

            slider.minValue = sliderMinVal;
        }
    }

    void setupGizmo()
    {
        rectGizmo = Resources.Load("rectGizmo") as GameObject;
        if(rectGizmo) Debug.Log("rectGizmo");
        if (cs_setup) Debug.Log("cs_setup");
        rectGizmo = Instantiate(rectGizmo, cs_setup.bounds.center + (-cs_setup.bounds.extents.y + (slider ? slider.value : 0) + zeroAlignmentVector.y) * transform.up, Quaternion.identity) as GameObject;

        RectGizmo rg = rectGizmo.GetComponent<RectGizmo>();

        rg.SetSizedGizmo(cs_setup.bounds.size, selectedAxis);
        /* Set rectangular gizmo size here: inner width, inner height, border width.
         */
        rectGizmo.SetActive(false);

    }

    void OnEnable()
    {
        Shader.EnableKeyword("CLIP_PLANE");
        if (xyzSectionPanel) xyzSectionPanel.SetActive(true);
        if (slider)
        {
            Shader.SetGlobalVector("_SectionPoint", new Vector3(sectionX, sectionY, sectionZ));
        }
    }

    void OnDisable()
    {
        if (xyzSectionPanel) xyzSectionPanel.SetActive(false);
        Shader.DisableKeyword("CLIP_PLANE");
        Shader.EnableKeyword("CLIP_SPHERE");
        //Shader.SetGlobalVector("_SectionPoint", boundbox.min + sliderRange);
    }

    void OnApplicationQuit()
    {
        Shader.DisableKeyword("CLIP_PLANE");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Collider coll = gameObject.GetComponent<Collider>();
            if (coll.Raycast(ray, out hit, 10000f))
            {
                if(gizmoOn) rectGizmo.SetActive(true);
                StartCoroutine(dragGizmo());
            }
            else 
            {
                rectGizmo.SetActive(false);
            }
        }
    }

    IEnumerator dragGizmo()
    {
        float cameraDistance = Vector3.Distance(cs_setup.bounds.center, Camera.main.transform.position);
        Vector3 startPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDistance));
        Vector3 startPos = rectGizmo.transform.position;
        Vector3 translation = Vector3.zero;
        Camera.main.GetComponent<maxCamera>().enabled = false;
        if (slider) slider.onValueChanged.RemoveListener(SliderListener);
        while (Input.GetMouseButton(0))
        {
            translation = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDistance)) - startPoint;
            Vector3 projectedTranslation = Vector3.Project(translation, sectionplane);
            Vector3 newPoint = startPos + projectedTranslation;
            switch (selectedAxis) { 
                case ConstrainedAxis.X:
                    if (newPoint.x > cs_setup.bounds.max.x) sectionX = cs_setup.bounds.max.x;
                    else if (newPoint.x < cs_setup.bounds.min.x) sectionX = cs_setup.bounds.min.x; 
                    else sectionX = newPoint.x;
                    break;
                case ConstrainedAxis.Y:
                    if (newPoint.y > cs_setup.bounds.max.y) sectionY = cs_setup.bounds.max.y;
                    else if (newPoint.y < cs_setup.bounds.min.y) sectionY = cs_setup.bounds.min.y;
                    else sectionY = newPoint.y;
                    break;
                case ConstrainedAxis.Z:
                    if (newPoint.z > cs_setup.bounds.max.z) sectionZ = cs_setup.bounds.max.z;
                    else if (newPoint.z < cs_setup.bounds.min.z) sectionZ = cs_setup.bounds.min.z;
                    else sectionZ = newPoint.z;
                    break;
            }
            setSection();
            yield return null;
        }
        Camera.main.GetComponent<maxCamera>().enabled = true;
        if (slider) slider.onValueChanged.AddListener(SliderListener);
    }

    public void GizmoOn(bool val) 
    {
        gizmoOn = val;
        if (rectGizmo) rectGizmo.SetActive(val);
    }

}
