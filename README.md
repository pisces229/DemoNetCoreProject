# DemoNetCoreProject
Development Tool: Visual Studio 2022<br>
.Net Core Version: 6<br>
範例方案包含以下專案<br>
* Common
* DataLayer
* BusinessLayer
* Backend
* Batch
* IntegrationTest
* UnitTest
## Common 共用層
專案相依：無 <br>
分層結構 <br>
* Constants
> 說明：常數 <br>
> 命名規則：\[Name\]***Constant*** <br>
* Dtos
> 說明：公用資料結構 <br>
> 命名規則：***Common***\[Name\]***Dto*** <br>
* Enums
> 說明：列舉 <br>
> 命名規則：\[Name\]***Type*** <br>
* Options
> 說明：設定檔結構 <br>
> 命名規則：\[Name\]***Option*** <br>
* Utilites
> 說明：工具 <br>
> 命名規則：\[Name\]***Utility*** <br>
## DataLayer 資料存取層
專案相依：Common <br>
負責資料存取服務 <br>
分層結構 <br>
* DtoMappers
> 說明：資料結構轉換 <br>
> 命名規則：\[Name\]***AutoMapper*** <br>
* Dtos
> 說明：資料結構 <br>
> 命名規則：\[Name\]***Repository***\[Input/Output\]***Dto*** <br>
* Entities
> 說明：資料表物件 <br>
> 命名規則：\[Name\]<br>
* IServices
> 說明：存取服務介面 <br>
> 命名規則：***I***\[Name\] <br>
* Services
> 說明：存取服務實作 <br>
> 命名規則：\[Name\] <br>
* IRepositories
> 說明：儲存庫介面 <br>
> 命名規則：***I***\[Name\]***Repository*** <br>
* Repositories
> 說明：儲存庫實作 <br>
> 命名規則：\[Name\]***Repository*** <br>
* Utilites
> 說明：工具 <br>
> 命名規則：\[Name\]***Utility*** <br>
## BusinessLayer 邏輯處理層
專案相依：Common、DataLayer <br>
負責處理業務邏輯 <br>
分層結構 <br>
* DtoMappers
> 說明：資料結構轉換 <br>
> 命名規則：\[Name\]***AutoMapper*** <br>
* Dtos
> 說明：資料結構 <br>
> 命名規則：\[Name\]***Logic***\[Input/Output\]***Dto*** <br>
* ILogics
> 說明：邏輯介面 <br>
> 命名規則：***I***\[Name\]***Logic*** <br>
* Logics
> 說明：邏輯實作 <br>
> 命名規則：\[Name\]***Logic*** <br>
* Registers
> 說明：邏輯服務註冊 <br>
> 命名規則：\[Name\]***Register*** <br>
## Backend Web服務層
專案相依：Domain、DataLayer、BusinessLayer <br>
分層結構 <br>
* Controllers
> 說明：服務進入點 <br>
> 命名規則：\[Name\]***Controller*** <br>
* ModelMappers
> 說明：資料結構轉換 <br>
> 命名規則：\[Name\]***AutoMapper*** <br>
* Models
> 說明：資料結構轉換 <br>
> 命名規則：\[Name\]***Model*** <br>
* Filters
> 說明：過濾器 <br>
> 命名規則：\[Name\]\[Type\]***Filter*** <br>
* Middlewares
> 說明：服務請求與回應攔截 <br>
> 命名規則：\[Name\]***Middleware*** <br>
* Services
> 說明：服務實作 <br>
> 命名規則：\[Name\]***Service*** <br>
* Validations
> 說明：模型驗證 <br>
> 命名規則：\[Name\]***Attribute*** <br>
## Batch 批次程式
專案相依：Common、DataLayer、BusinessLayer <br>
分層結構 <br>
* Runners
> 服務進入點 <br>
> 命名規則：\[Name\]***Runner*** <br>
* Services
> 說明：服務實作 <br>
> 命名規則：\[Name\]***Service*** <br>
## 整合測試
### [ASP.NET Core 中的整合測試](https://learn.microsoft.com/zh-tw/aspnet/core/test/integration-tests?view=aspnetcore-6.0)
* Backend
> 說明：Web服務層測試 <br>
* BusinessLayer
> 說明：邏輯處理層測試 <br>
* DataLayer
> 說明：資料存取層測試 <br>
## 單元測試
* BusinessLayer
> 說明：邏輯處理層測試 <br>
* Common
> 說明：共用層測試 <br>
* DataLayer
> 說明：資料存取層測試 <br>
## Unit Test 3A 原則
* Arrange – 初始化
* Act – 行為，測試對象的執行過程
* Assert – 驗證結果

## [dotnet dev-certs](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-dev-certs)

## Command (For Windwos Environment)

`dotnet test DemoNetCoreProject.UnitTest`

`dotnet build DemoNetCoreProject.Backend -c Release /p:DeployOnBuild=true /p:PublishProfile=Publish /p:EnvironmentName=Production`

`dotnet publish DemoNetCoreProject.Batch -c Release /p:PublishProfile=Publish`

## Environment Variables

### WINDOWS
`set ASPNETCORE_ENVIRONMENT=Development`

### LINUX
`export ASPNETCORE_ENVIRONMENT=Development`

`dotnet <Name>.dll <Arguments>`

`<Name>.exe <Arguments>`

## Diagrams(https://www.diagrams.net/)

## [Draw.io Integration](https://marketplace.visualstudio.com/items?itemName=hediet.vscode-drawio)

* DemoNetCoreProject.drawio
