[EN](README.md)

# Описание <a name = "about"></a>

Сборка, содержащая реализации документов с определенным форматом (docx, xlsx, pdf)

## Предпосылки <a name = "prerequisites"></a>

<p>Типов документов становилось все больше, методы контроллеров, отвечающие за генерацию документов, начали обрастать сложночитаемым и нетестируемым спагетти-кодом.</p>
<p>Библиотека разработана с целью минимизации обозначенных выше рисков и концентрации только на работе с бизнес-реализацией документа, не вникая в особенности его генерации под капотом.</p>

## С чего начать <a name = "getting_started"></a>

<p>Для реализации нового типа документа необходимо в каталоге <b>Implementations</b> создать папку с названием документа - <b>T</b>.</p>
<p>Далее требуется реализовать 3 класса со следующими наименованиями:</p>
<ul>
  <li><b>T</b><i>Input</i> - объект входных данных</li>
  <li><b>T</b><i>Processor</i> - обработчик</li>
  <li><b>T</b><i>Document</i> - конкретная реализация документа</li>
</ul>
<p>где <b>T</b> - наименование вашего <b>типа</b> документа</p>

### TInput.cs 
<p>В данном файле описывается модель, которая должна содержать входные данные для дальнейшего формирования документа</p>

```
namespace Cryoland.Infrastructure.Documents.Implementations.T
{
    public class TInput
    {
        public string Name { get; init; }
    }
}
```

### TProcessor.cs 
<p>В данном файле тип <b>TProcessor</b> реализует интерфейс <b>ITemplateProcessor<in TInput></b>, а именно: метод <b>Process</b> и задает тип документа, ожидаемый на выходе в потоке <b>Stream</b></p>

```
namespace Cryoland.Infrastructure.Documents.Implementations.T
{
    public class TProcessor : ITemplateProcessor<TInput>
    {
        public SupportedTypes Type => SupportedTypes.XLSX;

        public void Process(TInput input, Stream targetStream)
        {
          // Создание нового документа или модифицирование существующего шаблона

          // По окончанию редактирования документа, сохраняем его в поток:
          // document.Write(targetStream);
        }        
    }
}
```

### TDocument.cs

<p>В данном файле тип <b>TDocument</b> наследует реализацию метода <b>Create</b>, на вход которого подается объект с входными данными <b>TInput</b></p>
<p>Единственное, что требуется выполнить, это определить свою реализацию <b>TDocument</b>:</p>

```
namespace Cryoland.Infrastructure.Documents.Implementations.T
{
    public class TDocument : Document<TInput, TProcessor>
    {
        public TDocument(IPdfConverter pdfConverter) : base(pdfConverter)
        {
        }
    }
}
```

## Инъекция зависимостей <a name = "di"></a>
<p>Все частные реализации документов автоматически (с помощью рефлексии) добавляются в DI-контейнер</p>
<p>Пример использования документа в клиентском коде:</p>

```
...
[FromServices] InformationMessageDocument document,
[FromBody] InformationMessageInput dto,
...
var documentStream = document.Create(dto);
return new FileStreamResult(documentStream, document.Mime)
{
  FileDownloadName = $"Инфописьмо {dto.InfoNoticeStaticFields.NowOffsetDate:dd.MM.yyyy__HH_mm}.docx",
};
```

## Дополнительно <a name = "getting_started"></a>

<p>Имеющиеся файлы, которые будут использоваться в качестве шаблонов для формирования документов следует помещать в каталог <b>Resources\Templates</b>. Помимо этого новый документ следует добавить в файл ресурсов. Это нужно для того, чтобы содержимое файла добавилось в сборку, и в теле реализации процессора вашего типа докумета можно было обращаться к шаблону уже как массиву <b>byte[]</b>:</p>

```
public void Process(InfoMessagesLogbookInput input, Stream targetStream)
{
  using var templateStream = new MemoryStream(Templates.InfoMessagesLogbookTemplate, false);
  XSSFWorkbook document = new(templateStream);
  // операции с документом
  document.Write(targetStream, leaveOpen: true);
}
```

<p>Картинки требуется приводить к файлам с расширением .bin, это нужно для того, чтобы генератор ресурсов отдавал картинку как <b>byte[]</b> вместо типа <b>System.Drawing.Bitmap</b></p>

## Пасхальное Иичко <a name = "easter_egg"></a>

<p>Помимо генерации документов доступна также генерация превью документов с помощью сервиса IThumbnailService. На вход необходимо подать Stream документа, его тип и размеры выходного изображения в пикселях</p>
