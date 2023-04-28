zobrazení zprávy
await DisplayAlert("Alert", LangResources.LoginFailed, "OK"); 
heslo podpisového certifikátu je: Terminal

Windows App CERTIFICATION Kit - ověřuje aplikaci pro publikaci na windows Store - korektnost certifikátu

při chybě kompilace IOS smazat C:\Users\tatka\AppData\Local\Temp\Xamarin\XMA\Local

kompilace balíčku nastavení ad-hoc/iphone/vzdálené/ vytvoří v adhoc soubor IPA

stránka reportu
await Navigation.PushModalAsync(new NavigationPage(new SKReports.Report()));

nastavení barvy pro android 
v definici drawable-anydpi vyhodit  android:tint="?attr/colorControlNormal"

cancel/save na spodku obrayovky

<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="Terminal.Views.ServerAddressPage"
             xmlns:local="clr-namespace:Terminal.Singleton"
             xmlns:images="clr-namespace:Terminal.Images"
             xmlns:addOn="clr-namespace:Terminal"
             xmlns:language="clr-namespace:Terminal.Languages"
             Title="">
    <NavigationPage.TitleView>
        <StackLayout HeightRequest="48" Padding="0,0,10,0">
            <Grid HeightRequest="48">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="*" />
                    <ColumnDefinition Width="*" />
                    <ColumnDefinition Width="*" />
                </Grid.ColumnDefinitions>
                <Label x:Name="BackLabel" Grid.Column="0" Margin="0,8,0,0" HorizontalOptions="Start" Text="{x:Static language:LangResources.ldapServerIp}" LineBreakMode="NoWrap" FontSize="Medium" TextColor="{StaticResource NavigationTextColor}" />
                <Image Grid.Column="1" Source="{Binding OnlineStatusImage, Source={x:Static local:GlobalResources.Current}}" VerticalOptions="Center" HeightRequest="48" Aspect="AspectFit" Margin="0,5,0,0" HorizontalOptions="Center"/>
                <ImageButton Grid.Column="2" WidthRequest="48" Clicked="Cancel_Clicked" Source="{x:Static images:Images.cancelImage}" VerticalOptions="CenterAndExpand" HeightRequest="48" Aspect="AspectFit" HorizontalOptions="End" Margin="0,0,55,0"/>
                <ImageButton Grid.Column="2" WidthRequest="48" Clicked="Save_Clicked" Source="{x:Static images:Images.saveImage}" VerticalOptions="CenterAndExpand" HeightRequest="48" Aspect="AspectFit" HorizontalOptions="End"/>
            </Grid>
        </StackLayout>
    </NavigationPage.TitleView>
    <AbsoluteLayout HorizontalOptions="FillAndExpand" VerticalOptions="FillAndExpand">
        <ScrollView AbsoluteLayout.LayoutFlags="All" AbsoluteLayout.LayoutBounds="0,0,1,1">
        <ScrollView.Content>
            <Grid>
                <Grid.RowDefinitions>
                    <RowDefinition />
                    <RowDefinition Height="Auto" />
                </Grid.RowDefinitions>
                <StackLayout Grid.Row="0" Spacing="20" Padding="15,15,15,65">
                    <Label Text="{x:Static language:LangResources.ldapServerIp}" FontSize="Medium" />
                    <Entry x:Name="ldapServerIp" Text="{Binding Item.ldapServerIp}" FontSize="Small" />
                    <Label Text="{x:Static language:LangResources.ldapPort}" FontSize="Medium" />
                    <Entry x:Name="ldapPort" MaxLength="5" Text="{Binding Item.ldapPort}" Keyboard="Numeric" FontSize="Small" />
                    <Label Text="{x:Static language:LangResources.ldapDN}" FontSize="Medium" />
                    <Entry x:Name="ldapDN" MaxLength="100" Text="{Binding Item.ldapDN}" FontSize="Small" />
                    <Label Text="{x:Static language:LangResources.roleDN}" FontSize="Medium" />
                    <Entry x:Name="roleDN" MaxLength="100" Text="{Binding Item.roleDN}" FontSize="Small" />
                    <Label Text="{x:Static language:LangResources.RefreshInterval}" FontSize="Medium" />
                    <Entry x:Name="RefreshInterval" MaxLength="2" Text="{Binding Item.refreshInterval}" Keyboard="Numeric" FontSize="Small" Margin="0" >
                        <Entry.Behaviors>
                            <addOn:NumericValidationBehavior />
                        </Entry.Behaviors>
                    </Entry>
                </StackLayout>
            </Grid>
        </ScrollView.Content>
            
    </ScrollView>
        <Grid HeightRequest="48" Grid.Row="1" Padding="15,15,15,0"  AbsoluteLayout.LayoutFlags="PositionProportional,WidthProportional"  AbsoluteLayout.LayoutBounds="0,1,1,Autosize">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="*" />
                <ColumnDefinition Width="*" />
            </Grid.ColumnDefinitions>
            <ImageButton  Grid.Column="0" CornerRadius="8" BackgroundColor="{StaticResource ButtonBackground}" WidthRequest="48" Clicked="Cancel_Clicked" Source="{x:Static images:Images.cancelImage}" VerticalOptions="StartAndExpand" HeightRequest="48" Aspect="AspectFit" HorizontalOptions="Fill"/>
            <ImageButton  Grid.Column="1" CornerRadius="8" BackgroundColor="{StaticResource IconColor}" Clicked="Save_Clicked" Source="{x:Static images:Images.saveImage}" VerticalOptions="CenterAndExpand" HeightRequest="48" Aspect="AspectFit" HorizontalOptions="Fill"/>
        </Grid>
    </AbsoluteLayout>
