using System;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using System.Resources;

namespace Terminal
{
    [ContentProperty("Key")]
    public class TranslateExtension : IMarkupExtension
    {
        public string Key { get; set; }
        static ResourceManager ResourceManagerInstance;

        #region IMarkupExtension implementation

        public static void Init(ResourceManager r)
        {
            ResourceManagerInstance = r;
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ResourceManagerInstance == null)
            {
                throw new InvalidOperationException("Call TranslateExtension.Init(ResourceManager) in your App.cs");
            }
            return ResourceManagerInstance.GetString(this.Key);
        }

        #endregion
    }
}