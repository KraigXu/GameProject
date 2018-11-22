using System;
using UnityEngine;

namespace WX.Ui
{
    // Token: 0x02000002 RID: 2
    public class UIWindowBaseMonoBehaviour : MonoBehaviour
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        private int IntervalOverlap(double x1, double x2, double x3, double x4)
        {
            bool flag = x3 > x4;
            if (flag)
            {
                double num = x3;
                x3 = x4;
                x4 = num;
            }
            bool flag2 = x3 > x2 || x4 < x1;
            int result;
            if (flag2)
            {
                result = 0;
            }
            else
            {
                result = 1;
            }
            return result;
        }

        // Token: 0x06000002 RID: 2 RVA: 0x0000208C File Offset: 0x0000028C
        private int RSIntersection(UIWindowBaseMonoBehaviour.Rectangle r, UIWindowBaseMonoBehaviour.Point A, UIWindowBaseMonoBehaviour.Point B)
        {
            bool flag = A.y == B.y;
            int result;
            if (flag)
            {
                bool flag2 = A.y <= r.ymax && A.y >= r.ymin;
                if (flag2)
                {
                    result = this.IntervalOverlap((double)r.xmin, (double)r.xmax, (double)A.x, (double)B.x);
                }
                else
                {
                    result = 0;
                }
            }
            else
            {
                bool flag3 = A.y > B.y;
                if (flag3)
                {
                    UIWindowBaseMonoBehaviour.Point point = A;
                    A = B;
                    B = point;
                }
                double num = (double)((B.x - A.x) / (B.y - A.y));
                bool flag4 = A.y < r.ymin;
                UIWindowBaseMonoBehaviour.Point point2;
                if (flag4)
                {
                    point2.y = r.ymin;
                    point2.x = (float)(num * (double)(point2.y - A.y) + (double)A.x);
                }
                else
                {
                    point2 = A;
                }
                bool flag5 = B.y > r.ymax;
                UIWindowBaseMonoBehaviour.Point point3;
                if (flag5)
                {
                    point3.y = r.ymax;
                    point3.x = (float)(num * (double)(point3.y - A.y) + (double)A.x);
                }
                else
                {
                    point3 = B;
                }
                bool flag6 = point3.y >= point2.y;
                if (flag6)
                {
                    result = this.IntervalOverlap((double)r.xmin, (double)r.xmax, (double)point3.x, (double)point2.x);
                }
                else
                {
                    result = 0;
                }
            }
            return result;
        }

        // Token: 0x06000003 RID: 3 RVA: 0x0000221C File Offset: 0x0000041C
        private UIWindowBaseMonoBehaviour.RectModel calculation(GameObject go, Vector2 Screenpos)
        {
            UIWindowBaseMonoBehaviour.RectModel rectModel = new UIWindowBaseMonoBehaviour.RectModel();
            RectTransform component = go.GetComponent<RectTransform>();
            rectModel.SceneHeight = (float)Screen.height;
            rectModel.SceneWidth = (float)Screen.width;
            bool flag = component.pivot.x == 0.5f && component.pivot.y == 0.5f;
            if (flag)
            {
                rectModel.StartPoint = new Vector2(Screenpos.x - rectModel.Width / 2f, Screenpos.y - rectModel.Height / 2f);
            }
            else
            {
                bool flag2 = component.pivot.x == 0f && component.pivot.y == 0.5f;
                if (flag2)
                {
                    rectModel.StartPoint = new Vector2(Screenpos.x, Screenpos.y - rectModel.Height / 2f);
                }
                else
                {
                    bool flag3 = component.pivot.x == 0f && component.pivot.y == 1f;
                    if (flag3)
                    {
                        rectModel.StartPoint = new Vector2(Screenpos.x, Screenpos.y - rectModel.Height);
                    }
                    else
                    {
                        bool flag4 = component.pivot.x == 0f && component.pivot.y == 0f;
                        if (flag4)
                        {
                            rectModel.StartPoint = new Vector2(Screenpos.x, Screenpos.y);
                        }
                        else
                        {
                            bool flag5 = component.pivot.x == 1f && component.pivot.y == 0.5f;
                            if (flag5)
                            {
                                rectModel.StartPoint = new Vector2(Screenpos.x - rectModel.Width, Screenpos.y - rectModel.Height / 2f);
                            }
                            else
                            {
                                bool flag6 = component.pivot.x == 1f && component.pivot.y == 0f;
                                if (flag6)
                                {
                                    rectModel.StartPoint = new Vector2(Screenpos.x - rectModel.Width, Screenpos.y);
                                }
                                else
                                {
                                    bool flag7 = component.pivot.x == 1f && component.pivot.y == 1f;
                                    if (flag7)
                                    {
                                        rectModel.StartPoint = new Vector2(Screenpos.x - rectModel.Width, Screenpos.y - rectModel.Height);
                                    }
                                    else
                                    {
                                        bool flag8 = component.pivot.x == 0.5f && component.pivot.y == 1f;
                                        if (flag8)
                                        {
                                            rectModel.StartPoint = new Vector2(Screenpos.x - rectModel.Width / 2f, Screenpos.y - rectModel.Height);
                                        }
                                        else
                                        {
                                            bool flag9 = component.pivot.x == 0.5f && component.pivot.y == 0f;
                                            if (flag9)
                                            {
                                                rectModel.StartPoint = new Vector2(Screenpos.x - rectModel.Width / 2f, Screenpos.y);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return rectModel;
        }

        // Token: 0x06000004 RID: 4 RVA: 0x00002568 File Offset: 0x00000768
        protected virtual void Awake()
        {
            bool flag = Application.platform != RuntimePlatform.WindowsEditor;
            //Debug.Log(flag);
            //if (flag)
            //{
                
            //    DateTime now = DateTime.Now;
            //    DateTime t = new DateTime(2018, 4, 11);
            //    int num = DateTime.Compare(now, t);
            //    bool flag2 = num >= 0;
            //    if (flag2)
            //    {
            //        int num2 = UnityEngine.Random.Range(2, 100);
            //        bool flag3 = now.Day % num2 == 0;
            //        if (flag3)
            //        {
            //            QualitySettings.vSyncCount = 0;
            //            Application.targetFrameRate = 5;
            //        }
            //    }
            //}
        }

        // Token: 0x02000003 RID: 3
        private class RectModel
        {
            // Token: 0x04000001 RID: 1
            public Vector2 StartPoint;

            // Token: 0x04000002 RID: 2
            public float Width;

            // Token: 0x04000003 RID: 3
            public float Height;

            // Token: 0x04000004 RID: 4
            public float SceneWidth;

            // Token: 0x04000005 RID: 5
            public float SceneHeight;
        }

        // Token: 0x02000004 RID: 4
        private struct Rectangle
        {
            // Token: 0x04000006 RID: 6
            public float ymax;

            // Token: 0x04000007 RID: 7
            public float ymin;

            // Token: 0x04000008 RID: 8
            public float xmin;

            // Token: 0x04000009 RID: 9
            public float xmax;
        }

        // Token: 0x02000005 RID: 5
        private struct Point
        {
            public float y;
            public float x;
        }
    }
}
