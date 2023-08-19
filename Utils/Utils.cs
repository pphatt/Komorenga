using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using Komorenga.Models;
using Microsoft.UI.Xaml.Data;
using Windows.Graphics.DirectX.Direct3D11;

namespace Komorenga.Utils
{
    class GetStatusIcon : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string status)
            {
                switch (status)
                {
                    case "ongoing":
                        return "\uEE56";
                    case "completed":
                        return "\uE73E";
                    case "hiatus":
                        return "\uF8AE";
                    case "cancelled":
                        return "\uE8BB";
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    class ParserChapterTitle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is MangaChapterVolumeAttributes attributes)
            {
                string title = "";

                if (attributes.volume != null)
                {
                    title += $"Vol. {attributes.volume} ";
                }

                if (attributes.chapter != null)
                {
                    title += $"Ch. {attributes.chapter} ";
                }

                if (attributes.title != null)
                {
                    title += $"- {attributes.title}";
                }

                return title;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    class ParserPublishAtDate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string date)
            {
                return DateTime.Parse(date).ToString("dd/MM/yyyy");
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    class ParserTranslationGroup : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is MangaChapterVolumeRelationshipAttributes attributes)
            {
                if (attributes.name != null)
                {
                    return attributes.name;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
