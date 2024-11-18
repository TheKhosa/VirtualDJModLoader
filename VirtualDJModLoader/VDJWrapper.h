#pragma once
// VDJWrapper.h
#pragma once
#include <windows.h>
#include <comdef.h>
#include <string>
#include <memory>
#include <vector>
#include "vdjPlugin8.h"

// Import the .NET assembly
#import "VirtualDJPluginLoader.tlb" raw_interfaces_only

// Forward declarations
namespace VirtualDJPluginLoader {
    interface IModLoader;
}

class VDJWrapper : public IVdjPlugin8 {
private:
    ULONG m_RefCount;
    IUnknown* m_VdjApp8;
    VirtualDJPluginLoader::IModLoader* m_ModLoader;
    bool m_Initialized;

    // COM management
    void InitializeCOM();
    void UninitializeCOM();
    HRESULT CreateModLoader();

public:
    VDJWrapper();
    virtual ~VDJWrapper();

    // IUnknown methods
    HRESULT VDJ_API QueryInterface(REFIID riid, void** ppvObject) override;
    ULONG VDJ_API AddRef() override;
    ULONG VDJ_API Release() override;

    // IVdjPlugin8 methods
    HRESULT VDJ_API OnLoad() override;
    HRESULT VDJ_API OnGetPluginInfo(TVdjPluginInfo8* infos) override;
    HRESULT VDJ_API OnStart() override;
    HRESULT VDJ_API OnStop() override;
    HRESULT VDJ_API OnDeviceChange() override;
    HRESULT VDJ_API OnGetUserInterface(TVdjPluginInterface8* pluginInterface) override;
    HRESULT VDJ_API OnParameter(int id) override;
    HRESULT VDJ_API OnGetParameterString(int id, char* outParam, int outParamLen) override;
    HRESULT VDJ_API OnDatabaseCallback(int type, const char* str, void* data) override;
    HRESULT VDJ_API OnGetSongBuffer(int pos, void* buffer, int nb) override;
    HRESULT VDJ_API OnGetVisualData(float* visData, int nb) override;

    // Custom methods for mod interaction
    HRESULT NotifyDeckLoaded(int deckNumber);
    HRESULT NotifyPlayStateChanged(int deckNumber, bool isPlaying);
    HRESULT NotifyVolumeChanged(int deckNumber, float volume);
};

// Factory function
extern "C" __declspec(dllexport) HRESULT VDJ_API DllGetClassObject(const GUID& rclsid, const GUID& riid, void** ppObject);
