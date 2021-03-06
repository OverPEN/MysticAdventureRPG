﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace CommonClasses.Converters
{
    public class LevelToNecessaryXPConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int Level = int.Parse(value.ToString());

            return (Level * (100 + ((Level - 1) * 50))).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
