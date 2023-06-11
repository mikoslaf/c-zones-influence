using CitizenFX.Core;
using CitizenFX.Server;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
    public class Zone
    {
        public string gang { get; set; }
        public double val { get; set; }
    }
    public class Server: BaseScript
    {
        private Dictionary<string, List<int>> influencers = new Dictionary<string, List<int>>();
        private Dictionary<int, Zone> Zones = new Dictionary<int, Zone>();
        public Server() 
        {
            Debug.WriteLine("1234");

            //EventHandlers["chatMessage"] += Func.Create<int, string, string>(addplayer);
            Events.RegisterEventHandler("c-zones-influence:addplayer", Func.Create<Player, string>(addplayer), Binding.All);
            Events.RegisterEventHandler("c-zones-influence:alarmplayer", Func.Create<string, string>(alarmplayer), Binding.All);
            Events.RegisterEventHandler("c-zones-influence:setzones", Func.Create<dynamic>(setzones), Binding.All);
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
                    if (v.Remove(source.Handle)) 
                    {
                        if (v.Count == 0) 
                        {
                            foreach (KeyValuePair<string, List<int>> i in influencers)
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
                    if (!influencers[gang].Exists(x => x == source.Handle)) 
                    {
                        influencers[gang].Add(source.Handle);
                    }
                }
                else 
                {
                    influencers.Add(gang,new List<int> { source.Handle });
                }
            }

            foreach (KeyValuePair<string, List<int>> v in influencers)
            {
                Debug.WriteLine("Key = {0}, Value = {1}", v.Key, v.Value);
                foreach (var item in v.Value) 
                {
                    Debug.Write(item.ToString() + " ");
                }
            }
        }

        private void alarmplayer(string gang, string note) 
        {
            //foreach (KeyValuePair<int, string> v in influencers)
            //{
            //    if (v.Value == gang) 
            //    {
            //        Debug.Write("alarm player {0}", v.Key);
            //    }
            //    Debug.WriteLine("Key = {0}, Value = {1}", v.Key, v.Value);
            //}
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
