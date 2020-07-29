using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System;

namespace Verse.UIFrameWork
{
    public class UGUITools : MonoBehaviour
    {

        /// <summary>
        /// Save the specified binary data into the specified file.
        /// </summary>
        static public bool Save(string fileName, byte[] bytes)
        {
#if UNITY_WEBPLAYER || UNITY_FLASH || UNITY_METRO || UNITY_WP8 || UNITY_WP_8_1
        return false;
#else

            string path = Application.persistentDataPath + "/" + fileName;

            if (bytes == null)
            {
                //  if (File.Exists(path)) File.Delete(path);
                return false;
            }

            FileStream file = null;

            try
            {
                file = File.Create(path);
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
                return false;
            }

            file.Write(bytes, 0, bytes.Length);
            file.Close();
            return true;
#endif
        }

        /// <summary>
        /// Load all binary data from the specified file.
        /// </summary>

        static public byte[] Load(string fileName)
        {
#if UNITY_WEBPLAYER || UNITY_FLASH || UNITY_METRO || UNITY_WP8 || UNITY_WP_8_1
        return null;
#else

            // string path = Application.persistentDataPath + "/" + fileName;

            if (File.Exists(fileName))
            {
                return File.ReadAllBytes(fileName);
            }

            return null;
#endif
        }



        /// <summary>
        /// Get the rootmost object of the specified game object.
        /// </summary>

        static public GameObject GetRoot(GameObject go)
        {
            Transform t = go.transform;

            for (; ; )
            {
                Transform parent = t.parent;
                if (parent == null) break;
                t = parent;
            }

            return t.gameObject;
        }

        /// <summary>
        /// Instantiate an object and add it to the specified parent.
        /// </summary>

        static public GameObject AddChild(GameObject parent, GameObject prefab)
        {
            GameObject go = GameObject.Instantiate(prefab) as GameObject;
#if UNITY_EDITOR
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif
            if (go != null && parent != null)
            {
                Transform t = go.transform;
                t.SetParent(parent.transform);
                t.localPosition = Vector3.zero;
                t.localRotation = Quaternion.identity;
                t.localScale = Vector3.one;
                go.layer = parent.layer;
            }

            return go;
        }

        /// <summary>
        /// 将child 作为子物体移动到parent下
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="Child"></param>
        static public void ChangeParent(GameObject Parent, GameObject Child)
        {
            Child.transform.SetParent(Parent.transform);
            Child.transform.localPosition = Vector3.zero;
            Child.transform.localRotation = Quaternion.identity;
            Child.transform.localScale = Vector3.one;
            Child.layer = Parent.layer;
        }

        static public GameObject AddUnderlineByText(GameObject parent, GameObject prefab,
            UIEventTriggerListener.VoidDelegate _Click)
        {
            GameObject res = Resources.Load("Prefabs/MouseBlock") as GameObject;

            GameObject go = AddChild(parent, prefab);
            GameObject mouseBlock = AddChild(go, res); //刷入的是换鼠标的碰撞
            UnityEngine.UI.Text _underline = go.GetComponent<UnityEngine.UI.Text>();
            UnityEngine.UI.Text parenttext = parent.GetComponent<UnityEngine.UI.Text>();
            int length = parenttext.text.Length;
            _underline.text = "";
            for (int i = 0; i < length * 2; i++)
            {
                _underline.text += "_";
            }
            //Debug.Log(" _underline.flexibleHeight" + _underline.flexibleHeight);

            TextGenerator m_TextGenerator = _underline.cachedTextGeneratorForLayout;
            TextGenerationSettings m_TextGenerationSettings = _underline.GetGenerationSettings(Vector2.zero);
            float fWidth = m_TextGenerator.GetPreferredWidth(_underline.text, m_TextGenerationSettings);

            m_TextGenerationSettings =
                _underline.GetGenerationSettings(new Vector2(_underline.rectTransform.rect.x, 0.0f));
            float fHeight = m_TextGenerator.GetPreferredHeight(_underline.text, m_TextGenerationSettings);
           // mouseBlock.GetComponent<MouseImageControl>()._callback = _Click;
            SetTransformSize(mouseBlock, new Vector2(fWidth, fHeight));
            //mouseBlock.GetComponent<Rect>()
            _underline.rectTransform.anchorMin = Vector2.zero;
            _underline.rectTransform.anchorMax = Vector2.one;
            _underline.rectTransform.offsetMax = Vector2.zero;
            _underline.rectTransform.offsetMin = Vector2.zero;
            return go;
        }

