using UnityEngine;
using System.IO;

public class HandleTextFile : MonoBehaviour
{

    public static void WriteString(string Message)
    {
        string path = "test.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(Message);
        writer.Close();

        //Re-import the file to update the reference in the editor
        
        /*
        AssetDatabase.ImportAsset(path);
        TextAsset asset = Resources.Load("test");
        */
    }


}
