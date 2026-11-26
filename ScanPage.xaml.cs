using QrScanner.ViewModels;
using ZXing.Net.Maui;

namespace QrScanner.Views;

public partial class ScanPage : ContentPage
{
    private readonly ScanViewModel _viewModel;

    public ScanPage(ScanViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;

        Reader.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.TwoDimensional,
            AutoRotate = true,
            Multiple = false
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
            status = await Permissions.RequestAsync<Permissions.Camera>();

        if (status == PermissionStatus.Granted)
            _viewModel.Resume();
        else
            await DisplayAlert("Камера", "Для сканування потрібен дозвіл на використання камери.", "OK");
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.Pause();
    }

    // Тонкий адаптер події бібліотеки -> команда ViewModel (бізнес-логіки тут немає)
    private void OnBarcodeDetected(object? sender, BarcodeDetectionEventArgs e)
    {
        var value = e.Results?.FirstOrDefault()?.Value;
        if (string.IsNullOrWhiteSpace(value)) return;

        MainThread.BeginInvokeOnMainThread(() => _viewModel.ProcessScanCommand.Execute(value));
    }
}
