FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["my-portfolio-mvc/my-portfolio-mvc.csproj", "my-portfolio-mvc/"]

RUN dotnet restore "./my-portfolio-mvc/my-portfolio-mvc.csproj"

COPY . .

WORKDIR "/src/my-portfolio-mvc"

RUN dotnet publish "./my-portfolio-mvc.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "my-portfolio-mvc.dll"]