</ContentPage>
    

cancel/save na konci formuláře
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="Terminal.Views.ServerAddressPage"
             xmlns:local="clr-namespace:Terminal.Singleton"
             xmlns:images="clr-namespace:Terminal.Images"
             xmlns:addOn="clr-namespace:Terminal"
             xmlns:language="clr-namespace:Terminal.Languages"
             Title="">
    <NavigationPage.TitleView>
        <StackLayout HeightRequest="48" Padding="0,0,10,0">
            <Grid HeightRequest="48">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="*" />
                    <ColumnDefinition Width="*" />
                    <ColumnDefinition Width="*" />
                </Grid.ColumnDefinitions>
                <Label x:Name="BackLabel" Grid.Column="0" Margin="0,8,0,0" HorizontalOptions="Start" Text="{x:Static language:LangResources.ldapServerIp}" LineBreakMode="NoWrap" FontSize="Medium" TextColor="{StaticResource NavigationTextColor}" />
                <Image Grid.Column="1" Source="{Binding OnlineStatusImage, Source={x:Static local:GlobalResources.Current}}" VerticalOptions="Center" HeightRequest="48" Aspect="AspectFit" Margin="0,5,0,0" HorizontalOptions="Center"/>
                <ImageButton Grid.Column="2" WidthRequest="48" Clicked="Cancel_Clicked" Source="{x:Static images:Images.cancelImage}" VerticalOptions="CenterAndExpand" HeightRequest="48" Aspect="AspectFit" HorizontalOptions="End" Margin="0,0,55,0"/>
                <ImageButton Grid.Column="2" WidthRequest="48" Clicked="Save_Clicked" Source="{x:Static images:Images.saveImage}" VerticalOptions="CenterAndExpand" HeightRequest="48" Aspect="AspectFit" HorizontalOptions="End"/>
            </Grid>
        </StackLayout>
    </NavigationPage.TitleView>
    <ScrollView>
        <ScrollView.Content>
            <Grid>
                <Grid.RowDefinitions>
                    <RowDefinition />
                    <RowDefinition Height="Auto" />
                </Grid.RowDefinitions>
                <StackLayout Grid.Row="0" Spacing="20" Padding="15,15,15,65">
                    <Label Text="{x:Static language:LangResources.ldapServerIp}" FontSize="Medium" />
                    <Entry x:Name="ldapServerIp" Text="{Binding Item.ldapServerIp}" FontSize="Small" />
                    <Label Text="{x:Static language:LangResources.ldapPort}" FontSize="Medium" />
                    <Entry x:Name="ldapPort" MaxLength="5" Text="{Binding Item.ldapPort}" Keyboard="Numeric" FontSize="Small" />
                    <Label Text="{x:Static language:LangResources.ldapDN}" FontSize="Medium" />
                    <Entry x:Name="ldapDN" MaxLength="100" Text="{Binding Item.ldapDN}" FontSize="Small" />
                    <Label Text="{x:Static language:LangResources.roleDN}" FontSize="Medium" />
                    <Entry x:Name="roleDN" MaxLength="100" Text="{Binding Item.roleDN}" FontSize="Small" />
                    <Label Text="{x:Static language:LangResources.RefreshInterval}" FontSize="Medium" />
                    <Entry x:Name="RefreshInterval" MaxLength="2" Text="{Binding Item.refreshInterval}" Keyboard="Numeric" FontSize="Small" Margin="0" >
                        <Entry.Behaviors>
                            <addOn:NumericValidationBehavior />
                        </Entry.Behaviors>
                    </Entry>
                </StackLayout>
                <AbsoluteLayout HorizontalOptions="FillAndExpand" VerticalOptions="FillAndExpand">
                    <Grid HeightRequest="48" Grid.Row="1" Padding="15,15,15,0"  AbsoluteLayout.LayoutFlags="PositionProportional,WidthProportional"  AbsoluteLayout.LayoutBounds="0,1,1,Autosize">
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="*" />
                            <ColumnDefinition Width="*" />
                        </Grid.ColumnDefinitions>
                        <ImageButton  Grid.Column="0" CornerRadius="8" BackgroundColor="{StaticResource ButtonBackground}" WidthRequest="48" Clicked="Cancel_Clicked" Source="{x:Static images:Images.cancelImage}" VerticalOptions="StartAndExpand" HeightRequest="48" Aspect="AspectFit" HorizontalOptions="Fill"/>

                        <ImageButton  Grid.Column="1" CornerRadius="8" BackgroundColor="{StaticResource IconColor}" Clicked="Save_Clicked" Source="{x:Static images:Images.saveImage}" VerticalOptions="CenterAndExpand" HeightRequest="48" Aspect="AspectFit" HorizontalOptions="Fill"/>
                    </Grid>
                </AbsoluteLayout>
            </Grid>
        </ScrollView.Content>
    </ScrollView>
</ContentPage>
    



    -----------------------------------------
    odinstalace z Windows PowerShell 
    Get-AppxPackage  -AllUsers
    Remove-AppxPackage "996e5667-cb5c-4a30-b4d5-a3982de697ab_1.0.0.0_x86__wceetrvrp10yt" -AllUsers - používá se fullname