        /// <summary>
        /// 给label添加下划线
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        static public GameObject AddUnderlineByText(GameObject label, UIEventTriggerListener.VoidDelegate _Click)
        {
            return AddUnderlineByText(label, label, _Click);
        }

        static public GameObject AddInterface(GameObject parent, string path)
        {
            //将 界面 作为子物体 放到parent 下

            GameObject prefab = Resources.Load(path) as GameObject;
            GameObject go = AddChild(parent, prefab);
            go.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            prefab = null;
            return go;
        }

        static public void DestoryAllChild(GameObject Parent)
        {
            //删除所有子物体
            for (int i = 0; i < Parent.transform.childCount; i++)
            {
                Destroy(Parent.transform.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// 删除 某个物体底下 除了名为 OneChildName之外 的所有子物体
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="OneChild"></param>
        static public void DestoryChildExceptOneChild(GameObject Parent, string OneChildName)
        {
            for (int i = 0; i < Parent.transform.childCount; i++)
            {
                GameObject ChildObj = Parent.transform.GetChild(i).gameObject;
                if (ChildObj.name != OneChildName)
                {
                    Destroy(ChildObj);
                }
            }
        }

        /// <summary>
        /// 判断字符串是否为数字
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        static public bool IsNumberCheck(string str, out int value)
        {
            try
            {
                value = Convert.ToInt32(str);
                return true;
            }
            catch
            {
                value = -1;
                return false;
            }

        }


        /// <summary>
        /// 根据后缀判断是否为 图片
        /// </summary>
        /// <param name="str">后缀后三位</param>
        /// <returns></returns>
        static public bool IsImageCheck(string str)
        {
            bool IsPhoto = false;

            if (str == "bmp" || str == "tif" || str == "iff" || str == "cpx" || str == "dwg" || str == "eps" ||
                str == "gif" || str == "ico" || str == "peg" || str == "pm5" || str == "jpg" || str == "png")
            {
                IsPhoto = true; //png
            }

            return IsPhoto;
        }

        /// <summary>
        /// 根据后缀判断是否为 Excel 文件
        /// </summary>
        /// <param name="str">后缀后三位</param>
        /// <returns></returns>
        static public bool IsExcelCheck(string str)
        {
            bool isExcel = false;
            if (str == "xlsx" || str == "xls")
            {
                isExcel = true;
            }

            return isExcel;
        }

        /// <summary>
        /// 根据后缀判断是否为 Excel 文件
        /// </summary>
        /// <param name="str">后缀后三位</param>
        /// <returns></returns>
        static public bool IsWordCheck(string str)
        {
            bool isWord = false;
            if (str == "doc" || str == "docx")
            {
                isWord = true;
            }

            return isWord;
        }

        /// <summary>
        /// 添加子节点
        /// Add child to target
        /// </summary>
        public static void AddChildToTarget(Transform target, Transform child)
        {
            child.SetParent(target);
            child.localScale = Vector3.one;
            child.localPosition = Vector3.zero;
            child.localEulerAngles = Vector3.zero;

            ChangeChildLayer(child, target.gameObject.layer);
        }

        /// <summary>
        /// 修改子节点Layer  NGUITools.SetLayer();
        /// Change child layer
        /// </summary>
        public static void ChangeChildLayer(Transform t, int layer)
        {
            t.gameObject.layer = layer;
            for (int i = 0; i < t.childCount; ++i)
            {
                Transform child = t.GetChild(i);
                child.gameObject.layer = layer;
                ChangeChildLayer(child, layer);
            }
        }

        /// <summary>
        /// 获得指定目标最大depth值
        /// Get the target Max depth
        /// </summary>
        public static int GetMaxTargetDepth(GameObject obj, bool includeInactive = false)
        {
            int minDepth = -1;
            //List<UIPanel> lsPanels = GetPanelSorted(obj, includeInactive);
            //if (lsPanels != null)
            //    return lsPanels[lsPanels.Count - 1].depth;
            return minDepth;
        }

        public static void SetTargetMinPanelDepth(GameObject obj, int depth)
        {
            //List<UIPanel> lsPanels = GameUtility.GetPanelSorted(obj, true);
            //if (lsPanels != null)
            //{
            //    int i = 0;
            //    while (i < lsPanels.Count)
            //    {
            //        lsPanels[i].depth = depth + i;
            //        i++;
            //    }
            //}
        }

        // <summary>
        /// 给目标添加Collider背景
        /// Add Collider Background for target
        /// </summary>
        public static GameObject AddColliderBgToTarget(GameObject target, string maskNames, bool isTransparent)
        {
            // 添加UIPaneldepth最小上面
            // 保证添加的Collider放置在屏幕中间
            Transform windowBg = UGUITools.FindDeepChild(target, "WindowBg");
            if (windowBg == null)
            {
                //GameObject targetParent = GameUtility.GetPanelDepthMaxMin(target, false, true);
                //if (targetParent == null)
                //    targetParent = target;

                GameObject temp = new GameObject("WindowBg");
                UGUITools.AddChildToTarget(target.transform, temp.transform);
                RectTransform rt = temp.AddComponent<RectTransform>();
                rt.offsetMax = Vector2.zero;
                rt.offsetMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.anchorMin = Vector2.zero;
                windowBg = rt.transform;
                windowBg.SetAsFirstSibling();
            }

            Transform Masker = UGUITools.FindDeepChild(target, "WindowColliderBg(Cool)");
            if (Masker == null)
            {

                Transform root = UICenterMasterManager.Instance.GetTargetRoot(UIWindowType.ForegroundLayer);
                GameObject res = Resources.Load("UIPrefab/Mask/" + maskNames) as GameObject;
                Masker = UGUITools.AddChild(root.gameObject, res).transform;
                Masker.name = "WindowColliderBg(Cool)";
                SetRectTransformOffset(Masker.gameObject, Vector2.zero, Vector2.zero);
                res = null;
                UGUITools.AddChildToTarget(windowBg, Masker.transform);

                if (isTransparent)
                {
                    Masker.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.1f);
                }
                else
                {
                    Masker.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.5f);
                }


                // add sprite or widget to ColliderBg
                //UIWidget widget = null;
                //if (!isTransparent)
                //    widget = NGUITools.AddSprite(windowBg.gameObject, altas, maskName);
                //else
                //    widget = NGUITools.AddWidget<UIWidget>(windowBg.gameObject);

                //widget.name = "WindowColliderBg(Cool)";
                //bg = widget.transform;

                //// fill the screen
                //// You can use the new Anchor system
                //UIStretch stretch = bg.gameObject.AddComponent<UIStretch>();
                //stretch.style = UIStretch.Style.Both;
                //// set relative size bigger
                //stretch.relativeSize = new Vector2(1.5f, 1.5f);

                //// set a lower depth
                //widget.depth = -5;

                //// set alpha
                //widget.alpha = 0.6f;

                //// add collider
                //NGUITools.AddWidgetCollider(bg.gameObject);
            }

            return Masker.gameObject;
        }

        /// <summary>
        /// Find Deep child with name
        /// </summary>
        public static Transform FindDeepChild(GameObject _target, string _childName)
        {
            Transform resultTrs = null;
            resultTrs = _target.transform.Find(_childName);
            if (resultTrs == null)
            {
                foreach (Transform trs in _target.transform)
                {
                    resultTrs = UGUITools.FindDeepChild(trs.gameObject, _childName);
                    if (resultTrs != null)
                        return resultTrs;
                }
            }

            return resultTrs;
        }

        /// <summary>
        /// Find component in Target Child
        /// </summary>
        public static T FindDeepChild<T>(GameObject _target, string _childName) where T : Component
        {
            Transform resultTrs = UGUITools.FindDeepChild(_target, _childName);
            if (resultTrs != null)
                return resultTrs.gameObject.GetComponent<T>();
            return (T)((object)null);
        }


        /// <summary>
        /// 获取RectTransform组件
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static RectTransform GetObjectRectTransform(GameObject go)
        {
            if (go != null)
            {
                return go.GetComponent<RectTransform>();
            }
            else
                return null;

        }

        /// <summary>
        /// 改变传入对象的offsetMax和offsetMin
        /// </summary>
        /// <param name="go"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public static RectTransform SetRectTransformOffset(GameObject go, Vector2 min, Vector2 max)
        {
            RectTransform _rect = go.GetComponent<RectTransform>();
            _rect.offsetMax = max;
            _rect.offsetMin = min;
            return _rect;
        }

        /// <summary>
        /// 获得 RectTransform 的一些基本属性
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static float GetTransformAnchoredPositionX(GameObject obj)
        {

            return obj.GetComponent<RectTransform>().anchoredPosition.x;

        }

        public static float GetTransformAnchoredPositionY(GameObject obj)
        {

            return obj.GetComponent<RectTransform>().anchoredPosition.y;

        }

        public static float GetTransformAnchoredHeight(GameObject obj)
        {

            return obj.GetComponent<RectTransform>().rect.height;
        }

        public static float GetTransformAnchoredWidth(GameObject obj)
        {

            return obj.GetComponent<RectTransform>().rect.width;

        }

        public static Vector2 GetTransformAnchoredPostion(GameObject Obj)
        {

            return Obj.GetComponent<RectTransform>().anchoredPosition;

        }

        /// <summary>
        /// 控制缩放
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="vector2"></param>
        public static void SetTransformScale(GameObject obj, Vector2 vector2)
        {

            obj.GetComponent<RectTransform>().localScale = vector2;

        }

        /// <summary>
        /// 设置transformsize
        /// </summary>
        /// <param name="go"></param>
        /// <param name="size"></param>
        public static void SetTransformSize(GameObject go, Vector2 size)
        {
            go.GetComponent<RectTransform>().sizeDelta = size;
        }

        //设置宽与高
        public static void SetTransformWidthHeight(GameObject obj, Vector2 Vector2WidthHeight)
        {
            // Vector2(Width,Height)
            obj.GetComponent<RectTransform>().sizeDelta = Vector2WidthHeight;
        }

        //设置相对于父级 右边的距离，DiatanceNum 为距离右边的值，WidthNum 为 本身的宽度
        public static void SetTransformRight(GameObject obj, float DiatanceNum, float WidthNum)
        {
            obj.GetComponent<RectTransform>()
                .SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, DiatanceNum, WidthNum);
        }

