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

Оновлення

Я хотів зробити завдання на ноутбуки який взял в друга тому що мій на ремонті, але сьогодні чогось було дуже мало світла, і коли я почав робити завдання то світло відключили. Я зробив завдання в github, але скріншоти не зміг зробити тому що дуже мало часу залишилось, і забув добавити пояснення.
