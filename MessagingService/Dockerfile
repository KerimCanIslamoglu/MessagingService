#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MessagingService/MessagingService.csproj", "MessagingService/"]
COPY ["MessagingService.Business/MessagingService.Business.csproj", "MessagingService.Business/"]
COPY ["MessagingService.Entities/MessagingService.Entities.csproj", "MessagingService.Entities/"]
COPY ["MessagingService.DataAccess/MessagingService.DataAccess.csproj", "MessagingService.DataAccess/"]
RUN dotnet restore "MessagingService/MessagingService.csproj"
COPY . .
WORKDIR "/src/MessagingService"
RUN dotnet build "MessagingService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MessagingService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MessagingService.dll"]
