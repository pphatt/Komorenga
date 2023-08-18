using System;
using Microsoft.UI.Xaml.Data;

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
}
