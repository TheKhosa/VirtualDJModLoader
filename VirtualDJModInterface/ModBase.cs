using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDJModInterface
{
    public abstract class ModBase : IVdjMod
    {
        public abstract string GetModName();
        public abstract string GetModVersion();
        public abstract void Initialize();

        public virtual void ExecuteCommand(string command)
        {
            Console.WriteLine($"[{GetModName()}] Received command: {command}");
        }

        public virtual void OnDeckLoaded(int deckNumber)
        {
            Console.WriteLine($"[{GetModName()}] Deck {deckNumber} loaded");
        }

        public virtual void OnPlayStateChanged(int deckNumber, bool isPlaying)
        {
            Console.WriteLine($"[{GetModName()}] Deck {deckNumber} play state changed to: {isPlaying}");
        }

        public virtual void OnVolumeChanged(int deckNumber, float volume)
        {
            Console.WriteLine($"[{GetModName()}] Deck {deckNumber} volume changed to: {volume}");
        }
    }
}
