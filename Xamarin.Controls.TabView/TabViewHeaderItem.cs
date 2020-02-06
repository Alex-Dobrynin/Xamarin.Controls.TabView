using System.Runtime.CompilerServices;

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
            }

            if (HeaderContent is View) return;

            else if (propertyName == IsSelectedProperty.PropertyName)
            {
                if (IsSelected) this.SetBinding(View.BackgroundColorProperty, new TemplateBinding("SelectedHeaderBackgroundColor"));
                else this.SetBinding(View.BackgroundColorProperty, new TemplateBinding("HeaderBackgroundColor"));

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
}
