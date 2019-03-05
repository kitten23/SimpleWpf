using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfSimpleUI.CustomControls
{
    public class CustomDateTimePicker : DatePicker
    {
        static CustomDateTimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomDateTimePicker), new FrameworkPropertyMetadata(typeof(CustomDateTimePicker)));

            BorderBrushProperty.OverrideMetadata(typeof(CustomDateTimePicker), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.DeepSkyBlue)));
            BorderThicknessProperty.OverrideMetadata(typeof(CustomDateTimePicker), new FrameworkPropertyMetadata(new Thickness(1)));

            SelectedDateProperty.OverrideMetadata(typeof(CustomDateTimePicker), new FrameworkPropertyMetadata(DateTime.Now.Date, SelectedDateChangedCb));
        }

        private static void SelectedDateTimeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomDateTimePicker host = d as CustomDateTimePicker;

            if (e.NewValue is DateTime dt)
            {
                host.SelectedDate = dt.Date;
                host.SelectedTime = dt.TimeOfDay;
            }
        }

        //日期值变化
        private static void SelectedDateChangedCb(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomDateTimePicker host = (CustomDateTimePicker)d;
            DateTime dt = (DateTime)e.NewValue;

            host._IsUpdatingDate = true;
            host.Year = dt.Year;
            host.Month = dt.Month;
            host.Day = dt.Day;
            host._IsUpdatingDate = false;

            host.UpdateSelectedDateTime();
        }

        //时间值变化
        private static void SelectedTimeChangedCb(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomDateTimePicker host = (CustomDateTimePicker)d;
            TimeSpan ts = (TimeSpan)e.NewValue;

            host._IsUpdatingTime = true;
            host.Hour = ts.Hours;
            host.Minute = ts.Minutes;
            host.Second = ts.Seconds;
            host._IsUpdatingTime = false;

            host.UpdateSelectedDateTime();
        }

        //设置SelectedDateTime属性
        private void UpdateSelectedDateTime()
        {
            SelectedDateTime = new DateTime(SelectedDate.Value.Ticks + SelectedTime.Ticks);
        }

        //Update Date when date param updated
        /// <summary>
        /// 控件的单个日期参数被修改时触发（年、月、日）
        /// </summary>
        private static void DateParamChangedCb(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomDateTimePicker host = d as CustomDateTimePicker;

            if (host._IsUpdatingDate)
            {
                return;
            }

            var dayInMonth = DateTime.DaysInMonth(host.Year, host.Month);

            //设置控件的日期属性
            host.SelectedDate = new DateTime(host.Year, host.Month, Math.Min(host.Day, dayInMonth));
        }

        /// <summary>
        /// 控件的单个时间参数被修改时触发（时、分、秒）
        /// </summary>
        private static void TimeParamChangedCb(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomDateTimePicker host = d as CustomDateTimePicker;

            if (host._IsUpdatingTime)
            {
                return;
            }

            //设置控件的时间属性
            host.SelectedTime = new TimeSpan(host.Hour, host.Minute, host.Second);
        }

        /// <summary>
        /// 标记正在处理<see cref="SelectedDate"/>属性的变化回调，防止<see cref="Year"/>、<see cref="Month"/>、<see cref="Day"/>变化时，重复设置<see cref="SelectedDate"/>
        /// </summary>
        volatile bool _IsUpdatingDate = false;

        /// <summary>
        /// 标记正在处理<see cref="SelectedTime"/>属性的变化回调，防止<see cref="Hour"/>、<see cref="Minute"/>、<see cref="Second"/>变化时，重复设置<see cref="SelectedTime"/>
        /// </summary>
        volatile bool _IsUpdatingTime = false;


        public Brush ButtonDefaultBrush { get { return (Brush)GetValue(ContentDefaultBrushProperty); } set { SetValue(ContentDefaultBrushProperty, value); } }
        public static readonly DependencyProperty ContentDefaultBrushProperty = DependencyProperty.Register(nameof(ButtonDefaultBrush), typeof(Brush), typeof(CustomDateTimePicker),
            new FrameworkPropertyMetadata(new SolidColorBrush(Colors.DeepSkyBlue)));

        /// <summary>
        /// Button color when mouse over
        /// </summary>
        public Brush ButtonMouseOverBrush
        {
            get { return (Brush)GetValue(ButtonMouseOverBrushProperty); }
            set { SetValue(ButtonMouseOverBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonMouseOverBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonMouseOverBrushProperty = DependencyProperty.Register(
            nameof(ButtonMouseOverBrush), typeof(Brush), typeof(CustomDateTimePicker), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.DodgerBlue)));

        public DateTime? SelectedDateTime
        {
            get { return (DateTime?)GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Date.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedDateTimeProperty =
            DependencyProperty.Register(nameof(SelectedDateTime), typeof(DateTime?), typeof(CustomDateTimePicker), new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectedDateTimeChangedCallback));

        /// <summary>
        /// Time
        /// </summary>
        public TimeSpan SelectedTime
        {
            get { return (TimeSpan)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }
        public static readonly DependencyProperty SelectedTimeProperty = DependencyProperty.Register(
            nameof(SelectedTime), typeof(TimeSpan), typeof(CustomDateTimePicker), new FrameworkPropertyMetadata(DateTime.Now.TimeOfDay, SelectedTimeChangedCb));

        public int Year
        {
            get { return (int)GetValue(YearProperty); }
            set { SetValue(YearProperty, value); }
        }
        public static readonly DependencyProperty YearProperty = DependencyProperty.Register(
            nameof(Year), typeof(int), typeof(CustomDateTimePicker), new FrameworkPropertyMetadata(DateTime.Now.Year, DateParamChangedCb));

        public int Month
        {
            get { return (int)GetValue(MonthProperty); }
            set { SetValue(MonthProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Month.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MonthProperty = DependencyProperty.Register(
            nameof(Month), typeof(int), typeof(CustomDateTimePicker), new FrameworkPropertyMetadata(DateTime.Now.Month, DateParamChangedCb));

        public int Day
        {
            get { return (int)GetValue(DayProperty); }
            set { SetValue(DayProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Month.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DayProperty =
            DependencyProperty.Register(nameof(Day), typeof(int), typeof(CustomDateTimePicker), new FrameworkPropertyMetadata(DateTime.Now.Day, DateParamChangedCb));

        public int Hour
        {
            get { return (int)GetValue(HourProperty); }
            set { SetValue(HourProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Month.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HourProperty =
            DependencyProperty.Register(nameof(Hour), typeof(int), typeof(CustomDateTimePicker), new FrameworkPropertyMetadata(0, TimeParamChangedCb));

        public int Minute
        {
            get { return (int)GetValue(MinuteProperty); }
            set { SetValue(MinuteProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Month.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinuteProperty =
            DependencyProperty.Register(nameof(Minute), typeof(int), typeof(CustomDateTimePicker), new FrameworkPropertyMetadata(0, TimeParamChangedCb));

        public int Second
        {
            get { return (int)GetValue(SecondProperty); }
            set { SetValue(SecondProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Month.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondProperty =
            DependencyProperty.Register(nameof(Second), typeof(int), typeof(CustomDateTimePicker), new FrameworkPropertyMetadata(0, TimeParamChangedCb));


    }
}
