using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkDiscovery : NetworkDiscovery
{
    HUDNetworkManagerCostumized HUDnetManager;

    private float timeOut = 3f;

   private Dictionary<LanConnectionInfo, float> lanAdresses = new Dictionary<LanConnectionInfo, float>();

    private void Awake()
    {
        HUDnetManager = GetComponent<HUDNetworkManagerCostumized>();
    }


    public void StartListening()
    {
        base.Initialize();
        base.StartAsClient();
        //StartCoroutine(CleanupExpiredEntries());
    }


    /// <summary>
    /// When Start Hosting a Game, This function Is Called To Start Broadcasting
    /// </summary>
    public void StartBroadcast()
    {
        //StopBroadcast();
        base.Initialize();
        base.StartAsServer();
    }


    private IEnumerator CleanupExpiredEntries()
    {
        while (true)
        {
            bool changed = false;

            var keys = lanAdresses.Keys.ToList();

            foreach (var key in keys)
            {
                if(lanAdresses[key] <= Time.time)
                {
                    lanAdresses.Remove(key);
                    changed = true;
                }
            }
            if(changed)
            {
                //UpdateMatchInfos();
            }

            yield return new WaitForSeconds(timeOut);
            
        }

    }


    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);
        HUDnetManager.JoinRoom(fromAddress);

        return;
        LanConnectionInfo info = new LanConnectionInfo(fromAddress, data);

        if( lanAdresses.ContainsKey(info) == false )
        {
            lanAdresses.Add(info, Time.time + timeOut);

            // To send off an event to UI and say we have new matches
            HUDnetManager.JoinRoom(fromAddress);
        }
        else
        {
            lanAdresses[info] = Time.time + timeOut;
        }

    }

}
