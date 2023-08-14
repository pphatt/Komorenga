using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using Komorenga.Models;

namespace Komorenga.Utils
{
    class ParserTitle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Language currentAttributeTitle)
            {
                string title = GetTitleByLanguage(currentAttributeTitle);
                return title;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        private string GetTitleByLanguage(Language currentAttributeTitle)
        {
            if (currentAttributeTitle.en != null)
            {
                return currentAttributeTitle.en;
            }

            if (currentAttributeTitle.jaRo != null)
            {
                System.Diagnostics.Debug.WriteLine("ja-ro: " + currentAttributeTitle.jaRo);
                return currentAttributeTitle.jaRo;
            }

            return "Something went wrong... Try again";
        }
    }
}
