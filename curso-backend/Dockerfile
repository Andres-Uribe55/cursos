FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar archivos de proyecto y restaurar
COPY src/CoursePlatform.Domain/*.csproj src/CoursePlatform.Domain/
COPY src/CoursePlatform.Application/*.csproj src/CoursePlatform.Application/
COPY src/CoursePlatform.Infrastructure/*.csproj src/CoursePlatform.Infrastructure/
COPY src/CoursePlatform.API/*.csproj src/CoursePlatform.API/
RUN dotnet restore src/CoursePlatform.API/CoursePlatform.API.csproj

# Copiar el resto y publicar
COPY . .
RUN dotnet publish src/CoursePlatform.API/CoursePlatform.API.csproj -c Release -o /out

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CoursePlatform.API.dll"]
