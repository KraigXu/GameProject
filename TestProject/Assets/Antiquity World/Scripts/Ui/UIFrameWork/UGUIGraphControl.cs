using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GameSystem.Ui
{
    /// <summary>
    /// UGUI界面接口类。对UGUI接口分离
    /// </summary>
    public class UGUIGraphControl : MonoBehaviour
    {
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
        /// 为Toggle添加点击事件
        /// </summary>
        /// <param name="go"></param>
        /// <param name="call"></param>
        public static void ToggleAddListener(GameObject go, UnityAction<bool> call)
        {
            go.GetComponent<Toggle>().onValueChanged.AddListener(call);

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
            return go.GetComponent<Text>().text;
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

            go.GetComponent<Text>().text = content;

        }

        /// <summary>
        /// 设置 Text 的颜色
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="color"></param>
        public static void TextColorSet(GameObject obj, Color color)
        {
            obj.GetComponent<Text>().color = color;


        }

        /// <summary>
        /// 根据字数获取文本的整体宽度
        /// </summary>
        /// <param name="TextObj"></param>
        /// <returns></returns>
        public static float TextPreferredWidthGet(GameObject TextObj)
        {
            Text txt = TextObj.GetComponent<Text>();
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
            Text txt = TextObj.GetComponent<Text>();
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
            Text txt = TextObj.GetComponent<Text>();

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
            bool isEmptyNull = string.IsNullOrEmpty(_text.GetComponent<Text>().text);
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
        /// 当给InputField 注册事件
        /// </summary>
        /// <param name="go"></param>
        /// <param name="call"></param>
        public static void InputFieldAddListenerByEndEdit(GameObject go, UnityAction<string> call)
        {
            go.GetComponent<InputField>().onEndEdit.AddListener(call);
            //  go.GetComponent<InputField>().OnSubmit()
        }

        /// <summary>
        /// 隐藏 输入框光标
        /// </summary>
        /// <param name="go"></param>
        public static void SetInputFieldDeactivate(GameObject go)
        {
            go.GetComponent<InputField>().DeactivateInputField();
        }

        /// <summary>
        /// 当给InputField 注册事件
        /// </summary>
        /// <param name="go"></param>
        /// <param name="call"></param>
        public static void InputFieldAddListenerByValueChange(GameObject go, UnityAction<string> call)
        {
            go.GetComponent<InputField>().onValueChanged.AddListener(call);
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

        /// <summary>
        /// ScrollRect  注册事件
        /// </summary>
        public static void OnScrollRectValueChangedEventAddlistener(GameObject go, UnityAction<Vector2> call)
        {

            go.GetComponent<ScrollRect>().onValueChanged.AddListener(call);


        }


        /// <summary>
        /// button事件注册
        /// </summary>
        /// <param name="go"></param>
        /// <param name="call"></param>
        public static void SetButtonListener(GameObject go, UnityAction call)
        {
            Button temp = go.GetComponent<Button>();
            if (temp == null)
                temp = go.AddComponent<Button>();
            temp.onClick.RemoveAllListeners();
            temp.onClick.AddListener(call);
        }

        /// <summary>
        /// 设置button控件的属性
        /// </summary>
        /// <param name="Btn"></param>
        /// <param name="isEnable"></param>
        public static void SetButtonEnable(GameObject Btn, bool isEnable)
        {
            Btn.GetComponent<Button>().enabled = isEnable;
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
            go.GetComponent<Text>().fontSize = fontsize;
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