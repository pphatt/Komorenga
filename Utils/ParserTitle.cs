using System;
using System.Collections.Generic;
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

    class ParserAltTitles : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is List<Language> currentAttributeAltTitle)
            {
                string title = GetAltTitleByLanguage(currentAttributeAltTitle);
                return title;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        private string GetAltTitleByLanguage(List<Language> currentAttributeAltTitle)
        {
            var altTitles = "";

            for (int i = 0; i < currentAttributeAltTitle.Count; i++)
            {
                if (currentAttributeAltTitle[i].jaRo != null)
                {
                    if (altTitles.Length < currentAttributeAltTitle[i].jaRo.Length)
                    {
                        altTitles = currentAttributeAltTitle[i].jaRo;
                    }
                }

                if (currentAttributeAltTitle[i].ja != null)
                {
                    if (altTitles.Length < currentAttributeAltTitle[i].ja.Length)
                    {
                        altTitles = currentAttributeAltTitle[i].ja;
                    }
                }

                if (currentAttributeAltTitle[i].en != null)
                {
                    if (altTitles.Length < currentAttributeAltTitle[i].en.Length)
                    {
                        altTitles = currentAttributeAltTitle[i].en;
                    }
                }
            }

            return altTitles != null ? altTitles : "Something went wrong... Try again";
        }
    }
}
