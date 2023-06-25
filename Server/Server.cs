using CitizenFX.Core;
using CitizenFX.Server;
using CitizenFX.Server.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace Server
{
    public class Zone
    {
        public string gang { get; set; }
        public double val { get; set; }
    }
    public class Server: BaseScript
    {
        private readonly Dictionary<string, List<Player>> influencers = new Dictionary<string, List<Player>>();
        private readonly Dictionary<int, Zone> Zones = new Dictionary<int, Zone>();
        public Server() 
        {
            Events.RegisterEventHandler("c-zones-influence:addplayer", Func.Create<Player, string>(addplayer), Binding.All);
            Events.RegisterEventHandler("c-zones-influence:alarmplayer", Func.Create<string, string, string>(alarmplayer), Binding.All);
            Events.RegisterEventHandler("c-zones-influence:setzones", Func.Create<dynamic>(setzones), Binding.All);
            Events.RegisterEventHandler("c-zones-influence:server:influence", Func.Create<int, double, string, string, string>(influence), Binding.All);
            Events.RegisterEventHandler("c-zones-influence:server:nui_open", Func.Create<Player>(command), Binding.All);
            Natives.RegisterCommand("test123", Func.Create<Player>(command), false);
        }

        private void setzones(dynamic data) 
        {
            foreach (var item in data)
            {
                Zones.Add(Convert.ToInt32(item.Key), new Zone { gang = item.Value.gang, val = item.Value.val });
            }   
        }
        private void addplayer([Source] Player source, string gang) 
        {
            if (gang == "none")
            {
                foreach (var v in influencers.Values)
                {
                    if (v.Remove(source)) 
                    {
                        if (v.Count == 0) 
                        {
                            foreach (KeyValuePair<string, List<Player>> i in influencers)
                            {
                                if (!i.Value.Any()) 
                                {
                                    influencers.Remove(i.Key);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            else 
            {
                if (influencers.ContainsKey(gang))
                {
                    if (!influencers[gang].Exists(x => x == source)) 
                    {
                        influencers[gang].Add(source);
                    }
                }
                else 
                {
                    influencers.Add(gang,new List<Player> { source });
                }
            }

            foreach (KeyValuePair<string, List<Player>> v in influencers) // debug to delete
            {
                Debug.WriteLine("Key = {0}, Value = {1}", v.Key, v.Value);
                foreach (var item in v.Value) 
                {
                    Debug.Write(item.ToString() + " ");
                }
            }
        }

        private void alarmplayer(string gang, string note, string zone) 
        {
            foreach (var v in influencers[gang])
            {
                Events.TriggerClientEvent("QBCore:Notify", v, zone + " | " + note, "error", 8000);
            }
        }

        private void influence(int id, double val, string gang, string note, string name) 
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

                    if (Zones[id].val > 0.75) 
                    {
                        if(note  != "")
                        {
                            alarmplayer(Zones[id].gang, note, name);
                        }
                    }
                }
            }



            foreach (KeyValuePair<int, Zone> v in Zones)
            {
                Debug.WriteLine("Key = {0}, Value = {1}", v.Value.gang, v.Value.val);
            }
        }

        private void command([Source] Player source) 
        {
            string json = "{";
            foreach (KeyValuePair<int, Zone> v in Zones)
            {
                if(v.Value.val > 0.20)
                json += "\'"+ v.Key + "\': [\'" + v.Value.gang + "\',\'" + v.Value.val +"\'],";
                //Debug.WriteLine("Key = {0}, Value = {1}, Value = {2}", v.Key, v.Value.gang, v.Value.val);
            }
            json = json.Remove(json.Length - 1) + "}";
            Debug.WriteLine(json);

            Events.TriggerClientEvent("c-zones-influence:nui_open", source, json);
        }

    }

}
