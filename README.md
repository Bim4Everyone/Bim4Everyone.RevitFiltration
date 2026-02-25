# Bim4Everyone.RevitFiltration

Bim4Everyone.RevitFiltration — это библиотека C#, разработанная для упрощения создания `ElementFilter`. Библиотека
состоит из двух основных частей: **RevitFiltration** и **RevitFiltration.Controls**.
RevitFiltration.Controls можно использовать для создания `ElementFilter`, передавая его непосредственно в метод
`WherePasses` класса `FilteredElementCollector`.
RevitFiltration.Controls предоставляет пользователю возможность создавать пользовательские фильтры непосредственно в UI,
сохранять их и использовать повторно.

Контрол с возможностью выбрать категории фильтрации и задать правила для параметров:

<img src="./screenshots/DynamicCategoriesControl.png" width="800">

Контрол с возможностью только задавать правила параметров:

<img src="./screenshots/PresetCategoriesControl.png" width="600">

## Bim4Everyone.RevitFiltration

### Необходимые зависимости

- pyRevitLabs.Json
- Ninject
- dosymep.Revit
- dosymep.Bim4Everyone

### Подключение к плагину

1. Подключить ссылку на Bim4Everyone.RevitFiltration.dll в .csproj:

```
<ItemGroup Condition="$(RevitVersion) != ''">
    <Reference Include="Bim4Everyone.RevitFiltration">
        <HintPath>$(AppData)\pyRevit\Extensions\BIM4Everyone.lib\dosymep_libs\libs\$(RevitVersion)\Bim4Everyone.RevitFiltration.dll</HintPath>
        <Private>False</Private>
    </Reference>
</ItemGroup>
```

2. Зарегистрировать в ninject DI контейнере плагина необходимые сервисы:

```
kernel.UseLogicalFilterFactory(); // сервис для создания фильтра (обязательно)
kernel.UseLogicalFilterParser(); // сервис для сериализации и десериализации фильтра (опционально)
```

3. Реализовать `IOptions` для настроек генерации фильтра.

### Пример использования в плагине

Из DI контейнера необходимо получить сервис `ILogicalFilterFactory`, затем сконструировать необходимый фильтр и
сгенерировать `ElementFilter`, используя класс, реализующий `IOptions`. Пример:

```
public void SampleFilterCreation(ILogicalFilterFactory filterFactory, Autodesk.Revit.UI.UIDocument uiDoc) {
    ILogicalFilter filter = filterFactory.CreateAndFilter()
        .AddEqualsRule(BuiltInParameter.SCHEDULE_LEVEL_PARAM, "Level 1");

    IOptions opts = new DefaultOptions();
    var elements = new FilteredElementCollector(uiDoc.Document)
        .WhereElementIsNotElementType()
        .OfCategory(BuiltInCategory.OST_Planting)
        .WherePasses(filter.Build(uiDoc.Document, opts))
        .ToElementIds();
    uiDoc.Selection.SetElementIds(elements);
}

public void SampleFilterParsing(ILogicalFilter filter, ILogicalFilterParser parser) {
    string str = parser.Serialize(filter);

    bool success = parser.TryParse(str, out ILogicalFilter newFilter);
}
```

## Bim4Everyone.RevitFiltration.Controls

### Необходимые зависимости

То же, что и для Bim4Everyone.RevitFiltration и:

- WPF-UI

### Подключение к плагину

1. Подключить ссылки на Bim4Everyone.RevitFiltration.dll и Bim4Everyone.RevitFiltration.Controls.dll в .csproj
2. Зарегистрировать в ninject DI контейнере плагина необходимые сервисы:

```
kernel.UseLogicalFilterFactory(); // сервис для создания ILogicalFilter (обязательно)
kernel.UseDefaultProviderFactory(); // сервис для привязки провайдера контекста фильтра из UI к ViewModel (обязательно)
kernel.UseDefaultContextParser(); // сервис для сериализации и десериализации контекста фильтра UI (опционально)
```

3. Реализовать `IOptions`, `IDataProvider`.

Пример реализации `IDtaProvider`:

```
internal class FilterDataProvider : IDataProvider {
    private readonly Document _doc;

    public FilterDataProvider(Document doc) {
        _doc = doc;
    }

    public ICollection<RevitParam> GetParams(ICollection<Category> categories) {
        return ParameterFilterUtilities
            .GetFilterableParametersInCommon(_doc, [..categories.Select(c => c.Id)])
            .Select(GetFilterableParam)
            .Where(p => p != null)
            .ToArray();
    }

    public ICollection<Category> GetCategories() {
        return ParameterFilterUtilities.GetAllFilterableCategories()
            .Select(c => Category.GetCategory(_doc, c))
            .Where(category => category != null)
            .Where(c => c.CategoryType == CategoryType.Model && c.IsVisibleInUI)
            .ToArray();
    }

    public ICollection<Document> GetDocuments() {
        return [_doc];
    }

    private RevitParam GetFilterableParam(ElementId paramId) {
        try {
            if(paramId.IsSystemId()) {
                return SystemParamsConfig.Instance.CreateRevitParam(
                        _doc,
                        (BuiltInParameter) paramId.GetIdValue());
            }
            
            var element = _doc.GetElement(paramId);
            if(element is SharedParameterElement sharedParameterElement) {
                return SharedParamsConfig.Instance.CreateRevitParam(
                        _doc,
                        sharedParameterElement.Name);
            }

            if(element is ParameterElement parameterElement) {
                return ProjectParamsConfig.Instance.CreateRevitParam(_doc, parameterElement.Name);
            }
            return null;
        } catch(Exception) {
            return null;
        }
    }
}
```

4. Настроить ViewModel окна:

```
internal class YourViewModel {
    public YourViewModel(
        ILogicalFilterProviderFactory filterProviderFactory,
        ILanguageService languageService,
        IDataProvider dataProvider) {
        FilterProvider = filterProviderFactory.Create(dataProvider)
    }

    public ILogicalFilterProvider FilterProvider { get; } // провайдер для получения фильтра из UI
    public ILanguageService LanguageService { get; } // сервис для установки локализации в контроле
}
```

5. Подключить нужный контрол в xaml:

```
xmlns:filtration="clr-namespace:Bim4Everyone.RevitFiltration.Controls.Views;assembly=Bim4Everyone.RevitFiltration.Controls"
<filtration:DynamicCategoriesFilterControl
    LanguageService="{Binding LanguageService}"
    LogicalFilterProvider="{Binding FilterProvider}" />
```

## Сборка проекта

Компиляция проекта в папку `bin`

```
nuke compile
```

Компиляция проекта в `Bim4Everyone.lib\dosymep_libs\libs`

```
nuke publish
```

## Генерация документации

Для запуска необходима установка [docfx](https://dotnet.github.io/docfx/)

```
nuke docs-compile
```

## Запуск тестов

```
nuke test
```
