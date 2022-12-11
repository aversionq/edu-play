#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["EduPlay.WebAPI/EduPlay.WebAPI.csproj", "EduPlay.WebAPI/"]
COPY ["EduPlay.BLL/EduPlay.BLL.csproj", "EduPlay.BLL/"]
COPY ["EduPlay.DAL/EduPlay.DAL.csproj", "EduPlay.DAL/"]
RUN dotnet restore "EduPlay.WebAPI/EduPlay.WebAPI.csproj"
COPY . .
WORKDIR "/src/EduPlay.WebAPI"
RUN dotnet build "EduPlay.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EduPlay.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EduPlay.WebAPI.dll"]