﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

namespace Xamarin.Controls.TabView
{
    public class TabViewItem : ContentView
    {
        internal TabViewHeaderItem TabViewHeaderItem { get; set; } = new TabViewHeaderItem();

        public TabViewItem()
        {
            TabViewHeaderItem.PropertyChanged += TabViewHeaderItem_PropertyChanged;
            IsVisible = false;
        }

        private void TabViewHeaderItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TabViewHeaderItem.IsSelectedProperty.PropertyName)
            {
                this.IsSelected = TabViewHeaderItem.IsSelected;
                this.IsVisible = this.IsSelected;
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsSelectedProperty.PropertyName)
            {
                TabViewHeaderItem.IsSelected = IsSelected;
            }
            else if (propertyName == HeaderProperty.PropertyName)
            {
                TabViewHeaderItem.HeaderContent = Header;
            }
            else if (propertyName == IsEnabledProperty.PropertyName)
            {
                TabViewHeaderItem.IsVisible = this.IsEnabled;
            }
        }

        #region IsSelected
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            internal set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly BindableProperty IsSelectedProperty =
            BindableProperty.Create(
                nameof(IsSelected),
                typeof(bool),
                typeof(TabViewItem)
                );
        #endregion

        #region Header
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly BindableProperty HeaderProperty =
            BindableProperty.Create(
                nameof(Header),
                typeof(object),
                typeof(TabViewItem)
                );
        #endregion
    }
}
