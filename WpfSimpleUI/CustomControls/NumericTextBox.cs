using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfSimpleUI.CustomControls
{
    /// <summary>
    /// 输入数字的文本框
    /// </summary>
    /// <remarks>.Net 4.0 版本的TextBox中，Text属性的CoerceValueCallBack存在bug，无法用来监控用户输入</remarks>
    public class NumericTextBox : TextBox
    {
        public NumericTextBox()
        {
            PreviewTextInput += NumericTextBox_PreviewTextInput;
            KeyUp += NumericTextBox_KeyUp;
            //GotFocus += NumericTextBox_GotFocus;

            //鼠标点击时，获取焦点
            AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(SelectivelyIgnoreMouseButton), true);

            //键盘焦点
            AddHandler(GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText), true);

            //鼠标双击的焦点
            AddHandler(MouseDoubleClickEvent, new RoutedEventHandler(SelectAllText), true);

            //屏蔽输入法
            InputMethod.SetIsInputMethodEnabled(this, false);
        }

        //鼠标点击时，获取焦点
        private static void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            // Find the TextBox
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent != null)
            {
                var textBox = (TextBox)parent;
                if (!textBox.IsKeyboardFocusWithin)
                {
                    // If the text box is not yet focussed, give it the focus and
                    // stop further processing of this click event.
                    textBox.Focus();
                    e.Handled = true;
                }
            }
        }

        private static void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
            {
                textBox.SelectAll();
            }
        }

        private void NumericTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SelectAll();
        }

        //处理用户输入
        private void NumericTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            HandleInput(e.Text);
            e.Handled = true;
        }

        private void HandleInput(string text)
        {
            //此次用户输入生效之后，所得到的字符串结果
            string textNew = Text.Remove(SelectionStart, SelectionLength).Insert(SelectionStart, text);
            HandleText(textNew);
            _IsInputHandled = true;
        }

        private void NumericTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (_IsInputHandled)
            {
                _IsInputHandled = false;
                return;
            }

            HandleText(Text);
        }

        volatile bool _IsInputHandled = false;

        public static double MaxDefault = 1000d;
        public static double MinDefault = 0d;

        /// <summary>
        /// 值
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value), typeof(double), typeof(NumericTextBox),
            new FrameworkPropertyMetadata(MinDefault, null, CoerceValue) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged });

        /// <summary>
        /// 验证输入
        /// </summary>
        /// <returns></returns>
        private static object CoerceValue(DependencyObject d, object baseValue)
        {
            NumericTextBox host = (NumericTextBox)d;

            double res;
            if (baseValue is string s)
            {
                if (double.TryParse(s, out double value))
                {
                    res = host.CheckValue(value);
                }
                else
                {
                    res = host.Value;
                }
            }
            else if (baseValue is double value)
            {
                res = host.CheckValue(value);
            }
            else
            {
                res = host.Value;
            }

            if (res.ToString() != host.Text)
            {
                host.Text = res.ToString();

                //通过value刷新Text后，光标置于最后
                host.SelectionStart = host.Text.Length;
            }

            return res;
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public double Max
        {
            get { return (double)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Max.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register(nameof(Max), typeof(double), typeof(NumericTextBox), new PropertyMetadata(MaxDefault, null, MaxCoerce));

        private static object MaxCoerce(DependencyObject d, object baseValue)
        {
            NumericTextBox host = (NumericTextBox)d;
            return (double)baseValue < host.Min ? host.Min : baseValue;
        }

        /// <summary>
        /// 最小值
        /// </summary>
        public double Min
        {
            get { return (double)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Min.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register(nameof(Min), typeof(double), typeof(NumericTextBox), new PropertyMetadata(MinDefault, null, MinCoerce));

        private static object MinCoerce(DependencyObject d, object baseValue)
        {
            NumericTextBox host = (NumericTextBox)d;
            return (double)baseValue > host.Max ? host.Max : baseValue;
        }

        /// <summary>
        /// 校验输入的值，返回一个有效值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private double CheckValue(double value)
        {
            if (value > Max)
            {
                return Max;
            }

            if (value < Min)
            {
                return Min;
            }

            return value;
        }

        /// <summary>
        /// 判断输入是否合法
        /// </summary>
        private void HandleText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                Value = Min;
            }

            if (double.TryParse(text, out double num))
            {
                Value = num;
            }
            else
            {
                Text = Value.ToString();
                SelectionStart = Text.Length;
            }
        }
    }
}
