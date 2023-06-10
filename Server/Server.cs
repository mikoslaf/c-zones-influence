using CitizenFX.Core;
using CitizenFX.Server;
using System;
using System.Collections.Generic;

namespace Server
{
    public class Zone
    {
        private string gang { get; set; }
        private float val { get; set; }
    }
    public class Server: BaseScript
    {
        private Dictionary<int, string> influencers = new Dictionary<int, string>();
        private Dictionary<int, Zone> Zones = new Dictionary<int, Zone>();
        public Server() 
        {
            Debug.WriteLine("1234");

            //EventHandlers["chatMessage"] += Func.Create<int, string, string>(addplayer);
            Events.RegisterEventHandler("c-zones-influence:player", Func.Create<Player, string>(addplayer), Binding.All);
            Events.RegisterEventHandler("c-zones-influence:alarmplayer", Func.Create<string, string>(alarmplayer), Binding.All);
            Events.RegisterEventHandler("c-zones-influence:setzones", Func.Create<dynamic>(setzones), Binding.All);
        }

        private void setzones(dynamic data) 
        {
            Debug.WriteLine($"{data}");
            foreach (var item in data)
            {
                string key = item.Key;
                string gang = item.Value.gang;
                double val = item.Value.val;

                Console.WriteLine($"Key: {key}, Gang: {gang}, Value: {val}");
            }
            //Zones.Add(0, new Zone { });
        }
        private void addplayer([Source] Player source, string gang) 
        {
            Debug.WriteLine(gang);
            if (gang == "none")
            {
                influencers.Remove(source.Handle);
            }
            else 
            {
                influencers.Add(source.Handle, gang);
            }

            foreach (KeyValuePair<int, string> v in influencers)
            {
                Console.WriteLine("Key = {0}, Value = {1}", v.Key, v.Value);
            }
            Debug.WriteLine($"Player {source.Handle}");
        }

        private void alarmplayer(string gang, string note) 
        {
            foreach (KeyValuePair<int, string> v in influencers)
            {
                if (v.Value == gang) 
                {
                    Debug.Write("alarm player {0}", v.Key);
                }
                Console.WriteLine("Key = {0}, Value = {1}", v.Key, v.Value);
            }
        }

    }

}
