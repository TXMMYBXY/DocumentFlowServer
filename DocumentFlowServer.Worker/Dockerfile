FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["DocumentFlowService.csproj", "."]
RUN dotnet restore "DocumentFlowService.csproj"

COPY . .
RUN dotnet build "DocumentFlowService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DocumentFlowService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "DocumentFlowService.dll"]