FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CCETest.csproj", ""]
RUN dotnet restore "./CCETest.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "CCETest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CCETest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CCETest.dll"]