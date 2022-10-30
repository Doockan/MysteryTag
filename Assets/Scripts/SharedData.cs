using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace MysteryTag
{
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
}