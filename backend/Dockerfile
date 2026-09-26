FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY TaskTrack.Repo/TaskTrack.Repo.csproj TaskTrack.Repo/
COPY TaskTrack.Service/TaskTrack.Service.csproj TaskTrack.Service/
COPY TaskTrack.API/TaskTrack.API.csproj TaskTrack.API/
RUN dotnet restore TaskTrack.API/TaskTrack.API.csproj

COPY . .
RUN dotnet publish TaskTrack.API/TaskTrack.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "TaskTrack.API.dll"]