        /// <summary>
        /// 设置传入对象的旋转角度
        /// </summary>
        /// <param name="go"></param>
        /// <param name="qa"></param>
        public static RectTransform SetRectTransformlocalRotation(GameObject go, Quaternion qa)
        {
            RectTransform RT = go.GetComponent<RectTransform>();
            RT.localRotation = qa;
            return RT;
        }


        /// <summary>
        /// 设置传入对象的anchoredPosition
        /// </summary>
        /// <param name="go"></param>
        /// <param name="pos"></param>
        public static RectTransform SetRectTransformAnchoredPosition(GameObject go, Vector2 pos)
        {
            RectTransform RT = go.GetComponent<RectTransform>();
            RT.anchoredPosition = pos;
            return RT;
        }

        /// <summary>
        /// 设置传入对象的anchoredPosition，offsetMin，offsetMax
        /// </summary>
        /// <param name="go"></param>
        /// <param name="Anchoredpos"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static RectTransform SetRectTransformAnchoredAndOffset(GameObject go, Vector2 Anchoredpos, Vector2 min,
            Vector2 max)
        {
            RectTransform RT = go.GetComponent<RectTransform>();
            RT.offsetMin = min;
            RT.offsetMax = max;
            RT.anchoredPosition = Anchoredpos;
            return RT;
        }

