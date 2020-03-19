# Xamarin.Controls.TabView
Fully customizable Xamarin.Forms TabView

It is written using Xamarin.Forms without native code and it is fully compatible with all platforms Xamarin.Forms supports


The nuget package: https://www.nuget.org/packages/Xamarin.Controls.TabView

### Initialize:
Make sure to initialize the UI of the TabView inside your App.xaml file:

    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                
                <tabview:TabViewStyle />
                
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>

### Usage:

You can create tabs from markup or from ViewModel specifying ItemsSource and describing DataTemplate and header templates in xaml.
Note: ItemsSource has bigger priority than directly markup tab creation.

### Example of usage:

Here described two types of usage:
https://github.com/Alex-Dobrynin/Xamarin.Controls.TabView/blob/master/Sample.TabVIew/Sample.TabVIew/Views/MainPage.xaml

### Note:
You cannot use typeof Page as tab template.
