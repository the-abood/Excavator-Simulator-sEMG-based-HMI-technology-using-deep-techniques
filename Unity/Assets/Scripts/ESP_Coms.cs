using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

public class ESP_Coms : MonoBehaviour
{
    public Excavator Excavator;
    public string[] Results;
    public int FSR_Index, EMG_Index, OldValuesRecordSize = 50, EMG_Value, FSR_Value;
    public int EMG_Threshold = 2;
    public int FSR_Threshold = 2000;


    private List<int> EMGVals = new List<int>();
    private List<int> FSRVals = new List<int>();




    // Having data sent and recieved in a seperate thread to the main game thread stops unity from freezing
    Thread IOThread = new Thread(DataThread);
    private static SerialPort sp;
    // If the serial port class does not exist open your NuGet package manager Project->Manage NuGet Packages->Browse and search for
    // Serial Port. Install System.IO.Ports by Microsoft

    // Stores any data that comes in via the serial port
    private static string incomingMsg = "";


    // Stores the data to be sent to the arduino via the serial port
    //private static string outgoingMsg = "";

    private static void DataThread()
    {
        // Opens the serial port for reading and writing data
        sp = new SerialPort("/dev/cu.usbserial-0001", 115200); // Alter the first value to be whatever port the arduino is connected to within the arduino IDE; Alter the second value to be the same as Serial.beign at the start of the arduino program
        if (sp.IsOpen)
            sp.Close();
        sp.Open();

        // Every 200ms, it checks if there is a message stores in the output buffer string to be sent to the arduino,
        // Then recieves any data being sent to the project via the arduino 
        while (true)
        {
            //if (outgoingMsg != "")
            //{
            //    sp.Write(outgoingMsg);
            //    outgoingMsg = "";
            //}

            incomingMsg = sp.ReadLine();
            Thread.Sleep(25);
        }
    }

    private void OnDestroy()
    {
        // Closes the thread and serial port when the game ends
        IOThread.Abort();
        sp.Close();
    }

    // Start is called before the first frame update
    void Start()
    {
        IOThread.Start();
        Excavator = GetComponent<ExcavatorController>().excavator;
    }

    // Update is called once per frame
    void Update()
    {
        if (incomingMsg != "")
        {
            Results= incomingMsg.Split(",");
        }

        if (Results.Length >= EMG_Index)
            EMG_Value = getAverage(int.Parse(Results[EMG_Index]),EMGVals);

        if (Results.Length >= FSR_Index)
            FSR_Value = getAverage(int.Parse(Results[FSR_Index]), FSRVals);


        if(EMG_Value < EMG_Threshold)
            Excavator.boomRotate( 1);
        else
            Excavator.boomRotate( -1);

        if(FSR_Value < FSR_Threshold)
            Excavator.armRotate( -1);
        else
            Excavator.armRotate( 1);


        if(FSR_Value < FSR_Threshold)
            Excavator.bucketRotate( 1);
        else
            Excavator.bucketRotate( -1);
    }

    //Call with a new float to add each frame
    //The return value is the average of all floats, with a maximum of 10 floats stored
    private int getAverage(int newVal, List<int> valuesList)
    {
        valuesList.Add(newVal);
        if (valuesList.Count > OldValuesRecordSize)  //Remove the oldest when we have more than 10
        {
            valuesList.RemoveAt(0);
        }

        int total = 0;
        foreach (int f in valuesList)  //Calculate the total of all floats
        {
            total += f;
        }
        int average = total / (int)valuesList.Count;  //average is of course the total divided by the number of floats

        return average;
    }
}