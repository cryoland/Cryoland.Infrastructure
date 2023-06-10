[RU](README.ru.md)

# Description <a name = "about"></a>

An assembly containing implementations of documents with a specific format (docx, xlsx, pdf)

## Prerequisites <a name = "prerequisites"></a>

<p>There were more and more types of documents, controller methods responsible for generating documents began to grow into hard-to-read and untestable spaghetti code.</p>
<p>The library is designed to minimize the risks identified above and focus only on working with the business implementation of the document, without delving into the features of its generation under the hood.</p>

## Getting Started <a name = "getting_started"></a>

<p>To implement a new type of document, you need to create a folder in the <b>Implementations</b> directory with the name of the document - <b>T</b>.</p>
<p>Next, you need to implement 3 classes with the following names:</p>
<ul>
   <li><b>T</b><i>Input</i> - input data object</li>
   <li><b>T</b><i>Processor</i> - handler</li>
   <li><b>T</b><i>Document</i> - concrete implementation of the document</li>
</ul>
<p>where <b>T</b> is the name of your <b>document type</b></p>

### TInput.cs
<p>This file describes the model that should contain the input data for the further formation of the document</p>

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
<p>In this file, the <b>TProcessor</b> type implements the <b>ITemplateProcessor<in TInput></b> interface, namely the <b>Process</b> method, and specifies the document type expected in the output in <b>Stream</b></p>

```
namespace Cryoland.Infrastructure.Documents.Implementations.T
{
     public class TProcessor : ITemplateProcessor<TInput>
     {
         public SupportedTypes Type => SupportedTypes.XLSX;

         public void Process(TInput input, Stream targetStream)
         {
           // Create a new document or modify an existing template

           // At the end of editing the document, save it to the stream:
           // document.Write(targetStream);
         }
     }
}
```

### TDocument.cs

<p>In this file, the <b>TDocument</b> type inherits the implementation of the <b>Create</b> method, which receives an object with input data <b>TInput</b></p>
<p>The only thing you need to do is define your implementation of <b>TDocument</b>:</p>

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

## Dependency Injection <a name = "di"></a>
<p>All private implementations of documents are automatically (using reflection) added to the DI container</p>
<p>Example of using the document in client code:</p>

```
...
[FromServices] InformationMessageDocument document,
[FromBody] InformationMessageInput dto,
...
var documentStream = document.Create(dto);
return new FileStreamResult(documentStream, document.Mime)
{
   FileDownloadName = $"Info letter {dto.InfoNoticeStaticFields.NowOffsetDate:dd.MM.yyyy__HH_mm}.docx",
};
```

## Optional <a name = "getting_started"></a>

<p>Existing files that will be used as templates for generating documents should be placed in the <b>Resources\Templates</b> directory. In addition, the new document should be added to the resource file. This is necessary so that the contents of the file are added to the assembly, and in the body of the processor implementation of your document type, you can already access the template as an array <b>byte[]</b>:</p>

```
public void Process(InfoMessagesLogbookInput input, Stream targetStream)
{
   using var templateStream = new MemoryStream(Templates.InfoMessagesLogbookTemplate, false);
   XSSFWorkbook document = new(templateStream);
   // operations with the document
   document.Write(targetStream, leaveOpen: true);
}
```

<p>Pictures need to be converted to files with the .bin extension, this is necessary so that the resource generator returns the image as <b>byte[]</b> instead of the type <b>System.Drawing.Bitmap</b></ p>

## Easter Egg <a name = "easter_egg"></a>

<p>In addition to document generation, document preview generation is also available using the IThumbnailService service. The input must be the Stream of the document, its type and the dimensions of the output image in pixels</p>