using PPPokerHandGrabber.Common.Resources;

namespace PPPokerHandGrabber.Common.Wpf.ResX
{  
    public class EnumResXKeyProviderExtension : ResXKeyProviderExtension<CompositeKeyProvider>
    {
        public EnumResXKeyProviderExtension()
        {
            ResXKeyProvider.BaseKey = "Enum";
        }
    }
}