        /// <summary>
        /// 设置Pivot属性
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="PivotVector2"></param>
        public static void SetRectTransformPivot(GameObject obj, Vector2 PivotVector2)
        {
            obj.GetComponent<RectTransform>().pivot = PivotVector2;

        }

        /// <summary>
        /// 设置图片脚本显示状态
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="isenable"></param>
        public static void SetImageEnable(GameObject bo, bool isenable)
        {
            bo.GetComponent<Image>().enabled = isenable;
        }

        /// <summary>
        /// 将target的图片复制给original对象
        /// </summary>
        /// <param name="Original"></param>
        /// <param name="target"></param>
        public static void ChangeImagePic(GameObject Original, GameObject target)
        {
            Original.GetComponent<Image>().sprite = target.GetComponent<Image>().sprite;

        }

        /// <summary>
        /// 设置 图片的颜色 值
        /// </summary>
        /// <param name="_objImage"></param>
        /// <param name="NewColor"></param>
        public static void SetImageColor(GameObject _objImage, Color32 NewColor)
        {
            _objImage.GetComponent<Image>().color = NewColor;
        }

        /// <summary>
        /// 设置 图片 原本的格式
        /// </summary>
        /// <param name="_Image"></param>
        public static void SetImageNativeSize(GameObject _Image)
        {
            _Image.GetComponent<Image>().SetNativeSize();

        }

