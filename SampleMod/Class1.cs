using VirtualDJModInterface;

namespace SampleMod
{
    public class SampleMod : IVdjMod
    {
        public void Initialize()
        {
            Console.WriteLine("Sample Mod Initialized");
        }

        public void ExecuteCommand(string command)
        {
            Console.WriteLine($"Sample Mod received command: {command}");
        }

        public void OnDeckLoaded(int deckNumber)
        {
            throw new NotImplementedException();
        }

        public void OnPlayStateChanged(int deckNumber, bool isPlaying)
        {
            throw new NotImplementedException();
        }

        public void OnVolumeChanged(int deckNumber, float volume)
        {
            throw new NotImplementedException();
        }

        public string GetModName() => "Sample Mod";

        public string GetModVersion() => "1.0.0";
    }
}
