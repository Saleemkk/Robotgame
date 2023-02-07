using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RobotControl : MonoBehaviour
{
    Dictionary<int, string> Robot_IPs = new Dictionary<int, string>();
    Dropdown dropdown;
    InputField ttsInput;
    Dropdown.OptionData optiondata;
    private void Start()
    {
        dropdown = GameObject.Find("Dropdown").GetComponent<Dropdown>();
        ttsInput = GameObject.Find("InputField").GetComponent<InputField>();
    }
    public void auth(int id, string ip)
    {
        if (!Robot_IPs.ContainsKey(id))
        {
            Robot_IPs[id] = ip;
            AddItemToDropdown(dropdown, id.ToString());
        }
        else if (Robot_IPs[id] != ip)
        {
            RemoveItemFromDropdown(dropdown, id.ToString());
            Robot_IPs[id] = ip;
            AddItemToDropdown(dropdown, id.ToString());
        }
    }
    private void RemoveItemFromDropdown(Dropdown dropdown, string item)
    {
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>(dropdown.options);
        options.RemoveAll(x => x.text == item);
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }
    private void AddItemToDropdown(Dropdown dropdown, string item)
    {
        Dropdown.OptionData newItem = new Dropdown.OptionData();
        newItem.text = item;
        dropdown.options.Add(newItem);
    }
    public void SendServoCommand()
    {
        StartCoroutine(SendServoCoroutine(Robot_IPs[int.Parse(dropdown.options[dropdown.value].text)]));
    }
    IEnumerator SendServoCoroutine(string ip)
    {
        UnityWebRequest www = UnityWebRequest.Get("http://" + ip + "/servoMovement?movements=1,2,3,1,");
        www.timeout = 1;
        yield return www.SendWebRequest();
        Debug.Log("http://" + ip + "/servoMovement?movements=1,2,3,1,");
    }
    public void SendTTSCommand()
    {
      //  GetComponent<TutorialExample>().TTS_saveFile(ttsInput.text);
        StartCoroutine(SendTTSCoroutine(Robot_IPs[int.Parse(dropdown.options[dropdown.value].text)]));
    }
    IEnumerator SendTTSCoroutine(string ip)
    {
        UnityWebRequest www = UnityWebRequest.Get("http://" + ip + "/play?file=1.mp3");
        www.timeout = 1;
        yield return www.SendWebRequest();
        Debug.Log("http://" + ip + "/play?file=1.mp3");
    }
    public void SendDisplayCommand()
    {
        StartCoroutine(SendDisplayCoroutine(Robot_IPs[int.Parse(dropdown.options[dropdown.value].text)]));
    }
    IEnumerator SendDisplayCoroutine(string ip)
    {
        UnityWebRequest www = UnityWebRequest.Get("http://" + ip + "/display?text=Hello");
        www.timeout = 1;
        yield return www.SendWebRequest();
        Debug.Log("http://" + ip + "/display?text=Hello");
    }
}