        /// <summary>
        /// 设置Toggle按钮状态/没有可以传空
        /// </summary>
        /// <param name="toggle"></param>
        /// <param name="disabledSprite"></param>
        /// <param name="highlightedSprite"></param>
        /// <param name="pressedSprite"></param>
        public static void SetToggleSpriteState(GameObject toggle, GameObject disabledSprite,
            GameObject highlightedSprite, GameObject pressedSprite)
        {
            Sprite _disabledSprite = null;
            Sprite _highlightedSprite = null;
            Sprite _pressedSprite = null;

            if (disabledSprite != null)
            {
                _disabledSprite = disabledSprite.GetComponent<Image>().sprite;
            }

            if (highlightedSprite != null)
            {
                _highlightedSprite = highlightedSprite.GetComponent<Image>().sprite;
            }

            if (pressedSprite != null)
            {
                _pressedSprite = pressedSprite.GetComponent<Image>().sprite;
            }

            toggle.GetComponent<Toggle>().spriteState = new SpriteState
            {
                disabledSprite = _disabledSprite,
                highlightedSprite = _highlightedSprite,
                pressedSprite = _pressedSprite

            };
        }


        /// <summary>
        /// 为TOGGLE设置值
        /// </summary>
        /// <param name="go"></param>
        /// <param name="istrue"></param>
        public static void SetToggleValue(GameObject go, bool istrue)
        {
            go.GetComponent<Toggle>().isOn = istrue;
        }

        /// <summary>
        /// 设置 Toogle的Group
        /// </summary>
        /// <param name="ToggleGroupObj"></param>
        /// <param name="Toggleobj"></param>
        public static void SetToggleGroup(GameObject ToggleGroupObj, GameObject Toggleobj, bool IsNotMutex)
        {

            ToggleGroup ToggleGroup = ToggleGroupObj.GetComponent<ToggleGroup>();
            Toggleobj.GetComponent<Toggle>().group = ToggleGroup;
            ToggleGroup.allowSwitchOff = IsNotMutex; //是否互斥

        }

