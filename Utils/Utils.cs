﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Komorenga.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Windows.Foundation.Metadata;

namespace Komorenga.Utils;

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
        var en = currentAttributeAltTitle.Find(e => e.en != null);

        if (en != null)
        {
            return en.en;
        }

        var ja = currentAttributeAltTitle.Find(e => e.ja != null);

        if (ja != null)
        {
            return ja.ja;
        }

        var jaRo = currentAttributeAltTitle.Find(e => e.jaRo != null);
        
        if (jaRo != null)
        {
            return jaRo.jaRo;
        }

        return "Something went wrong... Try again";
    }
}

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
        if (value is List<MangaChapterVolumeRelationship> relationships)
        {
            for (int i = 0; i < relationships.Count; i++)
            {
                if (relationships[i].type == "scanlation_group")
                {
                    return relationships[i].attributes.name;
                }

                if (relationships[i].type == "user")
                {
                    return $"Uploaded by {relationships[i].attributes.username}";
                }
            }
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

class ParserChapterCount : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int chapters)
        {
            return $"{chapters} Chapters";
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool isVisible && isVisible)
        {
            return Visibility.Visible;
        }
        else
        {
            return Visibility.Collapsed;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

public class InverseBooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool isVisible && !isVisible)
        {
            return Visibility.Visible;
        }
        else
        {
            return Visibility.Collapsed;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

public class NoResultWasFound : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int count && count == 0)
        {
            return Visibility.Visible;
        }
        else
        {
            return Visibility.Collapsed;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}