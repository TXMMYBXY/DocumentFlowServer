FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Set working directory to project folder to avoid including other projects' source files
WORKDIR /src/DocumentFlowServer.Worker

# Copy csproj and restore as distinct layers
COPY ["DocumentFlowServer.Worker/DocumentFlowServer.Worker.csproj", "./DocumentFlowServer.Worker.csproj"]
RUN dotnet restore "DocumentFlowServer.Worker.csproj"

# Copy only worker project sources to avoid including other projects (Api) into the build context
COPY DocumentFlowServer.Worker/. ./
RUN dotnet build "DocumentFlowServer.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DocumentFlowServer.Worker.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "DocumentFlowServer.Worker.dll"]
