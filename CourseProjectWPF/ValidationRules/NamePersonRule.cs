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
    class NamePersonRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string name = value as string;

            if( 
                Regex.IsMatch(name, @"^(?:[А-Я][а-я]{2,50}\s){2}[А-Я][а-я]{2,50}$") 
                ||
                Regex.IsMatch(name, @"^(?:[A-Z][a-z]{2,50}\s){2}[A-Z][a-z]{2,50}$") 
              )
            {
                return ValidationResult.ValidResult;
            }

            return new ValidationResult(false, "Пример\nПетров Василий Иванович\nили\nPetrov Vasiliy Ivanovich");

        }
    }
}
