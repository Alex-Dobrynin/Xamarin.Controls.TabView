using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using Xamarin.Forms;

namespace Xamarin.Controls.TabView
{
    internal class TabViewHeaderItem : Grid
    {
        private readonly DataTemplate _stringTemplate = new DataTemplate(() =>
        {
            var lbl = new Label()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };
            lbl.SetBinding(Label.TextProperty, ".");
            lbl.SetBinding(Label.TextColorProperty, new TemplateBinding("HeaderTextColor"));
            lbl.SetBinding(Label.FontFamilyProperty, new TemplateBinding("HeaderFontFamily"));
            lbl.SetBinding(Label.FontSizeProperty, new TemplateBinding("HeaderFontSize"));
            lbl.SetBinding(Label.FontAttributesProperty, new TemplateBinding("HeaderFontAttributes"));
            return new ContentView()
            {
                Content = lbl,
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                Padding = new Thickness(20, 0)
            };
        });

        private View _selectedView;
        private View _unselectedView;

        public TabViewHeaderItem()
        {
            var tap = new TapGestureRecognizer();
            tap.Tapped += Tap_Tapped;
            this.GestureRecognizers.Add(tap);

            this.SetBinding(SelectedContentTemplateProperty, new TemplateBinding("SelectedHeaderTemplate"));
            this.SetBinding(ContentTemplateProperty, new TemplateBinding("HeaderTemplate"));
        }

        private void Tap_Tapped(object sender, System.EventArgs e)
        {
            if (!this.IsSelected) this.IsSelected = true;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == HeaderContentProperty.PropertyName && HeaderContent is View content)
            {
                this.Children.Clear();
                this.Children.Add(content);
                //this.Content = content;
            }

            if (HeaderContent is View) return;

