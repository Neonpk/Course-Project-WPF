using CourseProjectWPF.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CourseProjectWPF.ValidationRules
{
    class ComboboxSelectedItemRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            if (value == null)
            {
                return new ValidationResult(false, "Выбранный элемент не должен быть пустым.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
