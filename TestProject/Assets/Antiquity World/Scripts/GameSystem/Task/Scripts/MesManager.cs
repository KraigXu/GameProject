using UnityEngine;
using System.Collections;
using System;

public class MesManager : MonoSingletion<MesManager> {

    public event EventHandler<TaskEventArgs> checkEvent;

    public void Check(TaskEventArgs e)
    {
        checkEvent(this,e);
    }
}