        public static bool GetToggleValue(GameObject ToggleObj)
        {
            bool isTrue;
            isTrue = ToggleObj.GetComponent<Toggle>().isOn;
            return isTrue;

        }

        /// <summary>
        /// 设置 属性，方便 传更多不同的值
        /// </summary>
        /// <param name="Toggleobj"></param>
        /// <returns></returns>
        public static Toggle SetObjToToggle(GameObject Toggleobj)
        {
            Toggle ToggleObj = Toggleobj.GetComponent<Toggle>();
            return ToggleObj;

        }

        /// <summary>
        /// 设置 toggle是否可用
        /// </summary>
        /// <param name="isEnable"></param>
        /// <param name="_toggle"></param>
        public static void SetToggleIsEnable(GameObject _toggle, bool isEnable)
        {
            _toggle.GetComponent<Toggle>().enabled = isEnable;

        }


        /// <summary>
        /// 获取转入的TEXT对象的值
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static string TextGetValue(GameObject go)
        {
            return go.GetComponent<UnityEngine.UI.Text>().text;
        }

        /// <summary>
        /// 设置显示的文字的 内容
        /// </summary>
        /// <param name="go">挂了Label 脚本的 object</param>
        /// <param name="content">label 需要显示的内容</param>
        /// <param name="AlternativeText">当传入的 content 为空值时 文字替代显示的内容</param>
        public static void TextValueSet(GameObject go, string content, string AlternativeText = "")
        {
            if (string.IsNullOrEmpty(content))
            {
                if (string.IsNullOrEmpty(AlternativeText))
                {
                    content = "";
                }
                else
                {
                    content = AlternativeText;
                }

            }

            go.GetComponent<UnityEngine.UI.Text>().text = content;

        }

        /// <summary>
        /// 设置 Text 的颜色
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="color"></param>
        public static void TextColorSet(GameObject obj, Color color)
        {
            obj.GetComponent<UnityEngine.UI.Text>().color = color;


        }

        /// <summary>
        /// 根据字数获取文本的整体宽度
        /// </summary>
        /// <param name="TextObj"></param>
        /// <returns></returns>
        public static float TextPreferredWidthGet(GameObject TextObj)
        {
            UnityEngine.UI.Text txt = TextObj.GetComponent<UnityEngine.UI.Text>();
            float LineWidth = txt.preferredWidth;
            return LineWidth;
        }

        /// <summary>
        /// 根据字数获取文本的整体高度
        /// </summary>
        /// <param name="TextObj"></param>
        /// <returns></returns>
        public static float TextPreferredHeightGet(GameObject TextObj)
        {
            UnityEngine.UI.Text txt = TextObj.GetComponent<UnityEngine.UI.Text>();
            float LineHeight = txt.preferredHeight;
            return LineHeight;
        }

        /// <summary>
        /// 计算 文本有多少行
        /// </summary>
        /// <param name="TextObj"></param>
        /// <returns></returns>
        public static int TextSingleLineGet(GameObject TextObj)
        {
            UnityEngine.UI.Text txt = TextObj.GetComponent<UnityEngine.UI.Text>();

            string TempText = txt.text;
            float TotalHeight = TextPreferredHeightGet(TextObj); //获取Text整体高度

            txt.text = "";
            float Height = txt.preferredHeight; //获取 Text 单行的高度
            txt.text = TempText;
            return (int)(TotalHeight / Height);
        }


        /// <summary>
        /// 判断 Text有没有值
        /// </summary>
        /// <param name="_text"></param>
        /// <returns></returns>
        public static bool IsTextEmptyNull(GameObject _text)
        {
            bool isEmptyNull = string.IsNullOrEmpty(_text.GetComponent<UnityEngine.UI.Text>().text);
            return isEmptyNull;

        }

