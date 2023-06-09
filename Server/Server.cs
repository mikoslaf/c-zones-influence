using CitizenFX.Core;
using CitizenFX.Server;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Server
{
    public class Server: BaseScript
    {
        private Dictionary<int, string> influencers = new Dictionary<int, string>();
        public Server() 
        {
            Debug.WriteLine("1234");

            //EventHandlers["chatMessage"] += Func.Create<int, string, string>(addplayer);
            Events.RegisterEventHandler("c-zones-influence:player", Func.Create<Player, string>(addplayer), Binding.All);
            Events.RegisterEventHandler("c-zones-influence:alarmplayer", Func.Create<string, string>(alarmplayer), Binding.All);
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
