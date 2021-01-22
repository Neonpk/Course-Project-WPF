using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace CourseProjectWPF.ValidationRules
{
    class DescriptionRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = value as string;

            if ( Regex.IsMatch( text, @"^[A-Za-z\s\.\-]+$") 
                ||
                Regex.IsMatch( text, @"^[А-Яа-я\s\.\-]+$" )
               )
            {
                return ValidationResult.ValidResult;
            }

            return new ValidationResult(false, "Некорректный ввод. \n Допускаются только [буквы, пробел, тире и точка].");

        }
    }
}
