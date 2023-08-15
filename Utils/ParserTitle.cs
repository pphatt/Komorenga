using System;
using Komorenga.Models;
using Microsoft.UI.Xaml.Data;

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
                return currentAttributeTitle.jaRo;
            }

            return "Something went wrong... Try again";
        }
    }
}
