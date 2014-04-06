using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace extra_extra
{
    public static class TimeInterval
    {
        public static string DisplayValue { get; set; }
        public static TimeSpan TimeValue { get; set; }

        public static List<ComboBoxItem> GetIntervals()
        {
            var comboboxItems = new List<ComboBoxItem>
                {
                    new ComboBoxItem
                        {
                            Content = "Off",
                            Tag = new TimeSpan(0, 0, 0)
                        },
                    new ComboBoxItem
                        {
                            Content = "30 Seconds",
                            Tag = new TimeSpan(0, 0, 30)
                        },
                    new ComboBoxItem
                        {
                            Content = "1 Minute",
                            Tag = new TimeSpan(0, 1, 0)
                        },
                    new ComboBoxItem
                        {
                            Content = "5 Minutes",
                            Tag = new TimeSpan(0, 5, 0)
                        },
                    new ComboBoxItem
                        {
                            Content = "15 Minutes",
                            Tag = new TimeSpan(0, 15, 0)
                        },
                    new ComboBoxItem
                        {
                            Content = "30 Minutes",
                            Tag = new TimeSpan(0, 30, 0)
                        },
                    new ComboBoxItem
                        {
                            Content = "1 Hour",
                            Tag = new TimeSpan(1, 0, 0)
                        }
                };
            return comboboxItems;
        }
    }
}
