using System.Threading.Tasks;
using ScriptableObject;
using Services;
using UnityEngine.AddressableAssets;

public class SharedData
{
    private MainData _mainData;

    public MainData GetMainData => _mainData;

    public async Task Init()
    {
        var handleShip = Addressables.LoadAssetAsync<MainData>(AddressablePath.MAIN_DATA);

        await Task.WhenAll(handleShip.Task);

        _mainData = handleShip.Result;
    }
        
}