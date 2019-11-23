using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IEnumeratorLoad : MonoBehaviour
{
    public int currentCount;
    public int currentWaitCount;
    public bool IsResourceClear = false;

    public List<IEnumerator> _runinglist = new List<IEnumerator>();
    public Queue<IEnumerator> _waitRunList=new Queue<IEnumerator>();
    public List<IEnumeratorBase> _runing = new List<IEnumeratorBase>();

    private IEnumerator _curRuningIe = null;
    private int _limit=10;                    //下载上限
    private bool IsShowLoad = false;
    private int index=0;

    private int GetIndex
    {
        get { return index++;}
    }

    public class IEnumeratorBase
    {
        public int Id;
        public IEnumerator Ie;

        public IEnumeratorBase(int id, IEnumerator ie)
        {
            Id = id;
            Ie = ie;
        }
    }

    /// <summary>
    /// 将新协程加入队列 并等待之前协程结束在执行
    /// </summary>
    /// <param name="ie"></param>
    public void AddIEnumerator(IEnumerator ie)
    {
        IsResourceClear = false;
        if (_runing.Count > _limit)
        {
           _waitRunList.Enqueue(ie);
        }
        else
        {
            IEnumeratorBase ieoj = new IEnumeratorBase(GetIndex, ie);
            _runing.Add(ieoj);
           StartCoroutine(RunIE(ieoj));
        }
    }

    void Update()
    {
        currentCount = _runing.Count;
        currentWaitCount = _waitRunList.Count;
    }

    /// <summary>
    /// 将新协程加入队列，但是会清除之前队列。
    /// 新加入的协程会立即开始执行
    /// </summary>
    /// <param name="ie"></param>
    public void AddIEnumeratorButClearBefor(IEnumerator ie)
    {
        _runinglist.Clear();
        if (_curRuningIe != null)
        {
            StopCoroutine(_curRuningIe);
        }
        StopCoroutine("WaitRealCoroutinesQueue");
        StartCoroutine("WaitRealCoroutinesQueue");
        _runinglist.Add(ie);
    }

    private IEnumerator RunIE(IEnumeratorBase ie )
    {
        yield return ie.Ie;
        ChangeValue(ie.Id);
    }

    /// <summary>
    /// 协程队列
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitRealCoroutinesQueue()
    {
        while (true)
        {

            if (_runinglist.Count == 0)
            {
                yield return null;
            }
            else
            {
                for (int i = 0; i < _runinglist.Count; i++)
                {
                    _curRuningIe = _runinglist[i];
                    yield return StartCoroutine(_curRuningIe);
                    _runinglist.Remove(_curRuningIe);
                    if (_runinglist.Count < _limit && _waitRunList.Count > 0)
                    {
                        _runinglist.Add(_waitRunList.Dequeue());
                    }
                    StopCoroutine(_curRuningIe);
                }
                _runinglist.Clear();
                _curRuningIe = null;
            }
        }
    }


    private void ChangeValue(int id)
    {
        for (int i = 0; i < _runing.Count; i++)
        {
            if (_runing[i].Id == id)
            {
                _runing.Remove(_runing[i]);
            }
        }

        if (_runing.Count < _limit &&_waitRunList.Count>0)
        {
            IEnumeratorBase ie = new IEnumeratorBase(GetIndex, _waitRunList.Dequeue());
            _runing.Add(ie);
            StartCoroutine(RunIE(ie));
        }
        else if (_runing.Count == 0 && _waitRunList.Count == 0 && IsResourceClear==false)
        {
            IsResourceClear = true;
           // Resources.UnloadUnusedAssets();
            Debuger.Log("IEnumeratorLoad Over! Resources Over!");
        }

    }
}