            else if (propertyName == IsSelectedProperty.PropertyName)
            {
                if (IsSelected)
                {
                    this.SetBinding(View.BackgroundColorProperty, new TemplateBinding("SelectedHeaderBackgroundColor"));
                    //this.Content = _selectedView;
                }
                else
                {
                    this.SetBinding(View.BackgroundColorProperty, new TemplateBinding("HeaderBackgroundColor"));
                    //this.Content = _unselectedView;
                }

                _selectedView.IsVisible = this.IsSelected;
                _unselectedView.IsVisible = !this.IsSelected;
            }
            else if (propertyName == HeaderContentProperty.PropertyName
                || propertyName == SelectedContentTemplateProperty.PropertyName
                || propertyName == ContentTemplateProperty.PropertyName)
            {
                InitContent();
            }
        }

        internal void InitContent()
        {
            var unselectedTemplate = ContentTemplate != null
                                     ? (ContentTemplate is DataTemplateSelector selectorU ? selectorU.SelectTemplate(HeaderContent, null) : ContentTemplate)
                                     : _stringTemplate;

            var selectedTemplate = SelectedContentTemplate != null
                                   ? (SelectedContentTemplate is DataTemplateSelector selectorS ? selectorS.SelectTemplate(HeaderContent, null) : SelectedContentTemplate)
                                   : unselectedTemplate;

            object context = HeaderContent;
            if (context is null)
            {
                context = "Empty Header";
            }

            _selectedView = selectedTemplate.CreateContent() as View;
            _selectedView.BindingContext = selectedTemplate == _stringTemplate ? context.ToString() : context;

            _unselectedView = unselectedTemplate.CreateContent() as View;
            _unselectedView.BindingContext = unselectedTemplate == _stringTemplate ? context.ToString() : context;

            _selectedView.IsVisible = this.IsSelected;
            _unselectedView.IsVisible = !this.IsSelected;

            this.Children.Clear();
            this.Children.Add(_selectedView);
            this.Children.Add(_unselectedView);
            //this.Content = this.IsSelected ? _selectedView : _unselectedView;
        }

        #region IsSelected
        internal bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        internal static readonly BindableProperty IsSelectedProperty =
            BindableProperty.Create(
                nameof(IsSelected),
                typeof(bool),
                typeof(TabViewHeaderItem)
                );
        #endregion

        #region HeaderContent
        internal object HeaderContent
        {
            get { return (object)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        internal static readonly BindableProperty HeaderContentProperty =
            BindableProperty.Create(
                nameof(HeaderContent),
                typeof(object),
                typeof(TabViewHeaderItem)
                );
        #endregion

        #region SelectedContentTemplate
        internal DataTemplate SelectedContentTemplate
        {
            get { return (DataTemplate)GetValue(SelectedContentTemplateProperty); }
            set { SetValue(SelectedContentTemplateProperty, value); }
        }

        internal static readonly BindableProperty SelectedContentTemplateProperty =
            BindableProperty.Create(
                nameof(SelectedContentTemplate),
                typeof(DataTemplate),
                typeof(TabViewHeaderItem)
                );
        #endregion

        #region ContentTemplate
        internal DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        internal static readonly BindableProperty ContentTemplateProperty =
            BindableProperty.Create(
                nameof(ContentTemplate),
                typeof(DataTemplate),
                typeof(TabViewHeaderItem)
                );
        #endregion
    }

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

    [ContentProperty("Tabs")]
    public class TabView : ContentView
    {
        private Grid _contentContainer;
        private StackLayout _headersContainer;
        private ScrollView _headersScroll;
        private TabViewItem _prevSelectedTabItem;
        private bool _useItemsSource;

        public TabView()
        {
            Tabs.CollectionChanged += Tabs_CollectionChanged;
        }

        public ObservableCollection<TabViewItem> Tabs { get; } = new ObservableCollection<TabViewItem>();

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _contentContainer = this.GetTemplateChild("PART_ContentContainer") as Grid;
            _headersContainer = this.GetTemplateChild("PART_HeadersContainer") as StackLayout;
            _headersScroll = this.GetTemplateChild("PART_HeadersScrollView") as ScrollView;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (this.BindingContext != null && !_useItemsSource)
            {
                foreach (var tab in Tabs)
                {
                    if (tab.BindingContext != this.BindingContext) tab.BindingContext = this.BindingContext;
                }
            }
        }

        protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanging(propertyName);

            if (propertyName == ItemsSourceProperty.PropertyName)
            {
                if (ItemsSource != null)
                {
                    if (ItemsSource is INotifyCollectionChanged itemsSource) itemsSource.CollectionChanged -= ItemsSource_CollectionChanged;

                    _useItemsSource = false;

                    Tabs.Clear();
                }
            }
            else if (propertyName == SelectedTabIndexProperty.PropertyName)
            {
                if (SelectedTabIndex == -1) _prevSelectedTabItem = null;
                else
                {
                    _prevSelectedTabItem = Tabs.ElementAtOrDefault(SelectedTabIndex);
                }
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == ItemsSourceProperty.PropertyName)
            {
                if (ItemsSource != null)
                {
                    if (ItemsSource is INotifyCollectionChanged itemsSource) itemsSource.CollectionChanged += ItemsSource_CollectionChanged;

                    _useItemsSource = true;

                    InitItems(ItemsSource);
                }
            }
            else if (propertyName == SelectedTabIndexProperty.PropertyName)
            {
                if (SelectedTabIndex == -1)
                {
                    /*if (_contentContainer != null && _contentContainer.Content is TabViewItem tab)
                    {
                        tab.IsSelected = false;
                        _contentContainer.Content = null;
                    }*/
                    if (_prevSelectedTabItem != null) _prevSelectedTabItem.IsSelected = false;
                }
                else if (_contentContainer != null)
                {
                    if (_prevSelectedTabItem != null) _prevSelectedTabItem.IsSelected = false;
                    /*if (_contentContainer.Content is TabViewItem prevTab)
                    {
                        prevTab.IsSelected = false;
                    }*/

                    var newTab = Tabs.ElementAtOrDefault(SelectedTabIndex);
                    if (newTab != null)
                    {
                        newTab.IsSelected = true;
                    }
                    //_contentContainer.Content = newTab;
                }

                SelectedTabChangedCommand?.Execute(SelectedTabChangedCommandParameter);
            }
            else if (propertyName == ContentTemplateProperty.PropertyName && _useItemsSource)
            {
                foreach (var tab in Tabs)
                {
                    var index = Tabs.IndexOf(tab);
                    InitContentTemplate(tab.BindingContext, tab);
                }
            }
        }

        private void InitItems(IEnumerable source, bool useIndex = false, int index = 0)
        {
            if (source == null) return;

            foreach (var item in source)
            {
                var tabItem = new TabViewItem();
                InitContentTemplate(item, tabItem);

                tabItem.BindingContext = item;
                tabItem.Header = item;

                if (useIndex)
                {
                    Tabs.Insert(index, tabItem);
                }
                else Tabs.Add(tabItem);
            }
        }

        private void InitContentTemplate(object item, TabViewItem tabItem)
        {
            if (ContentTemplate != null)
            {
                var template = ContentTemplate;
                if (ContentTemplate is DataTemplateSelector selector)
                {
                    template = selector.SelectTemplate(item, this);
                }

                var tabContent = template.CreateContent() as View;
                tabContent.BindingContext = item;

                tabItem.Content = tabContent;
            }
            else
            {
                var tabContent = new Label();
                tabContent.Text = item.ToString();

                tabItem.Content = tabContent;
            }
        }

        private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        InitItems(e.NewItems, true, e.NewStartingIndex);
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    {
                        Tabs.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    {
                        if (Tabs.Count == 1)
                        {
                            Tabs.Clear();
                            return;
                        }

                        if (e.OldStartingIndex < SelectedTabIndex)
                        {
                            Tabs.RemoveAt(e.OldStartingIndex);
                            SelectedTabIndex--;
                        }
                        else if (e.OldStartingIndex == SelectedTabIndex && e.OldStartingIndex == Tabs.Count - 1)
                        {
                            SelectedTabIndex--;
                            Tabs.RemoveAt(e.OldStartingIndex);
                        }
                        else if (e.OldStartingIndex == SelectedTabIndex && e.OldStartingIndex != Tabs.Count - 1)
                        {
                            Tabs.RemoveAt(e.OldStartingIndex);
                            var tab = Tabs.ElementAtOrDefault(SelectedTabIndex);
                            tab.IsSelected = true;
                        }
                        else Tabs.RemoveAt(e.OldStartingIndex);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    {
                        var tab = Tabs.ElementAt(e.OldStartingIndex);
                        var newItem = e.NewItems[0];

                        InitContentTemplate(newItem, tab);

                        tab.Header = newItem;
                        tab.BindingContext = newItem;
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    {
                        foreach (var item in Tabs)
                        {
                            item.PropertyChanged -= TabItem_PropertyChanged;
                        }

                        Tabs.Clear();
                    }
                    break;
            }
        }

        private void Tabs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach (var item in e.NewItems)
                        {
                            var tabItem = item as TabViewItem;
                            tabItem.PropertyChanged += TabItem_PropertyChanged;

                            if (!(tabItem.Header is View))
                            {
                                tabItem.TabViewHeaderItem.InitContent();
                            }

                            if (_headersContainer != null) _headersContainer.Children.Insert(e.NewStartingIndex, tabItem.TabViewHeaderItem);
                            if (_contentContainer != null) _contentContainer.Children.Insert(e.NewStartingIndex, tabItem);
                        }

                        if (SelectedTabIndex == -1) SelectedTabIndex = 0;
                        else
                        {
                            if (!Tabs.Any(t => t.IsSelected))
                            {
                                var tab = Tabs.ElementAtOrDefault(SelectedTabIndex);
                                if (tab.IsEnabled) tab.IsSelected = true;
                            }
                            else
                            {
                                if (e.NewStartingIndex <= SelectedTabIndex)
                                {
                                    SelectedTabIndex++;
                                }
                            }
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    {
                        if (_headersContainer != null)
                        {
                            var header = _headersContainer.Children.ElementAt(e.OldStartingIndex);
                            _headersContainer.Children.Remove(header);
                            _headersContainer.Children.Insert(e.NewStartingIndex, header);
                        }

                        if (_contentContainer != null)
                        {
                            var content = _contentContainer.Children.ElementAt(e.OldStartingIndex);
                            _contentContainer.Children.Remove(content);
                            _contentContainer.Children.Insert(e.NewStartingIndex, content);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    {
                        if (_headersContainer != null) _headersContainer.Children.RemoveAt(e.OldStartingIndex);
                        if (_contentContainer != null) _contentContainer.Children.RemoveAt(e.OldStartingIndex);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    {
                        if (_headersContainer != null) _headersContainer.Children.Clear();
                        if (_contentContainer != null) _contentContainer.Children.Clear();
                        SelectedTabIndex = -1;
                    }
                    break;
            }
        }

        private void TabItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TabViewItem.IsSelectedProperty.PropertyName)
            {
                if (sender is TabViewItem tab && tab.IsSelected)
                {
                    SelectedTabIndex = Tabs.IndexOf(tab);
                    if (_headersScroll != null && ScrollToSelectedTab)
                    {
                        _headersScroll.ScrollToAsync(tab.TabViewHeaderItem.X - (_headersScroll.Width - tab.TabViewHeaderItem.Width) / 2, 0, true);
                    }
                }
            }
            else if (e.PropertyName == TabViewItem.IsEnabledProperty.PropertyName)
            {
                var tab = sender as TabViewItem;
                if (!tab.IsEnabled && tab.IsSelected) SelectClosestTab(tab, Tabs.Where(t => t.IsEnabled || t == tab).ToList());
                else if (tab.IsEnabled && !Tabs.Any(t => t.IsEnabled))
                {
                    var index = Tabs.IndexOf(tab);
                    SelectedTabIndex = index;
                }
            }
        }

        private void SelectClosestTab(TabViewItem tab, List<TabViewItem> tabs)
        {
            if (tabs.Count == 1) return;

            var index = tabs.IndexOf(tab);

            if (index == tabs.Count - 1)
            {
                var tabToSelect = tabs.ElementAt(index - 1);
                SelectedTabIndex = Tabs.IndexOf(tabToSelect);
            }
            else if (index >= 0)
            {
                var tabToSelect = tabs.ElementAt(index + 1);
                SelectedTabIndex = Tabs.IndexOf(tabToSelect);
            }
        }

        #region ScrollToSelectedTab
        public bool ScrollToSelectedTab
        {
            get { return (bool)GetValue(ScrollToSelectedTabProperty); }
            set { SetValue(ScrollToSelectedTabProperty, value); }
        }

        public static readonly BindableProperty ScrollToSelectedTabProperty =
            BindableProperty.Create(
                nameof(ScrollToSelectedTab),
                typeof(bool),
                typeof(TabView),
                true
                );
        #endregion

        #region ContentBackgroundColor
        public Color ContentBackgroundColor
        {
            get { return (Color)GetValue(ContentBackgroundColorProperty); }
            set { SetValue(ContentBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty ContentBackgroundColorProperty =
            BindableProperty.Create(
                nameof(ContentBackgroundColor),
                typeof(Color),
                typeof(TabView),
                Color.Transparent
                );
        #endregion

        #region HeaderBarBackgroundColor
        public Color HeaderBarBackgroundColor
        {
            get { return (Color)GetValue(HeaderBarBackgroundColorProperty); }
            set { SetValue(HeaderBarBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty HeaderBarBackgroundColorProperty =
            BindableProperty.Create(
                nameof(HeaderBarBackgroundColor),
                typeof(Color),
                typeof(TabView),
                Color.Transparent
                );
        #endregion

        #region SelectedHeaderBackgroundColor
        public Color SelectedHeaderBackgroundColor
        {
            get { return (Color)GetValue(SelectedHeaderBackgroundColorProperty); }
            set { SetValue(SelectedHeaderBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty SelectedHeaderBackgroundColorProperty =
            BindableProperty.Create(
                nameof(SelectedHeaderBackgroundColor),
                typeof(Color),
                typeof(TabView),
                Color.Accent
                );
        #endregion

        #region HeaderBackgroundColor
        public Color HeaderBackgroundColor
        {
            get { return (Color)GetValue(HeaderBackgroundColorProperty); }
            set { SetValue(HeaderBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty HeaderBackgroundColorProperty =
            BindableProperty.Create(
                nameof(HeaderBackgroundColor),
                typeof(Color),
                typeof(TabView),
                Color.Transparent
                );
        #endregion

        #region HeaderHeightRequest
        public double HeaderHeightRequest
        {
            get { return (double)GetValue(HeaderHeightRequestProperty); }
            set { SetValue(HeaderHeightRequestProperty, value); }
        }

        public static readonly BindableProperty HeaderHeightRequestProperty =
            BindableProperty.Create(
                nameof(HeaderHeightRequest),
                typeof(double),
                typeof(TabView),
                50.0
                );
        #endregion

        #region HeaderFontFamily
        public string HeaderFontFamily
        {
            get { return (string)GetValue(HeaderFontFamilyProperty); }
            set { SetValue(HeaderFontFamilyProperty, value); }
        }

        public static readonly BindableProperty HeaderFontFamilyProperty =
            BindableProperty.Create(
                nameof(HeaderFontFamily),
                typeof(string),
                typeof(TabView)
                );
        #endregion

        #region HeaderFontSize
        public double HeaderFontSize
        {
            get { return (double)GetValue(HeaderFontSizeProperty); }
            set { SetValue(HeaderFontSizeProperty, value); }
        }

        public static readonly BindableProperty HeaderFontSizeProperty =
            BindableProperty.Create(
                nameof(HeaderFontSize),
                typeof(double),
                typeof(TabView),
                14.0
                );
        #endregion

        #region HeaderFontAttributes
        public FontAttributes HeaderFontAttributes
        {
            get { return (FontAttributes)GetValue(HeaderFontAttributesProperty); }
            set { SetValue(HeaderFontAttributesProperty, value); }
        }

        public static readonly BindableProperty HeaderFontAttributesProperty =
            BindableProperty.Create(
                nameof(HeaderFontAttributes),
                typeof(FontAttributes),
                typeof(TabView),
                FontAttributes.None
                );
        #endregion

        #region HeaderTextColor
        public Color HeaderTextColor
        {
            get { return (Color)GetValue(HeaderTextColorProperty); }
            set { SetValue(HeaderTextColorProperty, value); }
        }

        public static readonly BindableProperty HeaderTextColorProperty =
            BindableProperty.Create(
                nameof(HeaderTextColor),
                typeof(Color),
                typeof(TabView),
                Color.Black
                );
        #endregion

        #region SelectedTabIndex
        public int SelectedTabIndex
        {
            get { return (int)GetValue(SelectedTabIndexProperty); }
            set { SetValue(SelectedTabIndexProperty, value); }
        }

        public static readonly BindableProperty SelectedTabIndexProperty =
            BindableProperty.Create(
                nameof(SelectedTabIndex),
                typeof(int),
                typeof(TabView),
                -1
                );
        #endregion

        #region SelectedHeaderTemplate
        public DataTemplate SelectedHeaderTemplate
        {
            get { return (DataTemplate)GetValue(SelectedHeaderTemplateProperty); }
            set { SetValue(SelectedHeaderTemplateProperty, value); }
        }

        public static readonly BindableProperty SelectedHeaderTemplateProperty =
            BindableProperty.Create(
                nameof(SelectedHeaderTemplate),
                typeof(DataTemplate),
                typeof(TabView)
                );
        #endregion

        #region HeaderTemplate
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly BindableProperty HeaderTemplateProperty =
            BindableProperty.Create(
                nameof(HeaderTemplate),
                typeof(DataTemplate),
                typeof(TabView)
                );
        #endregion

        #region ContentTemplate
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        public static readonly BindableProperty ContentTemplateProperty =
            BindableProperty.Create(
                nameof(ContentTemplate),
                typeof(DataTemplate),
                typeof(TabView)
                );
        #endregion

        #region ItemsSource
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(TabView)
                );
        #endregion

        #region SelectedTabChangedCommand
        public ICommand SelectedTabChangedCommand
        {
            get { return (ICommand)GetValue(SelectedTabChangedCommandProperty); }
            set { SetValue(SelectedTabChangedCommandProperty, value); }
        }

        public static readonly BindableProperty SelectedTabChangedCommandProperty =
            BindableProperty.Create(
                nameof(SelectedTabChangedCommand),
                typeof(ICommand),
                typeof(TabView)
                );
        #endregion

        #region SelectedTabChangedCommandParameter
        public object SelectedTabChangedCommandParameter
        {
            get { return (object)GetValue(SelectedTabChangedCommandParameterProperty); }
            set { SetValue(SelectedTabChangedCommandParameterProperty, value); }
        }

        public static readonly BindableProperty SelectedTabChangedCommandParameterProperty =
            BindableProperty.Create(
                nameof(SelectedTabChangedCommandParameter),
                typeof(object),
                typeof(TabView)
                );
        #endregion
    }
}
