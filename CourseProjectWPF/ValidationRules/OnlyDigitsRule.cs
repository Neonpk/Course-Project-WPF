using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CourseProjectWPF.ValidationRules
{
    class OnlyDigitsRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = value as string;

            if (!Regex.IsMatch(text, @"^\d+$"))
                return new ValidationResult(false, "Данное поле должно содержать только цифры.");

            return ValidationResult.ValidResult;

        }
    }
}
