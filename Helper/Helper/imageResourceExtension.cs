using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helper
{
    [ContentProperty (nameof(Source))]
    class imageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if(Source == null)
            {
                return null;
            }

            var image = ImageSource.FromResource(Source,typeof(imageResourceExtension).GetTypeInfo().Assembly);

            return image;
        }
    }
}