        /// <summary>
        /// 获得InputField输入的字符
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public static string GetInputFieldValue(GameObject Obj)
        {
            string Value = Obj.GetComponent<InputField>().text;
            return Value;
        }

        /// <summary>
        /// 设置 inputfield 输入框中显示的内容
        /// </summary>
        /// <param name="_inputField"></param>
        /// <param name="Content"></param>
        public static void SetInputFieldValue(GameObject _inputField, string Content)
        {
            if (string.IsNullOrEmpty(Content))
            {
                Content = "";
            }

            _inputField.GetComponent<InputField>().text = Content;

        }

        /// <summary>
        /// 隐藏 输入框光标
        /// </summary>
        /// <param name="go"></param>
        public static void SetInputFieldDeactivate(GameObject go)
        {
            go.GetComponent<InputField>().DeactivateInputField();
        }

        public static InputField SetObjToInputField(GameObject _inputField)
        {
            InputField _InputField = _inputField.GetComponent<InputField>();

            return _InputField;
        }

        /// <summary>
        /// 判断输入框有没有值
        /// </summary>
        /// <param name="_inputField"></param>
        /// <returns></returns>
        public static bool IsInputFieldEmptyNull(GameObject _inputField)
        {
            bool IsEmptyNull = string.IsNullOrEmpty(_inputField.GetComponent<InputField>().text);
            return IsEmptyNull;
        }


        /// <summary>
        /// 通过 输入 数值来控制ScrollView 所显示的 位置
        /// </summary>
        /// <param name="ScrollView"></param>
        /// <param name="ScrollBarValue"></param>

        public static void ScrollViewPostionControl(GameObject ScrollView, float ScrollBarValue, bool IsVertical)
        {
            if (IsVertical)
            {
                ScrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = ScrollBarValue;

            }

            else
            {
                ScrollView.GetComponent<ScrollRect>().horizontalNormalizedPosition = ScrollBarValue;

            }

        }


        public static void SetActive(GameObject go, bool istrue)
        {
            //go.transform.localScale
            if (go == null)
            {
                return;
            }

            if (istrue)
            {
                go.transform.localScale = Vector3.one;
            }
            else
                go.transform.localScale = new Vector3(0, 1f, 1f);

            go.SetActive(istrue);
        }

        public static Image GetImageConment(GameObject go)
        {
            return go.GetComponent<Image>();
        }



        /// <summary>
        /// 设置 属性，方便 传更多不同的值
        /// </summary>
        /// <param name="_slider"></param>
        /// <returns></returns>
        public static Slider SetObjToSlider(GameObject _slider)
        {
            Slider sliderobj = _slider.GetComponent<Slider>();
            return sliderobj;

        }

        /// <summary>
        /// 设置slider值
        /// </summary>
        /// <param name="_slider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Slider SetObjToSliderValue(GameObject _slider, float value)
        {
            Slider sliderobj = _slider.GetComponent<Slider>();
            sliderobj.value = value;
            return sliderobj;

        }

        /// <summary>
        /// 设置字体大小
        /// </summary>
        /// <param name="go"></param>
        /// <param name="fontsize"></param>
        public static void SetFontSize(GameObject go, int fontsize)
        {
            go.GetComponent<UnityEngine.UI.Text>().fontSize = fontsize;
        }


        /// <summary>
        /// 子对象坐标转换到Canvas的局部坐标
        /// </summary>
        /// <param name="canvasRect"></param>
        /// <param name="canvasCamera"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static void TransformToCanvasLocalPosition(RectTransform canvasRect, Camera canvasCamera,
            RectTransform target)
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, target.position, canvasCamera,
                out pos))
            {
                Debug.Log(pos);
            }

            target.anchoredPosition = pos;
        }
    }
}