using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileReaderProxy  {
    public static StreamReader ReadFile(string fileName)
    {
        StreamReader st = null;
        try
        {
            st = File.OpenText(fileName);
        }
        catch (Exception)
        {
            return null;
        }

        return st;
    }
}
