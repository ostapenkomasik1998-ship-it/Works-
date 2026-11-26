# QR Scanner (.NET MAUI + ZXing.Net.Maui)

Застосунок для зчитування QR-кодів камерою пристрою.

## Функціонал
- сканування QR-кодів (ZXing.Net.Maui, `CameraBarcodeReaderView`);
- збереження кожного результату в історію (JSON-файл у сховищі застосунку);
- сторінка «Історія»: перегляд, видалення запису, очищення всієї історії;
- якщо вміст є посиланням (http/https, також `www.`) — кнопка «Відкрити в браузері» (на екрані сканування та в історії).

## Архітектура: MVVM
| Шар | Файли |
|---|---|
| Model | `Models/ScanRecord.cs` |
| View | `Views/*.xaml` (+ тонкий code-behind) |
| ViewModel | `ViewModels/ScanViewModel.cs`, `HistoryViewModel.cs` (CommunityToolkit.Mvvm) |
| Services | `Services/*` — історія, відкриття посилань, діалоги, розбір посилань |

Принципи: SRP (кожен клас має одну відповідальність), DIP (ViewModel залежить від інтерфейсів `IHistoryService`, `ILinkOpener`, `IDialogService`), впровадження залежностей через DI-контейнер MAUI, `LinkParser` — чиста функція, зручна для модульного тестування.

## Збірка
Потрібно: .NET 8 SDK, workload `maui-android`.
```
dotnet workload install maui-android
dotnet build QrScanner/QrScanner.csproj -f net8.0-android -t:Run
```
Або Visual Studio 2022 → обрати Android-пристрій/емулятор → Run.
APK також збирається в GitHub Actions (`.github/workflows/build-apk.yml`).
