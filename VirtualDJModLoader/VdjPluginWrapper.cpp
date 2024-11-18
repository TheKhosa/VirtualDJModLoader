// C++ Part: VirtualDJ Plugin Wrapper

#include "VdjPlugin8.h"
#include <vcclr.h> // For GCHandle

class VdjPluginWrapper : public IVdjPlugin8 {
public:
    VdjPluginWrapper() {}
    virtual ~VdjPluginWrapper() {}

    // Loading the plugin
    HRESULT VDJ_API OnLoad() {
        // Plugin initialization code here
        return S_OK;
    }

    // Provide information about the plugin
    HRESULT VDJ_API OnGetPluginInfo(TVdjPluginInfo8* info) {
        info->PluginName = "C# Plugin Wrapper";
        info->Author = "Your Name";
        info->Description = "Wrapper for loading C# plugins into VirtualDJ.";
        info->Version = "1.0";
        return S_OK;
    }

    // Handle the plugin unloading
    ULONG VDJ_API Release() {
        delete this;
        return S_OK;
    }

    HRESULT SendCommand(const char* command) {
        // This function sends a command to VirtualDJ
        // Call back into the managed code via a delegate, if needed
        return cb->SendCommand(command);
    }

    HRESULT GetInfo(const char* command, double* result) {
        // Handle getting information from VirtualDJ here
        return cb->GetInfo(command, result);
    }
};

// DLL export function (Entry point of the plugin)
extern "C" VDJ_EXPORT HRESULT VDJ_API DllGetClassObject(const GUID& rclsid, const GUID& riid, void** ppObject) {
    if (rclsid == CLSID_VdjPlugin8) {
        if (riid == IID_IVdjPluginBasic8) {
            *ppObject = new VdjPluginWrapper();
            return S_OK;
        }
    }
    return CLASS_E_CLASSNOTAVAILABLE;
}

// Main entry point for testing the plugin (optional)
#ifdef _DEBUG
#include <iostream>

int main() {
    VdjPluginWrapper plugin;
    TVdjPluginInfo8 info;
    HRESULT hr = plugin.OnGetPluginInfo(&info);
    if (hr == S_OK) {
        std::cout << "Plugin Name: " << info.PluginName << std::endl;
        std::cout << "Author: " << info.Author << std::endl;
        std::cout << "Description: " << info.Description << std::endl;
        std::cout << "Version: " << info.Version << std::endl;
    }
    else {
        std::cout << "Failed to get plugin info." << std::endl;
    }

    return 0;
}
#endif
