FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["AspNetCore.AsyncRequests/AspNetCore.AsyncRequests.csproj", "AspNetCore.AsyncRequests/"]
RUN dotnet restore "AspNetCore.AsyncRequests/AspNetCore.AsyncRequests.csproj"
COPY . .
WORKDIR "/src/AspNetCore.AsyncRequests"
RUN dotnet build "AspNetCore.AsyncRequests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AspNetCore.AsyncRequests.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspNetCore.AsyncRequests.dll"]