// VirtualDJPluginLoader/ModLoader.cs
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VirtualDJModInterface;

namespace VirtualDJPluginLoader
{
    [ComVisible(true)]
    [Guid("ED8A8D87-F4F9-4DCD-BD24-291412E93B60")]
    [ClassInterface(ClassInterfaceType.None)]
    public class ModLoader
    {
        private readonly List<IVdjMod> _loadedMods = new List<IVdjMod>();
        private readonly string _modsPath;
        private FileSystemWatcher _watcher;

        public ModLoader()
        {
            _modsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mods");
            InitializeModWatcher();
        }

        private void InitializeModWatcher()
        {
            if (!Directory.Exists(_modsPath))
            {
                Directory.CreateDirectory(_modsPath);
            }

            _watcher = new FileSystemWatcher(_modsPath, "*.dll")
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime,
                EnableRaisingEvents = true
            };

            _watcher.Created += (s, e) => LoadMod(e.FullPath);
            _watcher.Deleted += (s, e) => UnloadMod(e.FullPath);
        }

        public void LoadMods(string modsDirectory = null)
        {
            string directory = modsDirectory ?? _modsPath;

            if (!Directory.Exists(directory))
            {
                Console.WriteLine($"Creating mods directory: {directory}");
                Directory.CreateDirectory(directory);
                return;
            }

            foreach (var dll in Directory.GetFiles(directory, "*.dll"))
            {
                LoadMod(dll);
            }
        }

        private void LoadMod(string dllPath)
        {
            try
            {
                var assembly = Assembly.LoadFrom(dllPath);
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IVdjMod).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        var mod = (IVdjMod)Activator.CreateInstance(type);
                        mod.Initialize();
                        _loadedMods.Add(mod);
                        Console.WriteLine($"Loaded mod: {mod.GetModName()} v{mod.GetModVersion()}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load mod from {dllPath}: {ex.Message}");
            }
        }

        private void UnloadMod(string dllPath)
        {
            var modName = Path.GetFileNameWithoutExtension(dllPath);
            _loadedMods.RemoveAll(mod => mod.GetModName() == modName);
            Console.WriteLine($"Unloaded mod: {modName}");
        }

        public void ExecuteCommand(string command)
        {
            foreach (var mod in _loadedMods)
            {
                try
                {
                    mod.ExecuteCommand(command);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing command on mod {mod.GetModName()}: {ex.Message}");
                }
            }
        }

        public void NotifyDeckLoaded(int deckNumber)
        {
            foreach (var mod in _loadedMods)
            {
                mod.OnDeckLoaded(deckNumber);
            }
        }

        public void NotifyPlayStateChanged(int deckNumber, bool isPlaying)
        {
            foreach (var mod in _loadedMods)
            {
                mod.OnPlayStateChanged(deckNumber, isPlaying);
            }
        }

        public void NotifyVolumeChanged(int deckNumber, float volume)
        {
            foreach (var mod in _loadedMods)
            {
                mod.OnVolumeChanged(deckNumber, volume);
            }
        }
    }
}