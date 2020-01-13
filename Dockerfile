FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
EXPOSE 8080

COPY XeroRefactoredApp/bin/Debug/netcoreapp3.0 /app
WORKDIR /app

ENTRYPOINT ["dotnet","XeroRefactoredApp.dll"]
