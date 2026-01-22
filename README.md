# Bim4Everyone.RevitFiltration

Bim4Everyone.RevitFiltration — это библиотека C#, разработанная для упрощения создания `ElementFilter`. Библиотека состоит из двух основных частей: **RevitFiltration** и **RevitFiltration.Controls**.
RevitFiltration.Controls можно использовать для создания `ElementFilter`, передавая его непосредственно в метод `WherePasses` класса `FilteredElementCollector`.
RevitFiltration.Controls предоставляет пользователю возможность создавать пользовательские фильтры непосредственно в UI, сохранять их и использовать повторно.

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
kernel.UseLogicalFilterParser(); // сервис для сохранения и загрузки фильтра (опционально)
```

3. Реализовать интерфейс `IOptions` для генерации фильтра. Пример реализации:

```
internal class DefaultOptions : IOptions {
    public DefaultOptions() {
        Tolerance = 0.001;
    }

    public double Tolerance { get; set; }
}
```

### Пример использования в плагине

Из DI контейнера необходимо получить сервис `ILogicalFilterFactory`, затем сконструировать необходимый фильтр и сгенерировать `ElementFilter`, используя класс, реализующий `IOptions`. Пример:

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

## Сборка проекта

Компиляция проекта в папку `bin`

```
nuke compile
```