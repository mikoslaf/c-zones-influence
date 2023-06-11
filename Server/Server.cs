using CitizenFX.Core;
using CitizenFX.Server;
using System;
using System.Collections.Generic;

namespace Server
{
    public class Zone
    {
        public string gang { get; set; }
        public double val { get; set; }
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
            foreach (var item in data)
            {
                Zones.Add(Convert.ToInt32(item.Key), new Zone { gang = item.Value.gang, val = item.Value.val });
            }

            influence(1, 0.01, "vagos");
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
                Debug.WriteLine("Key = {0}, Value = {1}", v.Key, v.Value);
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
                Debug.WriteLine("Key = {0}, Value = {1}", v.Key, v.Value);
            }
        }

        private void influence(int id, double val, string gang) 
        {
            if (Zones[id].gang == gang)
            {
                if (Zones[id].val != 1)
                {
                    double add = Zones[id].val + val;
                    if (add > 1)
                    {
                        Zones[id].val = 1;
                    }
                    else
                    {
                        Zones[id].val = add;
                    }

                    Events.TriggerEvent("qb-handler:c-zones-influence:influence", id, Zones[id].val);
                }
            }
            else
            {
                double remove = Zones[id].val - val;
                if (remove <= 0)
                {
                    Zones[id].val = 0.01;
                    Zones[id].gang = gang;
                    Events.TriggerEvent("qb-handler:c-zones-influence:influence", id, Zones[id].val, gang);
                }
                else
                {
                    Zones[id].val = remove;
                    Events.TriggerEvent("qb-handler:c-zones-influence:influence", id, Zones[id].val);
                }
            }

            foreach (KeyValuePair<int, Zone> v in Zones)
            {
                Debug.WriteLine("Key = {0}, Value = {1}", v.Value.gang, v.Value.val);
            }
        }

    }

}
