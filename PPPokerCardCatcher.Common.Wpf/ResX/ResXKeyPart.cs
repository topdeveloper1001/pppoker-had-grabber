namespace PPPokerCardCatcher.Common.Wpf.ResX
{  
    public class ResXKeyPart : ResXParamBase
    {
        public string Key { get; set; }
        public ResXKeyPart()
            : this(null)
        {
        }

        public ResXKeyPart(string path)
            : base(path)
        {
        }
    }
}