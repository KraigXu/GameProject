using System.IO;
using System;

public class FileReaderProxy
{
    public static StreamReader ReadFile(string fileName)
    {
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(fileName);
        }
        catch (Exception)
        {
            return null;
        }
        return sr;
    }
}
