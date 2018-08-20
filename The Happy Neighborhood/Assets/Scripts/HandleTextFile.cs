using UnityEngine;
using System.IO;
using UnityEditor;

public class HandleTextFile : MonoBehaviour
{
    static string path = "ConnectionLog.txt";
    static StreamWriter writer;

    public static void CreateStreamLog()
    {

        writer = new StreamWriter(path, true);
        writer.WriteLine("<< Log File >>");
        writer.Close();

        //Re-import the file to update the reference in the editor

        /*
        */
    }


    public static void WriteString(string Message)
    {
        writer = new StreamWriter(path, true);
        writer.WriteLine(Message);
        writer.Close();
    }

}
