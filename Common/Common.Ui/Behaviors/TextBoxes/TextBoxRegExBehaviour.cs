/*using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using Common.Extensions;

namespace Common.Ui.Behaviors.TextBoxs
{
    public class TextBoxRegExBehaviour : Behavior<TextBox>
    {
        private static readonly Regex IntegerRegex = new Regex("^([-]{1})?[0-9]+$");
        private static readonly Regex UnsignedIntegerRegex = new Regex("^[0-9]+$");
        private static readonly Regex DoubleRegex = new Regex("^-?[0-9]*(?:\\.[0-9]*)?$");

        #region DependencyProperties

        public static readonly DependencyProperty RegularExpressionProperty = DependencyProperty.Register(nameof(RegularExpression), typeof(string),
            typeof(TextBoxRegExBehaviour), new FrameworkPropertyMetadata(".*"));

        /// <summary>
        /// Регулярное выражение
        /// </summary>
        public string RegularExpression
        {
            get => (string)GetValue(RegularExpressionProperty);
            set => SetValue(RegularExpressionProperty, value);
        }

        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register(nameof(MaxLength), typeof(int),
            typeof(TextBoxRegExBehaviour), new FrameworkPropertyMetadata(int.MinValue));

        /// <summary>
        /// Максимальная длина
        /// </summary>
        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        public static readonly DependencyProperty EmptyValueProperty =
            DependencyProperty.Register(nameof(EmptyValue), typeof(string), typeof(TextBoxRegExBehaviour), null);

        public string EmptyValue
        {
            get => (string)GetValue(EmptyValueProperty);
            set => SetValue(EmptyValueProperty, value);
        }

        public static readonly DependencyProperty RootClassProperty = DependencyProperty.Register(nameof(RootClass), typeof(object),
            typeof(TextBoxRegExBehaviour), new PropertyMetadata(default(object)));

        /// <summary>
        /// Главный класс. Примечание: Если PropertyName не задан или элемент не найдется, проверяется по атрибутам класса
        /// </summary>
        public object RootClass
        {
            get => (object)GetValue(RootClassProperty);
            set => SetValue(RootClassProperty, value);
        }

        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(nameof(PropertyName), typeof(string),
            typeof(TextBoxRegExBehaviour), null);

        /// <summary>
        /// Имя элемента в классе.
        /// </summary>
        public string PropertyName
        {
            get => (string)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        #region Bools DependencyProperties

        public static readonly DependencyProperty IsIntegerProperty = DependencyProperty.Register(nameof(IsInteger), typeof(bool),
            typeof(TextBoxRegExBehaviour), new PropertyMetadata(default(bool)));

        /// <summary>
        /// Флаг для проверки текста по выражению Integer. Примечание: При активации переменная RegularExpression не учитывается!
        /// </summary>
        public bool IsInteger
        {
            get => (bool)GetValue(IsIntegerProperty);
            set => SetValue(IsIntegerProperty, value);
        }

        public static readonly DependencyProperty IsUnsignedIntegerProperty = DependencyProperty.Register(nameof(IsUnsignedInteger), typeof(bool),
            typeof(TextBoxRegExBehaviour), new PropertyMetadata(default(bool)));

        /// <summary>
        /// Флаг для проверки текста по выражению UnsignedInteger. Примечание: При активации переменная RegularExpression не учитывается!
        /// </summary>
        public bool IsUnsignedInteger
        {
            get => (bool)GetValue(IsUnsignedIntegerProperty);
            set => SetValue(IsUnsignedIntegerProperty, value);
        }

        public static readonly DependencyProperty IsDoubleProperty = DependencyProperty.Register(nameof(IsDouble), typeof(bool),
            typeof(TextBoxRegExBehaviour), new PropertyMetadata(default(bool)));

        /// <summary>
        /// Флаг для проверки текста по выражению Double. Примечание: При активации переменная RegularExpression не учитывается!
        /// </summary>
        public bool IsDouble
        {
            get => (bool)GetValue(IsDoubleProperty);
            set => SetValue(IsDoubleProperty, value);
        }

        #endregion

        #endregion

        protected override void OnSetup()
        {
            base.OnSetup();
            AssociatedObject.PreviewTextInput += PreviewTextInputHandler;
            AssociatedObject.PreviewKeyDown += PreviewKeyDownHandler;
            DataObject.AddPastingHandler(AssociatedObject, PastingHandler);
        }

        protected override void OnCleanup()
        {
            AssociatedObject.PreviewTextInput -= PreviewTextInputHandler;
            AssociatedObject.PreviewKeyDown -= PreviewKeyDownHandler;
            DataObject.RemovePastingHandler(AssociatedObject, PastingHandler);
            base.OnCleanup();
        }

        #region Обработчики событий [PRIVATE] --------------------------------------

        private void PreviewTextInputHandler(object sender, TextCompositionEventArgs e)
        {
            if (AssociatedObject == null)
            {
                return;
            }

            string text;
            if (AssociatedObject.Text.Length < AssociatedObject.CaretIndex)
                text = AssociatedObject.Text;
            else
            {
                // Оставшийся текст после удаления выделенного текста.
                text = TreatSelectedText(out var remainingTextAfterRemoveSelection)
                    ? remainingTextAfterRemoveSelection.Insert(AssociatedObject.SelectionStart, e.Text)
                    : AssociatedObject.Text.Insert(AssociatedObject.CaretIndex, e.Text);
            }

            e.Handled = !ValidateText(text);
        }

        /// <summary>
        /// Обработчик события PreviewKeyDown (при нажатии клавиши)
        /// </summary>
        private void PreviewKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(EmptyValue))
                return;

            string text = null;

            // Handle the Backspace key
            if (e.Key == Key.Back)
            {
                if (!TreatSelectedText(out text))
                {
                    if (AssociatedObject.SelectionStart > 0)
                        text = AssociatedObject.Text.Remove(AssociatedObject.SelectionStart - 1, 1);
                }
            }
            // Handle the Delete key
            else if (e.Key == Key.Delete)
            {
                // If text was selected, delete it
                if (!TreatSelectedText(out text) && AssociatedObject.Text.Length > AssociatedObject.SelectionStart)
                {
                    // Otherwise delete next symbol
                    text = AssociatedObject.Text.Remove(AssociatedObject.SelectionStart, 1);
                }
            }

            if (text == string.Empty)
            {
                AssociatedObject.Text = EmptyValue;
                if (e.Key == Key.Back)
                    AssociatedObject.SelectionStart++;
                e.Handled = true;
            }
        }

        private void PastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                var text = Convert.ToString(e.DataObject.GetData(DataFormats.Text));

                if (!ValidateText(text))
                    e.CancelCommand();
            }
            else
                e.CancelCommand();
        }

        #endregion Event handlers [PRIVATE] -----------------------------------

        #region Вспомогательные методы [PRIVATE] -----------------------------------

        /// <summary>
        /// Проверить определенный текст с помощью нашего регулярного выражения и условий длины текста
        /// </summary>
        /// <param name="text"> Text for validation </param>
        /// <returns> True - действительный, False - не действительный </returns>
        private bool ValidateText(string text)
        {
            if (RootClass != null)
            {
                ApplyAttributes(RootClass, PropertyName);
            }

            // если выражение по умолчанию
            if (RegularExpression.Equals(".*"))
            {
                if (IsInteger || IsUnsignedInteger || IsDouble)
                {
                    if (IsInteger)
                    {
                        RegularExpression = IntegerRegex.ToString();
                    }
                    else if (IsUnsignedInteger)
                    {
                        RegularExpression = UnsignedIntegerRegex.ToString();
                    }
                    else if (IsDouble)
                    {
                        RegularExpression = DoubleRegex.ToString();
                    }
                }
            }

            // проверяем, вдруг MaxLength совсем не задан
            if (MaxLength < 0)
            {
                MaxLength = int.MaxValue;
            }

            return new Regex(RegularExpression, RegexOptions.IgnoreCase).IsMatch(text) && (MaxLength == 0 || text.Length <= MaxLength);
        }

        /// <summary>
        /// Обработка выделения текста
        /// </summary>
        /// <returns>true, если символы был успешно удален; в противном случае ложно.</returns>
        private bool TreatSelectedText(out string text)
        {
            text = null;
            if (AssociatedObject.SelectionLength <= 0)
                return false;

            var length = AssociatedObject.Text.Length;
            if (AssociatedObject.SelectionStart >= length)
                return true;

            if (AssociatedObject.SelectionStart + AssociatedObject.SelectionLength >= length)
                AssociatedObject.SelectionLength = length - AssociatedObject.SelectionStart;

            text = AssociatedObject.Text.Remove(AssociatedObject.SelectionStart, AssociatedObject.SelectionLength);
            return true;
        }

        /// <summary>
        /// Применить параметры из элемента
        /// </summary>
        /// <param name="rootClass"></param>
        /// <param name="propertyName"></param>
        private void ApplyAttributes(object rootClass, string propertyName)
        {
            object[] attrs;

            // получаем тип класса
            var rooType = rootClass.GetType();

            // если имя элемента задано, получаем элемент из класса
            if (!propertyName.IsNullOrEmpty())
            {
                var property = rooType.GetProperty(propertyName);

                // если находим элемент в классе, получаем его атрибуты
                if (property != null)
                {
                    attrs = property.GetCustomAttributes(false);

                    // перебираем атрибуты
                    foreach (var attr in attrs)
                    {
                        if (attr is MaxLengthAttribute maxLengthAttribute)
                        {
                            MaxLength = maxLengthAttribute.MaxLength;
                        }

                        if (attr is PatternsAttribute patternsAttribute)
                        {
                            RegularExpression = patternsAttribute.Patterns.FirstOrDefault()?.ToString();
                        }
                    }
                }
            }
            else
            {
                // получаем атрибуты класса
                attrs = rooType.GetCustomAttributes(true);

                // перебираем атрибуты
                foreach (var attr in attrs)
                {
                    if (attr is NameMaxLengthAttribute maxLengthAttribute)
                    {
                        MaxLength = maxLengthAttribute.NameMaxLength;
                    }

                    if (attr is NamePatternsAttribute patternsAttribute)
                    {
                        RegularExpression = patternsAttribute.Patterns.FirstOrDefault()?.ToString();
                    }
                }
            }
        }

        #endregion Auxiliary methods [PRIVATE] --------------------------------
    }
}*/