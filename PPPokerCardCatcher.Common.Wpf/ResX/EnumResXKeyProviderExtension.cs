using PPPokerCardCatcher.Common.Resources;

namespace PPPokerCardCatcher.Common.Wpf.ResX
{  
    public class EnumResXKeyProviderExtension : ResXKeyProviderExtension<CompositeKeyProvider>
    {
        public EnumResXKeyProviderExtension()
        {
            ResXKeyProvider.BaseKey = "Enum";
        }
    }
}