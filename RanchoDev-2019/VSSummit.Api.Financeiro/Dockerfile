FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["VSSummit.Api.Financeiro/VSSummit.Api.Financeiro.csproj", "VSSummit.Api.Financeiro/"]
RUN dotnet restore "VSSummit.Api.Financeiro/VSSummit.Api.Financeiro.csproj"
COPY . .
WORKDIR "/src/VSSummit.Api.Financeiro"
RUN dotnet build "VSSummit.Api.Financeiro.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "VSSummit.Api.Financeiro.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VSSummit.Api.Financeiro.dll"]