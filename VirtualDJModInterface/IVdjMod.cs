namespace VirtualDJModInterface
{
    public interface IVdjMod
    {
        string GetModName();
        string GetModVersion();
        void Initialize();
        void ExecuteCommand(string command);
        void OnDeckLoaded(int deckNumber);
        void OnPlayStateChanged(int deckNumber, bool isPlaying);
        void OnVolumeChanged(int deckNumber, float volume);
    }

}
