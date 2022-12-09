[![Build Status](https://dev.azure.com/vdmkrchkn/ProductsWebAdmin/_apis/build/status/ProductsWeb?branchName=master)](https://dev.azure.com/vdmkrchkn/ProductsWebAdmin/_build/latest?definitionId=3&branchName=master)

## How to run project

There are admin panel and web-api services. If you want to debug the service, open it in your favourite IDE and choose the option.
To run the service in terminal you need to exec `dotnet run --project .\ProductsWeb[Api/Admin]\`.

## EF Migrations.

Projects prefer code-first paradigm. If you use the data, open the terminal and execute `dotnet ef 
database update --project .\Products.Web.Infrastructure\` in project root folder.